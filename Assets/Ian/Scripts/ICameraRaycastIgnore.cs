using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ICameraRaycastIgnore : MonoBehaviour
{
    public Transform target;

    public List<MeshRenderer> mrs;

    public float transparancy;

    void Start()
    {
        target = GetComponent<MDragMouseOrbit>().target;
        mrs = new List<MeshRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target == null) target = GetComponent<MDragMouseOrbit>().target;

        List<MeshRenderer> newMrs = new List<MeshRenderer>();

        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, target.transform.position - transform.position, Vector3.Distance(transform.position, target.transform.position));
        
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag != "Bone" && hit.transform.gameObject.GetComponent<MeshRenderer>() != null)
            {
                newMrs.Add(hit.transform.gameObject.GetComponent<MeshRenderer>());
                setOpacity(hit.transform.gameObject.GetComponent<MeshRenderer>(), transparancy);
            }
        }

        foreach (MeshRenderer mr in mrs.Except(newMrs))
        {
            setOpacity(mr, 1f);
        }

        mrs = newMrs;
    }

    private void setOpacity(MeshRenderer mr, float op)
    {
        if (mr == null) return;
        foreach (Material mat in mr.materials)
        {
            Color currentCol = mat.color;
            mat.color = new Color(currentCol.r, currentCol.g, currentCol.b, op);
        }
    }
}
