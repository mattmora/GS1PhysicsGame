using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCollectible : MonoBehaviour
{
    public Vector3 rotation;

    GameObject collectible;

    // Start is called before the first frame update
    void Start()
    {
        // Get the collectible which should be the first and only child
        collectible = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the collectible through this as the parent so 
        // we don't need to worry about where each objects center is
        transform.Rotate(rotation * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Skull")
        {
            // Set the hat instances on the skull inactive
            Transform collectibles = other.transform.Find("Collectibles");
            for (int i = 0; i < collectibles.childCount; ++i)
            {
                collectibles.GetChild(i).gameObject.SetActive(false);
            }
            // Activate the hat matchiing the one just collected
            collectibles.Find(collectible.name).gameObject.SetActive(true);
        }
    }
}
