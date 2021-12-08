using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRadio : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Jukebox.tmp.Collision(other);
    }
}
