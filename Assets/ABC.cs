using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ABC : MonoBehaviour
{
    public TextMesh g;
    public string a;
    public Font f1,f2;

    public void F1()
    {
        g.font = f1;
        MeshRenderer meshRenderer = g.GetComponent<MeshRenderer>();
        meshRenderer.materials = new Material[] {f1.material};
        g.text = a;
    }

    public void F2()
    {
        g.font = f2;
        MeshRenderer meshRenderer = g.GetComponent<MeshRenderer>();
        meshRenderer.materials = new Material[] { f2.material };
        g.text = a;
    }
}
