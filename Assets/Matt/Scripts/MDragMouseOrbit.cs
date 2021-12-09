using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class MDragMouseOrbit : MonoBehaviour
{
    public Transform target;
    public float distance = 2.0f;
    public float xSpeed = 20.0f;
    public float ySpeed = 20.0f;
    public float yMinLimit = -90f;
    public float yMaxLimit = 90f;
    public float distanceMin = 10f;
    public float distanceMax = 10f;
    public float smoothTime = 2f;
    float rotationYAxis = 0.0f;
    float rotationXAxis = 0.0f;
    float velocityX = 0.0f;
    float velocityY = 0.0f;
    // Use this for initialization

    public IInputManager inputManager;

    Transform realTarget;

    public GameObject titleCanvas;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        rotationYAxis = angles.y;
        rotationXAxis = angles.x;
        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }

        inputManager = transform.parent.GetChild(0).gameObject.GetComponent<IInputManager>();

        
    }
    void Update()
    {
        if (inputManager != null)
        {
            if (PlayerInputManager.instance.playerCount == 2)
            {
                if (inputManager.id == 1) gameObject.GetComponent<Camera>().rect = new Rect(0f, 0.5f, 1f, 0.5f);
                else if (inputManager.id == 2) gameObject.GetComponent<Camera>().rect = new Rect(0f, 0f, 1f, 0.5f);
            }
            if (inputManager.id == 3 && PlayerInputManager.instance.playerCount == 3)
            {
                //Debug.Log("3 players rn");
                gameObject.GetComponent<Camera>().rect = new Rect(0f, 0f, 1f, 0.5f);
                //rect.width = 1;
                //gameObject.GetComponent<Camera>().rect = rect;
            }
            else { }

            if (titleCanvas != null) titleCanvas.SetActive(false);
        }

        if (target != null)
        {
            MPlayerController player = target.gameObject.GetComponent<MPlayerController>();
            if (player != null) realTarget = player.camTarget.transform;
        }

        //if (realTarget == null) realTarget = target;

        if (target)
        {
            //if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            if (inputManager != null)
            {
                velocityX += xSpeed * inputManager.mouseX * distance * Time.deltaTime;
                velocityY += ySpeed * inputManager.mouseY * Time.deltaTime;
            }
            rotationYAxis += velocityX;
            rotationXAxis -= velocityY;
            rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);
            Quaternion fromRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
            Quaternion toRotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);
            Quaternion rotation = toRotation;

            //distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);
            RaycastHit hit;
            //if (Physics.Linecast(target.position, transform.position, out hit))
            //{
            //    distance -= hit.distance;
            //}
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + realTarget.position;

            transform.rotation = rotation;
            transform.position = position;
            velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime * smoothTime);
            velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * smoothTime);

            // Rotate the player for input direction purposes
            target.transform.rotation = Quaternion.Euler(target.transform.eulerAngles.x, transform.eulerAngles.y, target.transform.eulerAngles.z);
        }
    }
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}