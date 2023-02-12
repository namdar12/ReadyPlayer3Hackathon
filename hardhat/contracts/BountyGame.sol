// SPDX-License-Identifier: MIT

pragma solidity >=0.7.0 <0.9.0;

/**
 * @title BountyGame
 */
contract BountyGame {

    address public winner;
    uint256 public deadline = block.timestamp + 1 minutes;  

    mapping(address => bool) public isPlayer;
    mapping(address => uint256) public scores;

    function registerPlayer() public payable {
        require(msg.value == 0.1 ether);
        isPlayer[msg.sender] = true;
    }

    function updateScore(uint256 _score) public {
        require(isPlayer[msg.sender] == true);
        scores[msg.sender] = _score;
        if (_score > scores[winner] ){
            winner = msg.sender;
        }
    }

    function claimPrize () public {
        require(msg.sender == winner);
        require(block.timestamp >= deadline);
        uint prize = address(this).balance;
        (bool success, ) = winner.call{value: prize}("");
        require(success, "Failed to send prize");
    }

    function hello () public pure returns (string memory){
        return "hello from goerli :)";
    }
}