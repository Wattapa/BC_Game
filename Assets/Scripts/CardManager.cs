using ChainSafe.Gaming.Evm.Contracts.Custom;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;
using Scripts.EVM.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [Header("Contract Setup")]
    [SerializeField] private string contractAddress;
    [SerializeField] private CardDisplayManager cardDisplayManager;
    [SerializeField] private int mintPrice;
    [SerializeField] private long gasLimit = 5000000;
    [SerializeField] private long gasPrice = 50000000000;

    private CardGameNFT contract;
    private Web3 web3;
    private string playerAddress;
    private BigInteger mintPriceBI;

    public static CardManager Instance;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    private async void Start()
    {
        Web3Unity.Web3Initialized += OnWeb3Initialized;
    }

    private async void OnDestroy()
    {
        if (contract != null)
            await contract.DisposeAsync();

        Web3Unity.Web3Initialized -= OnWeb3Initialized;
    }

    private async void OnWeb3Initialized((Web3 web3, bool isLightweight) obj)
    {
        web3 = obj.web3;
        playerAddress = web3.Signer.PublicAddress;

        contract = await web3.ContractBuilder.Build<CardGameNFT>(contractAddress);
        await contract.InitAsync();

        Debug.Log("Contrat initialisé.");
        Debug.Log($"Adresse du contrat : {contract.Address}");

        // Événements
        contract.OnCardMinted += OnCardMinted;
        contract.OnCardListedForSale += OnCardListedForSale;
        contract.OnCardSaleCancelled += OnCardSaleCancelled;
        contract.OnCardPurchased += OnCardPurchased;
    }

    #region Methods
    public async void ChangeMintPrice()
    {
        try
        {
            mintPriceBI = mintPrice;

            var receipt = await contract.SetMintPriceWithReceipt(mintPriceBI, GetTxParams());

            Debug.Log($"Prix de mintage modifié à {mintPriceBI} wei. Tx : {receipt.TransactionHash}");
        } catch (Exception e)
        {
            Debug.LogError("Erreur lors du changement de prix de mintage : " + e.Message);
        }
    }

    public async void LoadMyCards()
    {
        try
        {
            var cards = await contract.GetCardsOfDetailed(playerAddress);
            Debug.Log($"Nombre de cartes : {cards.Count}");
            cardDisplayManager.DisplayCards(cards);
        } catch (Exception e)
        {
            Debug.LogError("Erreur lors du chargement des cartes : " + e.Message);
            Debug.LogError("Stacktrace complète : " + e.StackTrace);
        }
    }

    public async void MintCard()
    {
        try
        {
            MintLoadingUI.Instance.Show("Minting card...");
            // On récupère le prix de mintage (en wei) à partir du contrat
            mintPriceBI = await contract.GetMintPrice();

            CardAttributes card = CardCreator.Instance.CreateRandomCard();

            var receipt = await contract.MintCardWithReceipt(card.TokenURI, card.Name, card.Rarity, card.Description, GetTxParams(mintPriceBI));

            Debug.Log($"Carte mintée ! Tx : {receipt.TransactionHash}");
        } catch (Exception e)
        {
            Debug.LogError("Erreur lors du mint : " + e.Message);
        } finally
        {
            MintLoadingUI.Instance.Hide();
        }
    }

    public async void SellCard(BigInteger tokenId, BigInteger price)
    {
        try
        {
            var receipt = await contract.PutCardForSaleWithReceipt(tokenId, price, GetTxParams());
            Debug.Log($"Carte listée à {price} wei. Tx : {receipt.TransactionHash}");
        } catch (Exception e)
        {
            Debug.LogError("Erreur lors de la mise en vente : " + e.Message);
        }
    }

    public async void CancelSale(BigInteger tokenId)
    {
        try
        {
            var receipt = await contract.CancelSaleWithReceipt(tokenId, GetTxParams());
            Debug.Log($"Vente annulée. Tx : {receipt.TransactionHash}");
        } catch (Exception e)
        {
            Debug.LogError("Erreur lors de l'annulation de vente : " + e.Message);
        }
    }

    public async void BuyCard(BigInteger tokenId)
    {
        try
        {
            var (isForSale, seller, price) = await contract.GetSaleInfo(tokenId);

            if (isForSale)
            {
                var receipt = await contract.BuyCardWithReceipt(tokenId, GetTxParams(mintPriceBI));
                Debug.Log($"Carte achetée. Tx : {receipt.TransactionHash}");
            } else
            {
                Debug.LogWarning($"La carte {tokenId} n'est pas en vente.");
            }
        } catch (Exception e)
        {
            Debug.LogError("Erreur lors de l'achat : " + e.Message);
        }
    }

    public async void ShowSalePrice(BigInteger tokenId)
    {
        try
        {
            var (isForSale, seller, price) = await contract.GetSaleInfo(tokenId);

            if (isForSale)
                Debug.Log($"Carte {tokenId} en vente pour : {price} wei");
            else
                Debug.Log($"Carte {tokenId} n'est pas en vente.");
        } catch (Exception e)
        {
            Debug.LogError("Erreur en récupérant le prix : " + e.Message);
        }
    }

    public async void ShowTotalSupply()
    {
        try
        {
            BigInteger total = await contract.TotalSupply();
            Debug.Log($"Total de cartes mintées : {total}");
        } catch (Exception e)
        {
            Debug.LogError("Erreur récupération totalSupply : " + e.Message);
        }
    }
    #endregion

    #region Events
    private void OnCardMinted(CardGameNFT.CardMintedEventDTO e)
    {
        Debug.Log($"Carte mintée pour : {e.To} | TokenID : {e.TokenId}");
    }

    private void OnCardListedForSale(CardGameNFT.CardListedForSaleEventDTO e)
    {
        Debug.Log($"Carte {e.TokenId} mise en vente pour {e.Price} wei");
    }

    private void OnCardSaleCancelled(CardGameNFT.CardSaleCancelledEventDTO e)
    {
        Debug.Log($"Vente annulée pour la carte : {e.TokenId}");
    }

    private void OnCardPurchased(CardGameNFT.CardPurchasedEventDTO e)
    {
        Debug.Log($"Carte {e.TokenId} vendue à {e.Buyer} pour {e.Price} wei");
    }
    #endregion

    private TransactionRequest GetTxParams(BigInteger? value = null)
    {
        return new TransactionRequest()
        {
            GasLimit = new HexBigInteger(gasLimit),
            GasPrice = new HexBigInteger(gasPrice),
            Value = value.HasValue ? new HexBigInteger(value.Value) : null
        };
    }
}
