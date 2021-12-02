using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(movement);
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
