using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPassCollisionToParent : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (transform.parent != null && transform.parent.gameObject.GetComponent<CParticleScaleVelocity>() != null)
        {
            transform.parent.gameObject.GetComponent<CParticleScaleVelocity>().OnCollisionEnter(collision);
        }
    }
}
