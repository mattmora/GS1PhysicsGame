using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MJointController : MonoBehaviour
{
    HingeJoint joint;

    public float hingeSpringActiveStrength = 100f;
    public float hingeSpringRestStrength = 0f;

    public float hingeActivateAngle;
    public float hingeRestAngle;

    private void Start()
    {
        joint = GetComponent<HingeJoint>();

        joint.useSpring = true;
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

}
