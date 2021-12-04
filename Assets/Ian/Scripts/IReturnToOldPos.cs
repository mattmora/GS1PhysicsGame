using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IReturnToOldPos : MonoBehaviour
{
    public Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToSpawnPoint()
    {
        transform.position = startPos;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
