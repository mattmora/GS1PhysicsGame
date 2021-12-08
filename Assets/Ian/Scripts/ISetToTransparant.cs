using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ISetToTransparant : MonoBehaviour
{
    public List<Material> mats = new List<Material>();

    public bool toggle;

    // Start is called before the first frame update
    void Start()
    {
        //setToTransparantRecursive(transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (toggle)
        {
            setToTransparantRecursive(transform);
            toggle = false;
        }
    }

    private void setToTransparantRecursive(Transform t)
    {
        if (t.gameObject.GetComponent<MeshRenderer>() != null)
        {
            Material[] ms = t.gameObject.GetComponent<MeshRenderer>().sharedMaterials;
            Material[] newMats = new Material[ms.Length];
            for (int i=0; i<ms.Length; i++)
            {
                Material m = ms[i];
                m.SetFloat("_Mode", 3);
                m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                m.EnableKeyword("_ALPHABLEND_ON");
                m.renderQueue = 3000;
                
                /*
                Material m = ms[i];
                switch (m.name)
                {
                    case "lambert1SG": newMats[i] = mats[0];
                        break;
                    case "lambert2SG":
                        newMats[i] = mats[1];
                        break;
                    case "lambert3SG":
                        newMats[i] = mats[2];
                        break;
                    case "lambert4SG":
                        newMats[i] = mats[3];
                        break;
                }
                t.gameObject.GetComponent<MeshRenderer>().sharedMaterials = newMats;
                */
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
