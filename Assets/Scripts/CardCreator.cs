using ChainSafe.Gaming.Evm.Contracts.Custom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCreator : MonoBehaviour
{
    [SerializeField] private string[] availableURIs;

    private string[] cardNames = { "Dragon", "Knight", "Wizard", "Elf", "Goblin", "Phoenix", "Valkyrie", "Samurai", "Assassin", "Golem" };
    private string[] descriptions = {
        "Une carte ancienne pleine de mystère.",
        "Un héros légendaire prêt pour la bataille.",
        "Une créature magique d’un autre monde.",
        "Puissante, mais rare.",
        "Issue des terres oubliées.",
        "Forgée dans le feu des dieux.",
        "Gardienne des secrets anciens.",
        "Rapide comme l’éclair.",
        "Discrète et mortelle.",
        "Colosse de pierre indestructible."
    };

    public static CardCreator Instance;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    public CardAttributes CreateRandomCard()
    {
        CardAttributes card = new CardAttributes
        {
            TokenURI = availableURIs[Random.Range(0, availableURIs.Length)],
            Name = cardNames[Random.Range(0, cardNames.Length)],
            Description = descriptions[Random.Range(0, descriptions.Length)],
            Rarity = Random.Range(0, 4) // 0 = Common, 1 = Rare, 2 = Epic, 3 = Legendary
        };

        Debug.Log($"Génération aléatoire d'une carte : {card.Name}, Rarity {card.Rarity}, Description : {card.Description}");

        return card;
    }
}
