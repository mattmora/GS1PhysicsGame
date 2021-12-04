using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IRespawn : MonoBehaviour
{
    public List<Transform> spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);

        if (other.gameObject.tag == "Skull")
        {
            Debug.Log("Hit skull");
            Vector3 playerPos = other.transform.position;
            int index = -1;
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                if (spawnPoints[i].position.z < playerPos.z) index = i;
                else break;
            }
            other.transform.position = spawnPoints[index].position;
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        else if (other.gameObject.tag == "Foot")
        {
            other.GetComponent<IReturnToOldPos>().ReturnToSpawnPoint();
        }
    }
}
