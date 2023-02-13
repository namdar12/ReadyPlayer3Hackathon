# Bounty Bonanza

This is a online game built with Unity and thirdweb Unity SDK. 

## Game Rules
A player signs into the game using a MetaMask wallet and deposits 0.1 ETH into the game's pool. Once the deposit is made, the game begins. 
The player must click on objects that fly onto the screen. There are four distinct objects: a ball, a box, a barrel, and a skull. The ball, box, 
and barrel are worth 5 points each, while the skull is worth -5 points. If a player clicks on the skull three times,they automatically lose the game. 
The objective is to score as many points as possible within a one-minute time frame. The game's prize pool lasts for five days, and whoever has the
highest score at the end of that period can claim the prize.

**Fun Note**:
As the bounty is running, the games funds are gaining yeild through an aave contract. 


## Setup

To run the game locally, download Unity. Git pull this repo. Open Unity and go to -> file -> Build Settings -> Under platform click on WebGL.
Then click on Player settings in the bottom left. Under Project Settings, click player. Under Settings for WebGL, click on Resolution and Presesntation. 
Choose Thirdweb under WebGL Template. Click on Other Settings, and uncheck Auto Graphics API. Close Project Settings, and click on Build and Run. 

## Tech-Stack

We used Unity to build the actual game. We used the thirdweb Unity SDK to create our contract, and connect it to our game. 

## Future Plans

We want to add other wallet connections, so users can decide which wallet they want to connect with. 
Use scenario.gg to generate prefabs and backgrouds.
The winners can get a soul bound NFT with their high score.
Add different levels to the game.
Have users create their own mini bounites that they can shar with their friends. 
Have the background change each time a player plays. 
