using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IUICamera : MonoBehaviour
{
    public Transform mainCamTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = mainCamTransform.position;
        transform.rotation = mainCamTransform.rotation;
        GetComponent<Camera>().rect = mainCamTransform.GetComponent<Camera>().rect;
    }
}
