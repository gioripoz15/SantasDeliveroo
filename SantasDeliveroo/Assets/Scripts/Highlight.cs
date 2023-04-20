using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    public Material highlightMaterial;

    private List<Highlight> highlightList = new List<Highlight>();

    bool highlightOnStart = false;

    private void Start()
    {
        if (highlightOnStart)
        {
            RecurseHighlight();
        }
    }

    public void RecurseHighlight()
    {
        highlightList.Clear();
        foreach (var renderer in GetComponentsInChildren<MeshRenderer>())
        {
            highlightList.Add(HighlightRenderer(renderer));
        }
    }

    private Highlight HighlightRenderer(MeshRenderer renderer)
    {
        Highlight highlight =  (new GameObject("Highlight")).AddComponent<Highlight>();
        highlight.transform.position = renderer.transform.position;
        highlight.transform.rotation = renderer.transform.rotation;
        highlight.transform.parent = renderer.transform;
        highlight.transform.localScale = new Vector3(1,1,1);
        MeshRenderer meshRenderer =  highlight.gameObject.AddComponent<MeshRenderer>();
        MeshFilter meshfilter = highlight.gameObject.AddComponent<MeshFilter>();
        meshfilter.mesh = renderer.GetComponent<MeshFilter>().mesh;
        Material[] materials = new Material[renderer.materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = Material.Instantiate(highlightMaterial);
        }
        meshRenderer.materials = materials;
        return highlight;
    }

    public void RemoveHighlight()
    {
        foreach (var highlight in highlightList)
        {
            Destroy(highlight.gameObject);
        }
        Destroy(this);
    }
}
