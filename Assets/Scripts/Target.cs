using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    //Variables
    private Rigidbody targetRb;
    private float minSpeed =12;
    private float maxSpeed=16;
    private float maxTorque = 10;
    private float xRange = 4;
    private float ySpawnPos = -6;



    // Start is called before the first frame update
    void Start()
    {

        targetRb = GetComponent<Rigidbody>();
        targetRb.AddForce(Vector3.up * Random.Range(minSpeed,maxSpeed),ForceMode.Impulse);
        targetRb.AddTorque(Random.Range(-maxTorque,maxTorque),Random.Range(-maxTorque,maxTorque),
                            Random.Range(-maxTorque,maxTorque),ForceMode.Impulse);
        transform.position = new Vector3(Random.Range(-xRange,xRange),ySpawnPos);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Extra functions

}
