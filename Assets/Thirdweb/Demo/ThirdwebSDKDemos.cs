using UnityEngine;
using Thirdweb;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;



public class ThirdwebSDKDemos : MonoBehaviour
{
    private ThirdwebSDK sdk;
    private int count;
    public Text walletInfotext;
    public GameObject connectButtonsContainer;
    public GameObject walletInfoContainer;

    public GameObject playButton;

    public Text resultText;
    public Text bountyText;
    public Text winnerText;
    public Text scoreText;
    public Text dateText;


    private string deployedAt = "0xfFcbD5905570Fa810cDc63bb891678B7Ce0c0744";
    
    private string ABI = "[{\"inputs\": [],\"name\": \"claimPrize\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"registerPlayer\",\"outputs\": [],\"stateMutability\": \"payable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"uint256\",\"name\": \"_score\",\"type\": \"uint256\"}],\"name\": \"updateScore\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\":\"function\"},{\"inputs\": [],\"name\": \"deadline\",\"outputs\": [{\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"hello\",\"outputs\": [{\"internalType\": \"string\",\"name\": \"\",\"type\": \"string\"}],\"stateMutability\": \"pure\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"\",\"type\": \"address\"}],\"name\": \"isPlayer\",\"outputs\": [{\"internalType\": \"bool\",\"name\": \"\",\"type\": \"bool\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"\",\"type\": \"address\"}],\"name\": \"scores\",\"outputs\": [{\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"winner\",\"outputs\": [{\"internalType\": \"address\",\"name\": \"\",\"type\": \"address\"}],\"stateMutability\": \"view\",\"type\": \"function\"}]";


    void Start()
    {
        sdk = new ThirdwebSDK("goerli");
        InitializeState();
    }

    private void InitializeState()
    {
        connectButtonsContainer.SetActive(true);
        walletInfoContainer.SetActive(false);
        playButton.SetActive(false);
    }

    void Update()
    {
    }

    public void MetamaskLogin()
    {
        ConnectWallet(WalletProvider.MetaMask);
    }

    public void CoinbaseWalletLogin()
    {
        ConnectWallet(WalletProvider.CoinbaseWallet);
    }

    public void WalletConnectLogin()
    {
        ConnectWallet(WalletProvider.WalletConnect);
    }

    public void MagicAuthLogin()
    {
        // Requires passing a magic.link API key in the SDK options:
        // sdk = new ThirdwebSDK("goerli", new ThirdwebSDK.Options()
        // {
        //     wallet = new ThirdwebSDK.WalletOptions()
        //     {
        //         appName = "Thirdweb SDK Demo",
        //         extras = new Dictionary<string, object>()
        //         {
        //             {"apiKey", "your_api_key"}
        //         }
        //     }
        // });
        ConnectWallet(WalletProvider.MagicAuth);
    }

    public async void DisconnectWallet()
    {
        await sdk.wallet.Disconnect();
        connectButtonsContainer.SetActive(true);
        walletInfoContainer.SetActive(false);

        bountyText.text = "Bounty: ...";
        winnerText.text = "Winner: ...";
        scoreText.text = "Max score: ...";
        dateText.text = "Due date: ...";

        playButton.SetActive(false);


    }

    private async void ConnectWallet(WalletProvider provider)
    {
        connectButtonsContainer.SetActive(false);
        walletInfoContainer.SetActive(true);
        walletInfotext.text = "Connecting...";
        try
        {
            string address = await sdk.wallet.Connect(new WalletConnection()
            {
                provider = provider,
                chainId = 5 // Switch the wallet Goerli on connection
            });
            walletInfotext.text = "Connected as: " + address;

            FetchData();
            playButton.SetActive(true);

        }
        catch (System.Exception e)
        {
            walletInfotext.text = "Error (see console): " + e.Message;
        }
    }

    public async void OnBalanceClick()
    {
        resultText.text = "Loading...";
        CurrencyValue balance = await sdk.wallet.GetBalance();
        resultText.text = "Balance: " + balance.displayValue.Substring(0, 5) + " " + balance.symbol;
    }

    public async void OnSignClick()
    {
        resultText.text = "Signing...";
        try
        {
            var data = await sdk.wallet.Authenticate("example.com");
            resultText.text = "Sig: " + data.payload.address.Substring(0, 6) + "...";
        }
        catch (System.Exception e)
        {
            resultText.text = "Auth Error: " + e.Message;
        }
    }
    
    public async void GetTest(){
        var contract = sdk.GetContract(deployedAt, ABI); 
        var test = await contract.Read<string>("hello");
        resultText.text = test;
    }

    public async void FetchData(){
        GetDate();
        GetWinnerAndScore();
        GetContractBalance();
    }

    public async void GetWinnerAndScore(){
        winnerText.text = "Loading...";
        scoreText.text = "Loading...";

        var contract = sdk.GetContract(deployedAt, ABI); 
        var winner = await contract.Read<string>("winner");
        winnerText.text = "Winner: " + winner;

        var score = await contract.Read<string>("scores", winner);
        scoreText.text = "Max score: "+score;
    }

