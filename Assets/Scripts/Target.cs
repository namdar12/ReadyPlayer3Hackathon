using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Thirdweb;




public class Target : MonoBehaviour
{
    //Variables
    private Rigidbody targetRb;
    private GameManager gameManager;

    public ThirdwebSDKDemos thirdweb;
    private float minSpeed =12;
    private float maxSpeed=16;
    private float maxTorque = 10;
    private float xRange = 4;
    private float ySpawnPos = -6;

    private int lossCounter = 0;

    public int pointValue;
    public ParticleSystem explosionParticle;
   



    // Start is called before the first frame update
    void Start()
    {
     
        targetRb = GetComponent<Rigidbody>();
        targetRb.AddForce(Vector3.up * Random.Range(minSpeed,maxSpeed),ForceMode.Impulse);
        targetRb.AddTorque(Random.Range(-maxTorque,maxTorque),Random.Range(-maxTorque,maxTorque),
                            Random.Range(-maxTorque,maxTorque),ForceMode.Impulse);
        transform.position = new Vector3(Random.Range(-xRange,xRange),ySpawnPos);
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Extra functions
    private void OnMouseDown(){

        Destroy(gameObject);
        gameManager.UpdateScore(pointValue);
        Instantiate(explosionParticle,transform.position,explosionParticle.transform.rotation);
    }

    private void OnTriggerEnter(Collider other){
        if(!gameObject.CompareTag("Bad1")){
            lossCounter +=1;
            if(lossCounter == 3){
                gameManager.GameOver();

            }
        }
    }



}
