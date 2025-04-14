using System;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Transactions;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using UnityEngine;
using ChainSafe.Gaming.RPC.Events;
using System.Runtime.InteropServices;
using System.Collections.Generic;



namespace ChainSafe.Gaming.Evm.Contracts.Custom
{
    public partial class CardGameNFT : ICustomContract
    {
        public string Address => OriginalContract.Address;
       
        public string ABI => "[   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"to\",         \"type\": \"address\"       },       {         \"internalType\": \"string\",         \"name\": \"tokenURI\",         \"type\": \"string\"       },       {         \"internalType\": \"string\",         \"name\": \"name\",         \"type\": \"string\"       },       {         \"internalType\": \"enum CardGameNFT.Rarity\",         \"name\": \"rarity\",         \"type\": \"uint8\"       },       {         \"internalType\": \"string\",         \"name\": \"description\",         \"type\": \"string\"       }     ],     \"name\": \"mintCardOwner\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"string\",         \"name\": \"tokenURI\",         \"type\": \"string\"       },       {         \"internalType\": \"string\",         \"name\": \"name\",         \"type\": \"string\"       },       {         \"internalType\": \"enum CardGameNFT.Rarity\",         \"name\": \"rarity\",         \"type\": \"uint8\"       },       {         \"internalType\": \"string\",         \"name\": \"description\",         \"type\": \"string\"       }     ],     \"name\": \"mintCard\",     \"outputs\": [],     \"stateMutability\": \"payable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"newPrice\",         \"type\": \"uint256\"       }     ],     \"name\": \"setMintPrice\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [],     \"name\": \"getMintPrice\",     \"outputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"\",         \"type\": \"uint256\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       },       {         \"internalType\": \"uint256\",         \"name\": \"price\",         \"type\": \"uint256\"       }     ],     \"name\": \"putCardForSale\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"cancelSale\",     \"outputs\": [],     \"stateMutability\": \"nonpayable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"buyCard\",     \"outputs\": [],     \"stateMutability\": \"payable\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"getCard\",     \"outputs\": [       {         \"components\": [           {             \"internalType\": \"string\",             \"name\": \"name\",             \"type\": \"string\"           },           {             \"internalType\": \"enum CardGameNFT.Rarity\",             \"name\": \"rarity\",             \"type\": \"uint8\"           },           {             \"internalType\": \"string\",             \"name\": \"description\",             \"type\": \"string\"           },           {             \"internalType\": \"string\",             \"name\": \"tokenURI\",             \"type\": \"string\"           }         ],         \"internalType\": \"struct CardGameNFT.CardAttributes\",         \"name\": \"\",         \"type\": \"tuple\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [],     \"name\": \"totalSupply\",     \"outputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"\",         \"type\": \"uint256\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"address\",         \"name\": \"owner\",         \"type\": \"address\"       }     ],     \"name\": \"getCardsOfDetailed\",     \"outputs\": [       {         \"components\": [           {             \"internalType\": \"string\",             \"name\": \"name\",             \"type\": \"string\"           },           {             \"internalType\": \"enum CardGameNFT.Rarity\",             \"name\": \"rarity\",             \"type\": \"uint8\"           },           {             \"internalType\": \"string\",             \"name\": \"description\",             \"type\": \"string\"           },           {             \"internalType\": \"string\",             \"name\": \"tokenURI\",             \"type\": \"string\"           }         ],         \"internalType\": \"struct CardGameNFT.CardAttributes[]\",         \"name\": \"\",         \"type\": \"tuple[]\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"inputs\": [       {         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"getSaleInfo\",     \"outputs\": [       {         \"internalType\": \"bool\",         \"name\": \"isForSale\",         \"type\": \"bool\"       },       {         \"internalType\": \"address\",         \"name\": \"seller\",         \"type\": \"address\"       },       {         \"internalType\": \"uint256\",         \"name\": \"price\",         \"type\": \"uint256\"       }     ],     \"stateMutability\": \"view\",     \"type\": \"function\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"to\",         \"type\": \"address\"       },       {         \"indexed\": true,         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"CardMinted\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": true,         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       },       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"seller\",         \"type\": \"address\"       },       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"price\",         \"type\": \"uint256\"       }     ],     \"name\": \"CardListedForSale\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": true,         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       }     ],     \"name\": \"CardSaleCancelled\",     \"type\": \"event\"   },   {     \"anonymous\": false,     \"inputs\": [       {         \"indexed\": true,         \"internalType\": \"uint256\",         \"name\": \"tokenId\",         \"type\": \"uint256\"       },       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"buyer\",         \"type\": \"address\"       },       {         \"indexed\": true,         \"internalType\": \"address\",         \"name\": \"seller\",         \"type\": \"address\"       },       {         \"indexed\": false,         \"internalType\": \"uint256\",         \"name\": \"price\",         \"type\": \"uint256\"       }     ],     \"name\": \"CardPurchased\",     \"type\": \"event\"   } ]";
        
        public string ContractAddress { get; set; }
        
        public IEventManager EventManager { get; set; }

        public Contract OriginalContract { get; set; }
                
        public bool Subscribed { get; set; }

        
        #region Methods
        public async Task MintCardOwner(string to, string tokenURI, string name, BigInteger rarity, string description, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("mintCardOwner", new object [] {
                to, tokenURI, name, rarity, description
            }, transactionOverwrite);  
        }

        public async Task<TransactionReceipt> MintCardOwnerWithReceipt(string to, string tokenURI, string name, BigInteger rarity, string description, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("mintCardOwner", new object [] {
                to, tokenURI, name, rarity, description
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task MintCard(string tokenURI, string name, BigInteger rarity, string description, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("mintCard", new object [] {
                tokenURI, name, rarity, description
            }, transactionOverwrite);
        }

        public async Task<TransactionReceipt> MintCardWithReceipt(string tokenURI, string name, BigInteger rarity, string description, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("mintCard", new object [] {
                tokenURI, name, rarity, description
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task SetMintPrice(BigInteger newPrice, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("setMintPrice", new object [] {
                newPrice
            }, transactionOverwrite);
        }

        public async Task<TransactionReceipt> SetMintPriceWithReceipt(BigInteger newPrice, TransactionRequest transactionOverwrite=null) 
        {
            try
            {
                Debug.Log("I'm gonna change the Mint Price!");
                var response = await OriginalContract.SendWithReceipt("setMintPrice", new object[] { newPrice }, transactionOverwrite);
                return response.receipt;
            } catch (Exception ex)
            {
                Debug.LogError("Erreur dans SetMintPriceWithReceipt: " + ex.Message);
                Debug.LogError("StackTrace: " + ex.StackTrace);
                return null;
            }
        }

        public async Task<BigInteger> GetMintPrice( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<BigInteger>("getMintPrice", new object [] {
                
            }, transactionOverwrite);
            
            return response;
        }


        public async Task PutCardForSale(BigInteger tokenId, BigInteger price, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("putCardForSale", new object [] {
                tokenId, price
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> PutCardForSaleWithReceipt(BigInteger tokenId, BigInteger price, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("putCardForSale", new object [] {
                tokenId, price
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task CancelSale(BigInteger tokenId, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("cancelSale", new object [] {
                tokenId
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> CancelSaleWithReceipt(BigInteger tokenId, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("cancelSale", new object [] {
                tokenId
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task BuyCard(BigInteger tokenId, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Send("buyCard", new object [] {
                tokenId
            }, transactionOverwrite);
            
            
        }
        public async Task<TransactionReceipt> BuyCardWithReceipt(BigInteger tokenId, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.SendWithReceipt("buyCard", new object [] {
                tokenId
            }, transactionOverwrite);
            
            return response.receipt;
        }

        public async Task<CardAttributes> GetCard(BigInteger tokenId, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<CardAttributes>("getCard", new object [] {
                tokenId
            }, transactionOverwrite);
            
            return response;
        }


        public async Task<BigInteger> TotalSupply( TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<BigInteger>("totalSupply", new object [] {
                
            }, transactionOverwrite);
            
            return response;
        }

        public async Task<List<CardAttributes>> GetCardsOfDetailed(string owner, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call<List<CardAttributes>>("getCardsOfDetailed", new object [] {
                owner
            }, transactionOverwrite);

            return response;
        }

        public async Task<(bool isForSale, string seller, BigInteger price)> GetSaleInfo(BigInteger tokenId, TransactionRequest transactionOverwrite=null) 
        {
            var response = await OriginalContract.Call("getSaleInfo", new object [] {
                tokenId
            }, transactionOverwrite);
            
            return ((bool)response[0], (string)response[1], (BigInteger)response[2]);
        }
        #endregion
        
        
        #region Event Classes
        public partial class CardMintedEventDTO : CardMintedEventDTOBase { }
        
        [Event("CardMinted")]
        public class CardMintedEventDTOBase : IEventDTO
        {
            [Parameter("address", "to", 0, true)]
            public virtual string To { get; set; }

            [Parameter("uint256", "tokenId", 1, true)]
            public virtual BigInteger TokenId { get; set; }
        }
    
        public event Action<CardMintedEventDTO> OnCardMinted;
        
        private void CardMinted(CardMintedEventDTO cardMinted)
        {
            OnCardMinted?.Invoke(cardMinted);
        }

        public partial class CardListedForSaleEventDTO : CardListedForSaleEventDTOBase { }
        
        [Event("CardListedForSale")]
        public class CardListedForSaleEventDTOBase : IEventDTO
        {
            [Parameter("uint256", "tokenId", 0, true)]
            public virtual BigInteger TokenId { get; set; }

            [Parameter("address", "seller", 1, true)]
            public virtual string Seller { get; set; }

            [Parameter("uint256", "price", 2, false)]
            public virtual BigInteger Price { get; set; }
        }
    
        public event Action<CardListedForSaleEventDTO> OnCardListedForSale;
        
        private void CardListedForSale(CardListedForSaleEventDTO cardListedForSale)
        {
            OnCardListedForSale?.Invoke(cardListedForSale);
        }

        public partial class CardSaleCancelledEventDTO : CardSaleCancelledEventDTOBase { }
        
        [Event("CardSaleCancelled")]
        public class CardSaleCancelledEventDTOBase : IEventDTO
        {
            [Parameter("uint256", "tokenId", 0, true)]
            public virtual BigInteger TokenId { get; set; }
        }
    
        public event Action<CardSaleCancelledEventDTO> OnCardSaleCancelled;
        
        private void CardSaleCancelled(CardSaleCancelledEventDTO cardSaleCancelled)
        {
            OnCardSaleCancelled?.Invoke(cardSaleCancelled);
        }

        public partial class CardPurchasedEventDTO : CardPurchasedEventDTOBase { }
        
        [Event("CardPurchased")]
        public class CardPurchasedEventDTOBase : IEventDTO
        {
            [Parameter("uint256", "tokenId", 0, true)]
            public virtual BigInteger TokenId { get; set; }

            [Parameter("address", "buyer", 1, true)]
            public virtual string Buyer { get; set; }

            [Parameter("address", "seller", 2, true)]
            public virtual string Seller { get; set; }

            [Parameter("uint256", "price", 3, false)]
            public virtual BigInteger Price { get; set; }
        }
    
        public event Action<CardPurchasedEventDTO> OnCardPurchased;
        
        private void CardPurchased(CardPurchasedEventDTO cardPurchased)
        {
            OnCardPurchased?.Invoke(cardPurchased);
        }
        #endregion
        
        #region Interface Implemented Methods
        
        public async ValueTask DisposeAsync()
        {
            
            if(!Subscribed)
                return;
                
           
            Subscribed = false;
            try
            {
                if(EventManager == null)
                    return;

			await EventManager.Unsubscribe<CardMintedEventDTO>(CardMinted, ContractAddress);
			OnCardMinted = null;
			await EventManager.Unsubscribe<CardListedForSaleEventDTO>(CardListedForSale, ContractAddress);
			OnCardListedForSale = null;
			await EventManager.Unsubscribe<CardSaleCancelledEventDTO>(CardSaleCancelled, ContractAddress);
			OnCardSaleCancelled = null;
			await EventManager.Unsubscribe<CardPurchasedEventDTO>(CardPurchased, ContractAddress);
			OnCardPurchased = null;

            
            
            }catch(Exception e)
            {
                Debug.LogError("Caught an exception whilst unsubscribing from events\n" + e.Message);
            }
        }
        
        public async ValueTask InitAsync()
        {
            if(Subscribed)
                return;
            Subscribed = true;

            try
            {
                if(EventManager == null)
                    return;

                await EventManager.Subscribe<CardMintedEventDTO>(CardMinted, ContractAddress);
                await EventManager.Subscribe<CardListedForSaleEventDTO>(CardListedForSale, ContractAddress);
                await EventManager.Subscribe<CardSaleCancelledEventDTO>(CardSaleCancelled, ContractAddress);
                await EventManager.Subscribe<CardPurchasedEventDTO>(CardPurchased, ContractAddress);
    
            }catch(Exception e)
            {
                Debug.LogError("Caught an exception whilst subscribing to events. Subscribing to events will not work in this session\n" + e.Message);
            }
            
        }
        
        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public IContract Attach(string address)
        {
            return OriginalContract.Attach(address);
        }
        
        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public Task<object[]> Call(string method, object[] parameters = null, TransactionRequest overwrite = null)
        {
            return OriginalContract.Call(method, parameters, overwrite);
        }
        
        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public object[] Decode(string method, string output)
        {
            return OriginalContract.Decode(method, output);
        }
        
        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public Task<object[]> Send(string method, object[] parameters = null, TransactionRequest overwrite = null)
        {
            return OriginalContract.Send(method, parameters, overwrite);
        }
        
        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public Task<(object[] response, TransactionReceipt receipt)> SendWithReceipt(string method, object[] parameters = null, TransactionRequest overwrite = null)
        {
            return OriginalContract.SendWithReceipt(method, parameters, overwrite);
        }
        
        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public Task<HexBigInteger> EstimateGas(string method, object[] parameters, TransactionRequest overwrite = null)
        {
            return OriginalContract.EstimateGas(method, parameters, overwrite);
        }
        
        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public string Calldata(string method, object[] parameters = null)
        {
            return OriginalContract.Calldata(method, parameters);
        }
        
        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public Task<TransactionRequest> PrepareTransactionRequest(string method, object[] parameters, bool isReadCall = false, TransactionRequest overwrite = null)
        {
            return OriginalContract.PrepareTransactionRequest(method, parameters, isReadCall, overwrite);
        }
        #endregion
    }

    [FunctionOutput]
    public class CardAttributes : IFunctionOutputDTO
    {
        [Parameter("uint256", "tokenId", 1)]
        public BigInteger TokenID { get; set; }

        [Parameter("string", "name", 2)]
        public string Name { get; set; }

        [Parameter("uint8", "rarity", 3)]
        public BigInteger Rarity { get; set; }

        [Parameter("string", "description", 4)]
        public string Description { get; set; }

        [Parameter("string", "tokenURI", 5)]
        public string TokenURI { get; set; }
    }
}
