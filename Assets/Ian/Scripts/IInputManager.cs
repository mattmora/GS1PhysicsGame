using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class IInputManager : MonoBehaviour
{
    public Vector2 movement = Vector2.zero;
    public Vector2 mouseMovement = Vector2.zero;
    public float mouseX, mouseY;
    public bool freeze = false;
    public bool restart = false;
    public bool LMB = false;
    public bool ESC = false;
    public bool jump = false;
    public bool detach = false;

    public GameObject[] playerAndSkulls;
    private PlayerInput pi;

    public int id;

    public string[] fakeStuffs;

    //public bool flag;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //if (!flag)
        //if (fakeStuff != null) Destroy(fakeStuff);
        createPlayer();
        //}
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(transform.parent.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        fakeStuffs = new string[] { "FakePlayerAndSkullStanding", "FakePlayerAndSkullSitPose", "FakePlayerAndSkullSitLeanF", "FakePlayerAndSkullLyingOnFloor" };
        restart = false;
        pi = GetComponent<PlayerInput>();
        id = PlayerInputManager.instance.playerCount;
        //fakeStuff = GameObject.Find("FakeStuff");
        Debug.Log(id);
        createPlayer();

        
    }
    
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(movement);
        disableBorders();
        switch (PlayerInputManager.instance.playerCount)
        {
            case 1: break;
            case 2:
                if (id == 1) enableOneBorder("bottom");
                else enableOneBorder("top");
                break;
            case 3:
                if (id == 1)
                {
                    enableOneBorder("right");
                    enableOneBorder("bottom");
                }
                else if (id == 2)
                {
                    enableOneBorder("left");
                    enableOneBorder("bottom");
                }
                else enableOneBorder("top");
                break;
            case 4:
                if (id == 1)
                {
                    enableOneBorder("right");
                    enableOneBorder("bottom");
                }
                else if (id == 2)
                {
                    enableOneBorder("left");
                    enableOneBorder("bottom");
                }
                else if (id == 3)
                {
                    enableOneBorder("right");
                    enableOneBorder("top");
                }
                else
                {
                    enableOneBorder("left");
                    enableOneBorder("top");
                }
                break;
        }
    }

    private void createPlayer()
    {
        GameObject fakeStuff = GameObject.Find(fakeStuffs[id-1]);
        Transform tempT = fakeStuff.transform;
        if (fakeStuff != null) Destroy(fakeStuff);

        Debug.Log(id);
        GameObject obj = Instantiate(playerAndSkulls[id-1]);
        obj.transform.position = tempT.position;
        obj.transform.localRotation = tempT.localRotation;
        //pi.camera = obj.transform.GetChild(1).GetComponent<Camera>();
        obj.transform.Find("Player").gameObject.GetComponent<MPlayerController>().inputManager = this;
        pi.camera.gameObject.GetComponent<MDragMouseOrbit>().target = obj.transform.Find("Player");
        obj.transform.Find("skull").GetComponent<CParticleScaleVelocity>().im = this;
        obj.transform.Find("skull").Find("SkullConnectionPoint").gameObject.GetComponent<MConnectionPoint>().inputManager = this;
        //obj.transform.Find("Main Camera").gameObject.GetComponent<MDragMouseOrbit>().inputManager = this;
        //obj.transform.Find("Main Camera").gameObject.GetComponent<MMouseLock>().inputManager = this;

        if (transform.parent.Find("StartCam") == null)
        {
            //if (!transform.Find("Main Camera").parent.gameObject.activeSelf)
            //{
                GameObject startCam = GameObject.Find("StartCam");
                if (startCam != null && (startCam.transform.parent == null))
                {
                    MDragMouseOrbit md = startCam.GetComponent<MDragMouseOrbit>();
                    md.target = obj.transform.Find("Player");
                    md.inputManager = this;
                    MMouseLock mm = startCam.GetComponent<MMouseLock>();
                    mm.inputManager = this;
                    startCam.transform.SetParent(transform.parent);
                    pi.camera = startCam.GetComponent<Camera>();
                    transform.parent.Find("UICamera").GetComponent<IUICamera>().mainCamTransform = startCam.transform;
                }
                else
                {
                    transform.parent.Find("Main Camera").gameObject.SetActive(true);
                }
            //}
        }
        //else
        //{
            
        //}
    }

    private void disableBorders()
    {
        Transform borders = transform.parent.Find("Borders");
        for (int i=0; i<borders.childCount; i++)
        {
            borders.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void enableOneBorder(string side)
    {
        Transform borders = transform.parent.Find("Borders");
        borders.Find(side + "border").gameObject.SetActive(true);
    }

    public void onMove(InputAction.CallbackContext ctx)
    {
        movement = ctx.ReadValue<Vector2>();
        //Debug.Log(movement);
    }

    public void onFreeze(InputAction.CallbackContext ctx)
    {
        freeze = ctx.action.triggered;
    }

    public void onRestart(InputAction.CallbackContext ctx)
    {
        restart = ctx.action.triggered;
    }

    public void onMouseX(InputAction.CallbackContext ctx)
    {
        //Debug.Log("hey");
        //mouseMovement = ctx.ReadValue<Vector2>();
        mouseX = ctx.ReadValue<float>();
        //Debug.Log(mouseX);
    }

    public void onMouseY(InputAction.CallbackContext ctx)
    {
        mouseY = ctx.ReadValue<float>();
    }

    public void onLMB(InputAction.CallbackContext ctx)
    {
        LMB = ctx.action.triggered;
    }

    public void onESC(InputAction.CallbackContext ctx)
    {
        ESC = ctx.action.triggered;
    }

    public void onJump(InputAction.CallbackContext ctx)
    {
        jump = ctx.action.triggered;
    }

    public void onDetach(InputAction.CallbackContext ctx)
    {
        detach = ctx.action.triggered;
    }
}
