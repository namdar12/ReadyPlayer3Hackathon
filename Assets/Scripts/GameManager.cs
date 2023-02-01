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

    public void GameOver(){
        gameOverText.gameObject.SetActive(true);
        thirdwebSDKDemos.UpdateScore(score.ToString());


    }
  







}
