using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : MonoBehaviour
{
	public GameObject TargetHouse;
	public Santa Owner;
    [SerializeField]
    private Material highlightMaterial;

	public void PickUpGameobject()
    {
		//play some audio/particles;
		gameObject.SetActive(false);
    }

	public void HighlightTargetHouse(bool add)
    {
        if (add)
        {
            Highlight highlight = TargetHouse.GetComponent<Highlight>();
            if (highlight) return;

            highlight = TargetHouse.AddComponent<Highlight>();
            highlight.highlightMaterial = highlightMaterial;
            highlight.RecurseHighlight();
            
        }
        else
        {
            Highlight highlight = TargetHouse.GetComponent<Highlight>();
            if (highlight)
            {
                highlight.RemoveHighlight();
            }
        }
    }
}
