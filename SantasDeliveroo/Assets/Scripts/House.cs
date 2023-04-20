using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class House : MonoBehaviour
{
	public List<Gift> assignedGifts = new List<Gift>();

    [SerializeField]
    private LineRenderer linePrefab;
    private List<LineRenderer> currentLines = new List<LineRenderer>();

    private Highlight highlight;
    public Highlight Highlight => highlight;

    [SerializeField]
    private Highlight permanentHighlight;

    private List<MeshRenderer> allMeshRenderers = new List<MeshRenderer>();
    private List<MeshRenderer> AllMeshRenderers
    {
        get
        {
            if(allMeshRenderers.Count == 0)
            {
                allMeshRenderers = GetComponentsInChildren<MeshRenderer>().ToList();
            }
            return allMeshRenderers;
        }
        set
        {
            allMeshRenderers = value;
        }
    }

    private Highlight PermanentHighlight => permanentHighlight;

    public bool startHighlighted = false;


    private void Start()
    {
        if (startHighlighted)
        {
            permanentHighlight.RecurseHighlight();
            foreach (var rend in AllMeshRenderers)
            {
                rend.enabled = false;
            }
        }
    }

    public void HighlightHouse(Material highlightMaterial)
    {
        if (highlight) return;

        highlight = gameObject.AddComponent<Highlight>();
        highlight.highlightMaterial = highlightMaterial;
        highlight.RecurseHighlight();
        foreach (var rend in AllMeshRenderers)
        {
            rend.enabled = false;
        }
    }
    public void RemoveHighlight()
    {
        if (highlight)
        {
            highlight.RemoveHighlight();
            foreach (var rend in AllMeshRenderers)
            {
                rend.enabled = true;
            }
        }
    }

    public void SetLineRenderer(bool on)
    {
        if (on)
        {

            foreach (var line in currentLines)
            {
                Destroy(line.gameObject);
            }
            currentLines.Clear();

            assignedGifts.RemoveAll(x => x == null);
            foreach (var gift in assignedGifts)
            {
                if(gift!= null)
                {
                    Vector3 giftPosition = gift.Owner ? gift.Owner.transform.position : gift.transform.position;
                    LineRenderer currentLine = Instantiate(linePrefab);
                    Vector3 startPosition = transform.position;
                    currentLine.SetPosition(0, startPosition);
                    currentLine.SetPosition(1, giftPosition);
                    currentLines.Add(currentLine);
                }
                
            }
                
        }
        else
        {
            foreach(var line in currentLines)
            {
                Destroy(line.gameObject);
            }
            currentLines.Clear();
        }
        
        
    }
}
