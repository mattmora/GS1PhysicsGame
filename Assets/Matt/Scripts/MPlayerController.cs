using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MPlayerController : MonoBehaviour
{
    public Rigidbody controlRb;

    [SerializeField] float moveForceMagnitude = 1f;
    [SerializeField] float rightingForceMagnitude = 1f;

    [SerializeField] float hInput, vInput;
    [SerializeField] float hInputRaw, vInputRaw;

    Camera mainCamera;

    List<MBone> playerBones;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Get movement inputs
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");

        // Get inputs without smoothing for checking whether keys are pressed
        hInputRaw = Input.GetAxisRaw("Horizontal");
        vInputRaw = Input.GetAxisRaw("Vertical");

        transform.position = controlRb.worldCenterOfMass;

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void FixedUpdate()
    {
        //Vector3 force = new Vector3(hInput, 0, vInput);
        //force = Vector3.ClampMagnitude(force, 1f) * moveForceMagnitude * Time.fixedDeltaTime;
        //controlRb.AddForce(force);

        controlRb.constraints = RigidbodyConstraints.None;
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.LeftShift)) 
        {
            if (hInputRaw < 0.01f)
            {
                controlRb.constraints = RigidbodyConstraints.FreezeRotation;
            }
            else
            {
                Vector3 force = (hInput * transform.up);
                force = Vector3.ClampMagnitude(force, 1f) * moveForceMagnitude * Time.fixedDeltaTime;
                controlRb.AddTorque(force);
            }
        }
        else
        {
            Vector3 force = (vInput * transform.right + hInput * -transform.forward);
            force = Vector3.ClampMagnitude(force, 1f) * moveForceMagnitude * Time.fixedDeltaTime;
            controlRb.AddTorque(force);
        }
       

        transform.position = controlRb.worldCenterOfMass;
    }
}
