// SPDX-License-Identifier: MIT
pragma solidity ^0.8.20;

import "@openzeppelin/contracts/token/ERC721/extensions/ERC721URIStorage.sol";
import "@openzeppelin/contracts/access/Ownable.sol";

contract CardGameNFT2 is ERC721URIStorage, Ownable(msg.sender) {
    uint256 private _nextTokenId;
    uint256 public mintPrice;

    enum Rarity { Common, Rare, Epic, Legendary }

    struct CardAttributes {
        uint256 tokenId;
        string name;
        Rarity rarity;
        string description;
        string tokenURI;
    }

    struct Sale {
        address seller;
        uint256 price; // in wei
    }

    mapping(uint256 => CardAttributes) public cardAttributes;
    mapping(uint256 => Sale) public cardsForSale;

    event CardMinted(address indexed to, uint256 indexed tokenId);
    event CardListedForSale(uint256 indexed tokenId, address indexed seller, uint256 price);
    event CardSaleCancelled(uint256 indexed tokenId);
    event CardPurchased(uint256 indexed tokenId, address indexed buyer, address indexed seller, uint256 price);

    constructor() ERC721("CardGameNFT", "CGNFT") {
         mintPrice = 0.01 ether;
    }

    // MINT
    function mintCardOwner(
        address to,
        string memory tokenURI,
        string memory name,
        Rarity rarity,
        string memory description
    ) external onlyOwner {
        uint256 tokenId = _nextTokenId++;
        _safeMint(to, tokenId);
        _setTokenURI(tokenId, tokenURI);

        cardAttributes[tokenId] = CardAttributes(tokenId, name, rarity, description, tokenURI);
        emit CardMinted(to, tokenId);
    }

    // Mint public avec paiement
    function mintCard(
        string memory tokenURI,
        string memory name,
        Rarity rarity,
        string memory description
    ) external payable {
        require(msg.value >= mintPrice, "Montant insuffisant pour mint");

        uint256 tokenId = _nextTokenId++;
        _safeMint(msg.sender, tokenId);
        _setTokenURI(tokenId, tokenURI);

        cardAttributes[tokenId] = CardAttributes(tokenId, name, rarity, description, tokenURI);
        emit CardMinted(msg.sender, tokenId);
    }

    // Owner peut changer le prix
    function setMintPrice(uint256 newPrice) external onlyOwner {
        mintPrice = newPrice;
    }

    // Fonction pour récupérer le prix actuel de mint
    function getMintPrice() external view returns (uint256) {
        return mintPrice;
    }

    // VENTE
    function putCardForSale(uint256 tokenId, uint256 price) external {
        require(ownerOf(tokenId) == msg.sender, "Vous n etes pas le proprietaire");
        require(price > 0, "Prix invalide");

        cardsForSale[tokenId] = Sale({
            seller: msg.sender,
            price: price
        });

        emit CardListedForSale(tokenId, msg.sender, price);
    }

    function cancelSale(uint256 tokenId) external {
        require(cardsForSale[tokenId].seller == msg.sender, "Vous n'avez pas mis cette carte en vente");
        delete cardsForSale[tokenId];
        emit CardSaleCancelled(tokenId);
    }

    function buyCard(uint256 tokenId) external payable {
        Sale memory sale = cardsForSale[tokenId];
        require(sale.price > 0, "Cette carte n'est pas en vente");
        require(msg.value >= sale.price, "Montant insuffisant");

        address seller = sale.seller;
        delete cardsForSale[tokenId];

        // Transfert ETH au vendeur
        payable(seller).transfer(sale.price);

        // Transfert du NFT à l'acheteur
        _transfer(seller, msg.sender, tokenId);

        emit CardPurchased(tokenId, msg.sender, seller, sale.price);
    }

    // VUE
function getCard(uint256 tokenId) external view returns (CardAttributes memory) {
    require(_ownerOf(tokenId) != address(0), "Card does not exist");

    CardAttributes memory attrs = cardAttributes[tokenId];

    return CardAttributes({
        tokenId: attrs.tokenId,
        name: attrs.name,
        rarity: attrs.rarity,
        description: attrs.description,
        tokenURI: tokenURI(tokenId)
    });
}


    function totalSupply() public view returns (uint256) {
        return _nextTokenId;
    }

    function getCardsOfDetailed(address owner) external view returns (CardAttributes[] memory) {
        uint256 count;
        for (uint256 i = 0; i < _nextTokenId; i++) {
            if (_ownerOf(i) == owner) {
                count++;
            }
        }

        CardAttributes[] memory result = new CardAttributes[](count);
        uint256 index;

        for (uint256 i = 0; i < _nextTokenId; i++) {
            if (_ownerOf(i) == owner) {
                CardAttributes memory attrs = cardAttributes[i];
                string memory uri = tokenURI(i);

                result[index] = CardAttributes({
                    tokenId: attrs.tokenId,
                    name: attrs.name,
                    rarity: attrs.rarity,
                    description: attrs.description,
                    tokenURI: uri
                });

                index++;
            }
        }

        return result;
    }


    function getSaleInfo(uint256 tokenId) external view returns (bool isForSale, address seller, uint256 price) {
        Sale memory sale = cardsForSale[tokenId];
        isForSale = sale.price > 0;
        seller = sale.seller;
        price = sale.price;
    }
}
