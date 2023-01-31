using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameManager : MonoBehaviour
{

    public List<GameObject> targets;
    private float SpawnRate = 1;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    private int score = 0;
    public int pointValue;




    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnTarget());
        UpdateScore(score);
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

    }
  







}
