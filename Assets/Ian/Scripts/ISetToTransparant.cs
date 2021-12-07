using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISetToTransparant : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        setToTransparantRecursive(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void setToTransparantRecursive(Transform t)
    {
        if (t.gameObject.GetComponent<MeshRenderer>() != null)
        {
            Material[] mats = t.gameObject.GetComponent<MeshRenderer>().sharedMaterials;
            foreach (Material m in mats)
            {
                m.SetFloat("_Mode", 3);
                m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                m.EnableKeyword("_ALPHABLEND_ON");
                m.renderQueue = 3000;
                
            }
            return;
        }
        else
        {
            for (int i=0; i<t.childCount; i++)
            {
                setToTransparantRecursive(t.GetChild(i));
            }
        }
    }
}
