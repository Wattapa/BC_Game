using ChainSafe.Gaming.Evm.Contracts.Custom;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CardDisplayManager : MonoBehaviour
{
    [Header("Prefab & Layout")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject cardList;
    [SerializeField] private RectTransform contentParent; // Conteneur ScrollView
    [SerializeField] private float cardSpacing = 300f;

    private List<CardUI> displayedCards = new List<CardUI>();

    public void ClearCards()
    {
        foreach (var cardUI in displayedCards)
        {
            if (cardUI != null)
                Destroy(cardUI.gameObject);
        }
        displayedCards.Clear();
    }

    public void DisplayCards(List<CardAttributes> cards)
    {
        if (displayedCards.Count > 0)
        {
            cardList.SetActive(false);

            ClearCards();
        } else
        {
            cardList.SetActive(true);

            contentParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 420 * cards.Count);

            for (int i = 0; i < cards.Count; i++)
            {
                GameObject cardObj = Instantiate(cardPrefab, contentParent);
                RectTransform rt = cardObj.GetComponent<RectTransform>();

                // Position horizontale selon l'index
                rt.anchoredPosition = new Vector2(i * cardSpacing, 0);

                CardUI cardUI = cardObj.GetComponent<CardUI>();
                cardUI.SetCardValue(cards[i]);

                displayedCards.Add(cardUI);
            }
        }
    }
}
