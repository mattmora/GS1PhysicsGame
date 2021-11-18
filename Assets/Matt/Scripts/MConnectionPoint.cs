using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MConnectionPoint : MonoBehaviour
{
    MBone bone;

    HingeJoint joint;

    public float hingeSpringStrength = 100f;

    public float hingeRestAngle;
    public float hingeActivateAngle;

    bool ignoreConnections = false;

    MBone connectedBone;

    // Start is called before the first frame update
    void Start()
    {
        bone = transform.GetComponentInParent<MBone>();
        bone.connectionPoints.Add(this);
        gameObject.tag = "Connection";

        transform.Find("ConnectionReference").gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (joint == null) return;

        if (Input.GetKey(KeyCode.Space))
        {
            //joint.useSpring = true;
            JointSpring hingeSpring = joint.spring;
            hingeSpring.spring = hingeSpringStrength;
            hingeSpring.damper = 0.1f;
            hingeSpring.targetPosition = hingeActivateAngle;
            joint.spring = hingeSpring;
        }
        else
        {
            //joint.useSpring = false;
            JointSpring hingeSpring = joint.spring;
            hingeSpring.spring = hingeSpringStrength;
            hingeSpring.damper = 0.1f;
            hingeSpring.targetPosition = hingeRestAngle;
            joint.spring = hingeSpring;
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.X))
        {
            Disconnect();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ignoreConnections) return;
        if (other.gameObject.tag == "Connection")
        {
            MBone otherBone = other.gameObject.GetComponentInParent<MBone>();
            MConnectionPoint otherConnectionPoint = other.gameObject.GetComponent<MConnectionPoint>();

            if (bone.attachedToPlayer && !otherBone.attachedToPlayer)
            {
                // Align the transform before attaching with joint?
                otherBone.transform.position = transform.position;
                otherBone.transform.rotation = transform.rotation;

                otherBone.attachedToPlayer = true;
                joint = bone.gameObject.AddComponent<HingeJoint>();

                joint.axis = Vector3.right;
                //joint.swingAxis = new Vector3(1f, 0f, 1f);

                joint.enableCollision = true;
                joint.autoConfigureConnectedAnchor = false;
                joint.anchor = transform.localPosition;
                joint.connectedBody = otherBone.rb;
                joint.connectedAnchor = otherConnectionPoint.transform.localPosition;

                joint.useSpring = true;

                connectedBone = otherBone;
                ignoreConnections = true;
            }
        }
    }

    public void Disconnect()
    {
        if (joint == null) return;

        Destroy(joint);
        connectedBone.attachedToPlayer = false;
        connectedBone = null;
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1f);
        ignoreConnections = false;
    }
}
