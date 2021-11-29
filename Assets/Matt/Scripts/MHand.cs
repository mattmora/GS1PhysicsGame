using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MHand : MonoBehaviour
{
    Rigidbody rb;
    CharacterJoint joint;

    HashSet<GameObject> collidedObjects;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        collidedObjects = new HashSet<GameObject>();
    }

    public void Grab()
    {
        if (collidedObjects.Count == 0) return;

        if (joint == null)
        {
            joint = gameObject.AddComponent<CharacterJoint>();
        }
    }

    public void Release()
    {
        if (joint != null)
        {
            Destroy(joint);
            joint = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        collidedObjects.Add(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        collidedObjects.Remove(collision.gameObject);
    }
}
