using ChainSafe.Gaming.Evm.Contracts.Custom;
using System.Numerics;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [SerializeField] private Image illustration;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI rarity;
    [SerializeField] private Button sellButton; // Le bouton pour vendre la carte

    [SerializeField] private TMP_InputField priceInput;  // Champ de saisie du prix
    [SerializeField] private Button confirmButton;  // Bouton pour confirmer la vente
    [SerializeField] private GameObject content;

    private CardAttributes card;  // Référence de la carte affichée

    private void Start()
    {
        sellButton.onClick.AddListener(OnSellCardClicked);
        confirmButton.onClick.AddListener(OnConfirmSell);
    }

    public async void SetCardValue(CardAttributes _card)
    {
        card = _card;
        title.text = card.Name;
        description.text = card.Description;
        rarity.text = card.Rarity.ToString();

        if (!string.IsNullOrEmpty(card.TokenURI))
        {
            Sprite sprite = await LoadImageFromURL(card.TokenURI);
            if (sprite != null)
            {
                illustration.sprite = sprite;
            }
        }
    }

    private void OnSellCardClicked()
    {
        content.SetActive(true);
    }

    private async Task<Sprite> LoadImageFromURL(string url)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            var asyncOp = uwr.SendWebRequest();

            while (!asyncOp.isDone)
                await Task.Yield();

#if UNITY_2020_1_OR_NEWER
            if (uwr.result != UnityWebRequest.Result.Success)
#else
        if (uwr.isNetworkError || uwr.isHttpError)
#endif
            {
                Debug.LogError("Erreur de chargement d'image : " + uwr.error);
                return null;
            }

            Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), UnityEngine.Vector2.one * 0.5f);
        }
    }

    private void OnConfirmSell()
    {
        if (decimal.TryParse(priceInput.text, out decimal price) && price > 0)
        {
            // Appeler la fonction pour mettre la carte en vente avec le prix
            CardManager.Instance.SellCard(card.TokenID, (BigInteger)price);
            HideSellCardUI();
        } else
        {
            Debug.LogError("Prix invalide !");
        }
    }

    private void HideSellCardUI()
    {
        // Cacher l'interface après la confirmation
        content.SetActive(false);
    }
}