    public async void GetDate(){

        dateText.text = "Loading...";

        var contract = sdk.GetContract(deployedAt, ABI); 
        var date = await contract.Read<string>("deadline");
        double doubleDate = Convert.ToDouble(date);

        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    	dateTime = dateTime.AddSeconds( doubleDate ).ToLocalTime();

        dateText.text = "Due date: " + dateTime + " UTC";
    }


    public async void RegisterPlayer(){
        var contract = sdk.GetContract(deployedAt, ABI); 
        //custom write with transaction overrides
        var result = await contract.Write("registerPlayer", new TransactionRequest
        {
             value = "0.1".ToWei() // 0.1 ETH
        });
        if (result.isSuccessful())
        {
        SceneManager.LoadScene(1);
        resultText.text = "You are playing!";

        }
        else
        {
        resultText.text = "ERROR: NOT PLAYING!";
        }
    }

    public async void UpdateScore(string score){
        var contract = sdk.GetContract(deployedAt, ABI); 
        var result = await contract.Write("updateScore",score);
        
        if (result.isSuccessful())
        {
        resultText.text = "Update Succesfull";
        }
        else
        {
        resultText.text = "ERROR: NOT UPDATED";
        }
    }

    public async void ClaimPrize(){
        var contract = sdk.GetContract(deployedAt, ABI); 
        var result = await contract.Write("claimPrize");
        
        if (result.isSuccessful())
        {
        resultText.text = "Prize claimed, congrats!";
        }
        else
        {
        resultText.text = "ERROR: NOT CLAIMED";
        }
    }

    public async void GetContractBalance()
    {
        bountyText.text = "Loading...";
        var contract = sdk.GetContract(deployedAt, ABI); 

        try
        {
            CurrencyValue balance = await contract.GetBalance();
            bountyText.text = "Bounty: " + balance.displayValue + " ETH";
        }
        catch (System.Exception e)
        {
            bountyText.text = "Error calling contract (see console): " + e.Message;
        } 
    }

    public async void CustomContract()
    {
        var contract = sdk.GetContract("0x62Cf5485B6C24b707E47C5E0FB2EAe7EbE18EC4c");
        try
        {
            // custom read
            resultText.text = "Fetching contract data...";
            var result = await contract.Read<string>("uri", 0);
            resultText.text = "Read custom token uri: " + result;
            // custom write
            await contract.Write("claimKitten");
            // custom write with transaction overrides
            // await contract.Write("claim", new TransactionRequest
            // {
            //     value = "0.05".ToWei() // 0.05 ETH
            // }, "0xE79ee09bD47F4F5381dbbACaCff2040f2FbC5803", 0, 1);
            resultText.text = "Custom contraact call successful";
        }
        catch (System.Exception e)
        {
            resultText.text = "Error calling contract (see console): " + e.Message;
        }
    }

    public async void GetERC721()
    {
        // fetch single NFT
        var contract = sdk.GetContract("0x2e01763fA0e15e07294D74B63cE4b526B321E389"); // NFT Drop
        count++;
        resultText.text = "Fetching Token: " + count;
        NFT result = await contract.ERC721.Get(count.ToString());
        resultText.text = result.metadata.name + "\nowned by " + result.owner.Substring(0, 6) + "...";

        // fetch all NFTs
        // resultText.text = "Fetching all NFTs";
        // List<NFT> result = await contract.ERC721.GetAll(new Thirdweb.QueryAllParams() {
        //     start = 0,
        //     count = 10,
        // });
        // resultText.text = "Fetched " + result.Count + " NFTs";
        // for (int i = 0; i < result.Count; i++) {
        //     Debug.Log(result[i].metadata.name + " owned by " + result[i].owner);
        // }

        // custom function call
        // string uri = await contract.Read<string>("tokenURI", count);
        // fetchButton.text = uri;
    }

    public async void GetERC1155()
    {
        var contract = sdk.GetContract("0x86B7df0dc0A790789D8fDE4C604EF8187FF8AD2A");

        // Edition Drop
        // Fetch single NFT
        // count++;
        // resultText.text = "Fetching Token: " + count;
        // NFT result = await contract.ERC1155.Get(count.ToString());
        // resultText.text = result.metadata.name + " (x" + result.supply + ")";

        // fetch all NFTs
        resultText.text = "Fetching all NFTs";
        List<NFT> result = await contract.ERC1155.GetAll();
        resultText.text = "Fetched " + result.Count + " NFTs";

    }

    public async void GetERC20()
    {
        var contract = sdk.GetContract("0xB4870B21f80223696b68798a755478C86ce349bE"); // Token
        resultText.text = "Fetching Token info";
        Currency result = await contract.ERC20.Get();
        CurrencyValue currencyValue = await contract.ERC20.TotalSupply();
        resultText.text = result.name + " (" + currencyValue.displayValue + ")";
    }

