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

    public float secretForce;

    Vector3 followVelocity;

    [HideInInspector]
    public GameObject camTarget;
    private void Awake()
    {
        playerBones = new List<MBone>();
        camTarget = new GameObject();
        camTarget.name = "CamTarget";
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        //inputManager = GetComponent<IInputManager>();
        GameObject[] bones = GameObject.FindGameObjectsWithTag("Bone");
        foreach (GameObject b in bones)
        {
            playerBones.Add(b.GetComponent<MBone>());
        }

        camTarget.transform.position = transform.position + Vector3.up * 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (inputManager == null)
        {
            SmoothPosition();
            return;
        }
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
        if (inputManager == null) return;
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
            else
            {
                Upright();
            }
            Pivot();
            //Roll();
        }
        else
        {
            Roll();
        }
    }

    void SmoothPosition()
    {
        Vector3 smooth = Vector3.SmoothDamp(transform.position, controlRb.worldCenterOfMass, ref followVelocity, 0.01f);
        transform.position = controlRb.worldCenterOfMass;

        float x = camTarget.transform.position.x;
        float y = camTarget.transform.position.y;
        float z = camTarget.transform.position.z;

        //if (transform.position.x > x)
        //{
        //    x = transform.position.x;
        //}
        //else if (transform.position.x < x - 0.2f)
        //{
        //    x = transform.position.x + 0.2f;
        //}

        x = transform.position.x;

        if (transform.position.y > y)
        {
            y = transform.position.y;
        }
        else if (transform.position.y < y - 0.2f)
        {
            y = transform.position.y + 0.2f;
        }

        z = transform.position.z;

        camTarget.transform.position = new Vector3(x, y, z);

        //if (transform.position.z >z)
        //{
        //    z = transform.position.z;
        //}
        //else if (transform.position.z < z - 0.2f)
        //{
        //    z = transform.position.z + 0.2f;
        //}

        //float effectiveSmoothing = smoothing;
        //if ((controlRb.worldCenterOfMass - transform.position).magnitude > 1f)
        //{
        //    effectiveSmoothing = 0f;
        //}
        //transform.position = transform.position * smoothing + controlRb.worldCenterOfMass * (1f - smoothing); 
    }

    public void Freeze()
    {
        //if (Mathf.Abs(hInputRaw) > 0.01f)
        //if (Mathf.Abs(hInput) > 0.01f)
        //{
            //return;
            //controlRb.transform.Rotate((hInput * Vector3.up) * Time.fixedDeltaTime * rotationSpeed, Space.World);
        //}
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

    public void Pivot()
    {
        //Vector3 torque = (vInput * transform.right + hInput * -transform.forward);
        //torque = Vector3.ClampMagnitude(torque, 1f) * -torqueForceMagnitude * Time.fixedDeltaTime;
        //controlRb.AddTorque(torque);

        Vector3 face = (vInput * transform.forward + hInput * transform.right);
        var rot = Quaternion.FromToRotation(-skull.connectionPoints[0].transform.up, face);
        // Debug.Log(new Vector3(rot.x, rot.y, rot.z));
        controlRb.AddTorque(new Vector3(rot.x, rot.y, rot.z) * uprightForce * 2);
        //controlRb.AddForce(Vector3.up * secretForce);
    }
}
