using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : MonoBehaviour
{
    public GameObject TargetHouse;
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

    private void OnDestroy()
    {
        HighlightTargetHouse(false);
    }

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
            Highlight highlight = TargetHouse.GetComponent<Highlight>();
            if (highlight) return;

            highlight = TargetHouse.AddComponent<Highlight>();
            highlight.highlightMaterial = highlightMaterial;
            highlight.RecurseHighlight();
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
            currentLine.SetPosition(1, TargetHouse.transform.position);

        }
        else
        {
            Highlight highlight = TargetHouse.GetComponent<Highlight>();
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
}
