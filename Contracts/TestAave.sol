// SPDX-License-Identifier: MIT

pragma solidity >=0.7.0 <0.9.0;

import {IERC20} from "@openzeppelin/contracts/token/ERC20/IERC20.sol";

interface IWETHGateway {
  function depositETH(
    address lendingPool,
    address onBehalfOf,
    uint16 referralCode
  ) external payable;

  function withdrawETH(
    address lendingPool,
    uint256 amount,
    address onBehalfOf
  ) external;
}

//Adresses in Goerli:
//AAVE Lending Pool: 0x7b5C526B7F8dfdff278b4a3e045083FBA4028790
//WETH : 0xB4FBF271143F4FBf7B91A5ded31805e42b2208d6
//aWETH: 0x7649e0d153752c556b8b23DB1f1D3d42993E83a5
//WETH Gateway = 0x2A498323aCaD2971a8b1936fD7540596dC9BBacD;


contract TestAave {
  
  IWETHGateway public wethGateway;
  IERC20 public aToken;
  address aavePool; 
  constructor(address _gatewayAddress, address _aTokenAddress)  {
    wethGateway = IWETHGateway(_gatewayAddress);
    aToken = IERC20(_aTokenAddress);
    aavePool = 0x7b5C526B7F8dfdff278b4a3e045083FBA4028790;
  }

  function supplyETH() public payable {
    require(msg.value == 0.01 ether);
    wethGateway.depositETH{value: 0.01 ether}(aavePool, address(this), 0);
  }

  function withdrawETH() public {
    aToken.approve(address(wethGateway), type(uint).max);
    wethGateway.withdrawETH(aavePool, type(uint).max, msg.sender);
  }
  
}