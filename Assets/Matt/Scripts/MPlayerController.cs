using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPlayerController : MonoBehaviour
{
    public Rigidbody controlRb;

    [SerializeField] float moveForceMagnitude = 1f;

    [SerializeField] float hInput, vInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");

        transform.position = controlRb.worldCenterOfMass;
    }

    private void FixedUpdate()
    {
        //Vector3 force = new Vector3(hInput, 0, vInput);
        //force = Vector3.ClampMagnitude(force, 1f) * moveForceMagnitude * Time.fixedDeltaTime;
        //controlRb.AddForce(force);

        Vector3 force = new Vector3(vInput, 0, -hInput);
        force = Vector3.ClampMagnitude(force, 1f) * moveForceMagnitude * Time.fixedDeltaTime;
        controlRb.AddTorque(force);

        transform.position = controlRb.worldCenterOfMass;
    }
}
