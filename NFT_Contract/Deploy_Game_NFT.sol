// SPDX-License-Identifier: MIT
pragma solidity ^0.8.20;

import "forge-std/Script.sol";
import "../src/Game_NFT_Sepolia.s.sol";

contract DeployNFT is Script {
    function run() external {
        vm.startBroadcast();
        CardGameNFT2 nft = new CardGameNFT2();
        console.log("NFT Contract deployed at:", address(nft));
        vm.stopBroadcast();
    }
}
