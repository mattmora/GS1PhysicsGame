using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MConnectionPoint : MonoBehaviour
{
    MBone bone;

    HingeJoint joint;

    public float hingeSpringActiveStrength = 100f;
    public float hingeSpringRestStrength = 0f;

    public float hingeActivateAngle;
    public float hingeRestAngle;

    bool ignoreConnections = false;

    MBone connectedBone;

    // Hacky, think of these two lists as a Dictionary
    // bones are used to call actions at the same index
    // (Unity does serialize Dictionaries by default) 
    public List<MBone> bones;
    public List<UnityEvent> activeActions;
    public List<UnityEvent> restActions;
    public List<float> boneHingeSpringStrength;
    public List<float> boneAngle;

    public List<UnityEvent> ability;
    public List<UnityEvent> basic;

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
        if (connectedBone == null) return;

        if (bones.Contains(connectedBone))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                activeActions[bones.IndexOf(connectedBone)].Invoke();
            }
            else
            {
                restActions[bones.IndexOf(connectedBone)].Invoke();
            }
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.X))
        {
            Disconnect();
        }
    }

    public bool Ability()
    {
        if (connectedBone == null) return false;
        ability[bones.IndexOf(connectedBone)].Invoke();
        return true;
    }

    public bool Basic()
    {
        if (connectedBone == null) return false;
        basic[bones.IndexOf(connectedBone)].Invoke();
        return true;
    }

    public void ActivateHinge()
    {
        //joint.useSpring = true;
        JointSpring hingeSpring = joint.spring;
        hingeSpring.spring = hingeSpringActiveStrength;
        hingeSpring.damper = 0.1f;
        hingeSpring.targetPosition = hingeActivateAngle;
        joint.spring = hingeSpring;
    }

    public void DeactivateHinge()
    {
        //joint.useSpring = false;
        JointSpring hingeSpring = joint.spring;
        hingeSpring.spring = hingeSpringRestStrength;
        hingeSpring.damper = 0.1f;
        hingeSpring.targetPosition = hingeRestAngle;
        joint.spring = hingeSpring;
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
                otherConnectionPoint.ignoreConnections = true;

                connectedBone = otherBone;

                joint = bone.gameObject.AddComponent<HingeJoint>();

                joint.axis = Vector3.right;
                //joint.swingAxis = new Vector3(1f, 0f, 1f);

                //joint.enableCollision = true;
                joint.autoConfigureConnectedAnchor = false;
                joint.anchor = transform.localPosition;
                joint.connectedBody = otherBone.rb;
                joint.connectedAnchor = otherConnectionPoint.transform.localPosition;

                joint.useSpring = true;

                JointSpring hingeSpring = joint.spring;
                hingeSpring.spring = boneHingeSpringStrength[bones.IndexOf(connectedBone)];
                hingeSpring.damper = 0.1f;
                hingeSpring.targetPosition = boneAngle[bones.IndexOf(connectedBone)];
                joint.spring = hingeSpring;

                ignoreConnections = true;

                connectedBone.transform.SetParent(bone.transform);
            }
        }
    }

    public void Disconnect()
    {
        if (connectedBone == null) return;

        Destroy(joint);
        connectedBone.transform.SetParent(null);
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
