using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MPlayerController : MonoBehaviour
{
    public MBone skull;
    public Rigidbody controlRb;

    [SerializeField] float moveForceMagnitude = 1f;
    [SerializeField] float torqueForceMagnitude = 1f;

    [SerializeField] float hInput, vInput;
    [SerializeField] float hInputRaw, vInputRaw;

    [SerializeField] float rotationSpeed = 100f;

    public float uprightForce;

    Camera mainCamera;

    public List<MBone> playerBones;

    public float smoothing;

    public IInputManager inputManager;
    [SerializeField] bool isfrozen = false;

    private void Awake()
    {
        playerBones = new List<MBone>();
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        inputManager = GetComponent<IInputManager>();
        GameObject[] bones = GameObject.FindGameObjectsWithTag("Bone");
        foreach (GameObject b in bones)
        {
            playerBones.Add(b.GetComponent<MBone>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Get movement inputs
        //hInput = Input.GetAxis("Horizontal");
        //vInput = Input.GetAxis("Vertical");
        hInput = inputManager.movement.x;
        vInput = inputManager.movement.y;

        // Get inputs without smoothing for checking whether keys are pressed
        //hInputRaw = Input.GetAxisRaw("Horizontal");
        //vInputRaw = Input.GetAxisRaw("Vertical");

        isfrozen = inputManager.freeze;

        if (inputManager.restart)
        {
            inputManager.restart = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        SmoothPosition();
    }

    private void FixedUpdate()
    {
        //Vector3 force = new Vector3(hInput, 0, vInput);
        //force = Vector3.ClampMagnitude(force, 1f) * moveForceMagnitude * Time.fixedDeltaTime;
        //controlRb.AddForce(force);

        controlRb.constraints = RigidbodyConstraints.None;
        //if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.LeftShift)) 
        if (isfrozen)
        {
            //if (Input.GetKey(KeyCode.Space))
            if (inputManager.jump)
            {
                Freeze();
            }
            //else if (!skull.connectionPoints[0].Ability())
            else { 
                Upright();
            }
           
        }
        //else
        //{
        //    if (!skull.connectionPoints[0].Basic())
        //    {
                Roll();
        //    }
        //}

        

        SmoothPosition();
    }

    void SmoothPosition()
    {
        float effectiveSmoothing = smoothing;
        if ((controlRb.worldCenterOfMass - transform.position).magnitude > 1f)
        {
            effectiveSmoothing = 0f;
        }
        transform.position = transform.position * smoothing + controlRb.worldCenterOfMass * (1f - smoothing); 
    }

    public void Freeze()
    {
        //if (Mathf.Abs(hInputRaw) > 0.01f)
        if (Mathf.Abs(hInput) > 0.01f)
        {
            return;
            //controlRb.transform.Rotate((hInput * Vector3.up) * Time.fixedDeltaTime * rotationSpeed, Space.World);
        }
        controlRb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void Upright()
    {
        var rot = Quaternion.FromToRotation(skull.connectionPoints[0].transform.forward, Vector3.up);
        
        controlRb.AddTorque(new Vector3(rot.x, rot.y, rot.z) * uprightForce - (controlRb.angularVelocity * Time.fixedDeltaTime) * uprightForce);
    }

    public void Roll()
    {
        Vector3 torque = (vInput * transform.right + hInput * -transform.forward);
        torque = Vector3.ClampMagnitude(torque, 1f) * torqueForceMagnitude * Time.fixedDeltaTime;
        controlRb.AddTorque(torque);

        Vector3 force = (vInput * transform.forward + hInput * transform.right);
        force = Vector3.ClampMagnitude(force, 1f) * moveForceMagnitude * Time.fixedDeltaTime;
        controlRb.AddForce(force);
    }
}
