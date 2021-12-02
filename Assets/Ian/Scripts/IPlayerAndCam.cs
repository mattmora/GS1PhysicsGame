using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IPlayerAndCam : MonoBehaviour
{
    public PlayerInputManager pim;

    private void Awake()
    {
        pim = GameObject.Find("PlayerInputManager").GetComponent<PlayerInputManager>();
        //transform.position = transform.position - new Vector3((pim.playerCount - 1) * 2, 0, 0);
    }
}