    public async void MintERC721()
    {
        resultText.text = "SigMinting... (needs minter role to generate signature)";
        // claim
        // var contract = sdk.GetContract("0x2e01763fA0e15e07294D74B63cE4b526B321E389"); // NFT Drop
        // resultText.text = "claiming...";
        // var result = await contract.ERC721.Claim(1);
        // Debug.Log("result id: " + result[0].id);
        // Debug.Log("result receipt: " + result[0].receipt.transactionHash);
        // resultText.text = "claimed tokenId: " + result[0].id;

        // sig mint
        var contract = sdk.GetContract("0x8bFD00BD1D3A2778BDA12AFddE5E65Cca95082DF"); // NFT Collection
        var meta = new NFTMetadata()
        {
            name = "Unity NFT",
            description = "Minted From Unity (signature)",
            image = "ipfs://QmbpciV7R5SSPb6aT9kEBAxoYoXBUsStJkMpxzymV4ZcVc"
        };
        string connectedAddress = await sdk.wallet.GetAddress();
        var payload = new ERC721MintPayload(connectedAddress, meta);
        try
        {
            var p = await contract.ERC721.signature.Generate(payload); // typically generated on the backend
            var result = await contract.ERC721.signature.Mint(p);
            resultText.text = "SigMinted tokenId: " + result.id;
        }
        catch (System.Exception e)
        {
            resultText.text = "Sigmint Failed (see console): " + e.Message;
        }
    }

    public async void MintERC1155()
    {
        Debug.Log("Claim button clicked");
        resultText.text = "Claiming...";

        // claim
        var contract = sdk.GetContract("0x86B7df0dc0A790789D8fDE4C604EF8187FF8AD2A"); // Edition Drop
        var canClaim = await contract.ERC1155.claimConditions.CanClaim("0", 1);
        if (canClaim)
        {
            try
            {
                var result = await contract.ERC1155.Claim("0", 1);
                var newSupply = await contract.ERC1155.TotalSupply("0");
                resultText.text = "Claim successful! New supply: " + newSupply;
            }
            catch (System.Exception e)
            {
                resultText.text = "Claim Failed: " + e.Message;
            }
        }
        else
        {
            resultText.text = "Can't claim";
        }

        // sig mint additional supply
        // var contract = sdk.GetContract("0xdb9AAb1cB8336CCd50aF8aFd7d75769CD19E5FEc"); // Edition
        // var payload = new ERC1155MintAdditionalPayload("0xE79ee09bD47F4F5381dbbACaCff2040f2FbC5803", "1");
        // payload.quantity = 3;
        // var p = await contract.ERC1155.signature.GenerateFromTokenId(payload);
        // var result = await contract.ERC1155.signature.Mint(p);
        // resultText.text = "sigminted tokenId: " + result.id;
    }

    public async void MintERC20()
    {
        resultText.text = "Minting... (needs minter role)";

        // Mint
        var contract = sdk.GetContract("0xB4870B21f80223696b68798a755478C86ce349bE"); // Token
        try
        {
            var result = await contract.ERC20.Mint("1.2");
            resultText.text = "mint successful";
        }
        catch (System.Exception e)
        {
            resultText.text = "Mint failed (see console): " + e.Message;
        }

        // sig mint
        // var contract = sdk.GetContract("0xB4870B21f80223696b68798a755478C86ce349bE"); // Token
        // var payload = new ERC20MintPayload("0xE79ee09bD47F4F5381dbbACaCff2040f2FbC5803", "3.2");
        // var p = await contract.ERC20.signature.Generate(payload);
        // await contract.ERC20.signature.Mint(p);
        // resultText.text = "sigminted currency successfully";
    }

    public async void GetListing()
    {
        resultText.text = "Fetching listing...";

        // fetch listings
        var marketplace = sdk.GetContract("0xC7DBaD01B18403c041132C5e8c7e9a6542C4291A").marketplace; // Marketplace
        var result = await marketplace.GetAllListings();
        resultText.text = "Listing count: " + result.Count + " | " + result[0].asset.name + "(" + result[0].buyoutCurrencyValuePerToken.displayValue + ")";
    }

    public async void BuyListing()
    {
        resultText.text = "Buying...";

        // buy listing
        var marketplace = sdk.GetContract("0xC7DBaD01B18403c041132C5e8c7e9a6542C4291A").marketplace; // Marketplace
        try
        {
            var result = await marketplace.BuyListing("0", 1);
            resultText.text = "NFT bought successfully";
        }
        catch (System.Exception e)
        {
            resultText.text = "Error Buying listing (see console): " + e.Message;
        }
    }

    public async void Deploy()
    {
        resultText.text = "Deploying...";

        // deploy nft collection contract
        try
        {
            var address = await sdk.deployer.DeployNFTCollection(new NFTContractDeployMetadata
            {
                name = "Unity Collection",
                primary_sale_recipient = await sdk.wallet.GetAddress(),
            });
            resultText.text = "Deployed: " + address;
        }
        catch (System.Exception e)
        {
            resultText.text = "Deploy Failed (see console): " + e.Message;
        }
    }

    
}
