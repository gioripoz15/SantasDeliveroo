using System.Collections;
using System.Collections.Generic;
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

    public Highlight PermanentHighlight => permanentHighlight;

    public void HighlightHouse(Material highlightMaterial)
    {
        if (highlight) return;

        highlight = gameObject.AddComponent<Highlight>();
        highlight.highlightMaterial = highlightMaterial;
        highlight.RecurseHighlight();
    }
    public void RemoveHighlight()
    {
        if (highlight)
        {
            highlight.RemoveHighlight();
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
