using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBone : MonoBehaviour
{
    public bool attachedToPlayer = false;

    [HideInInspector]
    public Rigidbody rb;

    // Child connection points will add themselves to this list at runtime
    public List<MConnectionPoint> connectionPoints = new List<MConnectionPoint>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
