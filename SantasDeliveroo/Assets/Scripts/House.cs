using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
	public List<Gift> assignedGifts = new List<Gift>();

    [SerializeField]
    private LineRenderer linePrefab;
    private List<LineRenderer> currentLines = new List<LineRenderer>();
    public void SetLineRenderer(bool on)
    {
        if (on)
        {
            assignedGifts.RemoveAll(x => x == null);
            foreach (var gift in assignedGifts)
            {
                if(gift!= null)
                {
                    LineRenderer currentLine = Instantiate(linePrefab);
                    Vector3 startPosition = transform.position;
                    currentLine.SetPosition(0, startPosition);
                    currentLine.SetPosition(1, gift.transform.position);
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
