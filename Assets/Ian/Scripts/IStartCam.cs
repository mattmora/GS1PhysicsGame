using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IStartCam : MonoBehaviour
{
    private void Awake()
    {
        if (GameObject.Find("ControlAndCamera(Clone)") != null)
        {
            Destroy(this.gameObject);
        }
    }
}
