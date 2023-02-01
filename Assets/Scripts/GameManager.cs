using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Thirdweb;



public class GameManager : MonoBehaviour
{
    private ThirdwebSDK sdk;


    public List<GameObject> targets;
    private float SpawnRate = 1;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public int score = 0;
    public int pointValue;
    private ThirdwebSDKDemos thirdwebSDKDemos;

    private string deployedAt = "0xfFcbD5905570Fa810cDc63bb891678B7Ce0c0744";
    
    private string ABI = "[{\"inputs\": [],\"name\": \"claimPrize\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"registerPlayer\",\"outputs\": [],\"stateMutability\": \"payable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"uint256\",\"name\": \"_score\",\"type\": \"uint256\"}],\"name\": \"updateScore\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\":\"function\"},{\"inputs\": [],\"name\": \"deadline\",\"outputs\": [{\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"hello\",\"outputs\": [{\"internalType\": \"string\",\"name\": \"\",\"type\": \"string\"}],\"stateMutability\": \"pure\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"\",\"type\": \"address\"}],\"name\": \"isPlayer\",\"outputs\": [{\"internalType\": \"bool\",\"name\": \"\",\"type\": \"bool\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"\",\"type\": \"address\"}],\"name\": \"scores\",\"outputs\": [{\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"winner\",\"outputs\": [{\"internalType\": \"address\",\"name\": \"\",\"type\": \"address\"}],\"stateMutability\": \"view\",\"type\": \"function\"}]";
    





    // Start is called before the first frame update
    void Start()
    {
        sdk = new ThirdwebSDK("goerli");

        StartCoroutine(SpawnTarget());
        UpdateScore(score);
        thirdwebSDKDemos = GameObject.Find("Thirdweb").GetComponent<ThirdwebSDKDemos>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnTarget(){
        while(true){
            yield return new WaitForSeconds(SpawnRate);
            int index = Random.Range(0,targets.Count);
            Instantiate(targets[index]);
            //UpdateScore(pointValue);
        }
    }

    public void UpdateScore(int scoreToAdd){
        score += scoreToAdd;
        scoreText.text = "Score: " + score;

    }

    public async void UpdateScore(){
        
        var contract = sdk.GetContract(deployedAt, ABI); 
        var result = await contract.Write("updateScore","60");
        
        if (result.isSuccessful())
        {
        
        }
        else
        {
        
        }
    }

  
    public void MetamaskLogin()
    {
        ConnectWallet(WalletProvider.MetaMask);
    }

    private async void ConnectWallet(WalletProvider provider)
    {
        try
        {
            string address = await sdk.wallet.Connect(new WalletConnection()
            {
                provider = provider,
                chainId = 5 // Switch the wallet Goerli on connection
            });
            

        }
        catch (System.Exception e)
        {
            
        }
    }

    public void GameOver(){
        gameOverText.gameObject.SetActive(true);
        //thirdwebSDKDemos.MetamaskLogin();
        
        //thirdwebSDKDemos.UpdateScore();
        MetamaskLogin();
        UpdateScore();

    }







}
