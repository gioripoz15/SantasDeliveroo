using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : MonoBehaviour
{
    public GameObject targetHouse;
    public Santa Owner;
    [SerializeField]
    private Material highlightMaterial;
    [SerializeField]
    private LineRenderer linePrefab;
    private LineRenderer currentLine;

    [SerializeField]
    private GameObject model;
    [SerializeField]
    private Collider collider;

    public void PickUpGameobject()
    {
        //play some audio/particles;
        HighlightTargetHouse(false);
        model.SetActive(false);
        collider.enabled = false;
    }

    public void HighlightTargetHouse(bool add)
    {
        if (add)
        {
            Highlight highlight = targetHouse.GetComponent<Highlight>();
            if (highlight) return;

            highlight = targetHouse.AddComponent<Highlight>();
            highlight.highlightMaterial = highlightMaterial;
            highlight.RecurseHighlight();
            SetLineRenderer();

        }
        else
        {
            Highlight highlight = targetHouse?.GetComponent<Highlight>();
            if (highlight)
            {
                highlight.RemoveHighlight();
            }
            if (currentLine)
            {
                Destroy(currentLine.gameObject);
            }
        }
    }

    public void SetLineRenderer()
    {
        if (!currentLine)
        {
            currentLine = Instantiate(linePrefab);
            //currentLine.transform.parent = transform;
        }
        Vector3 startPosition = transform.position;
        if (Owner)
        {
            startPosition = Owner.transform.position;
        }
        currentLine.SetPosition(0, startPosition);
        currentLine.SetPosition(1, targetHouse.transform.position);
    }
}
