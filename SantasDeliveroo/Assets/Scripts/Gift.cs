using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : MonoBehaviour
{
    public GameObject targetHouseGameObject;
    private House targetHosue;
    private House TargetHouse
    {
        get
        {
            if (!targetHosue)
            {
                targetHosue = targetHouseGameObject.GetComponentInChildren<House>();
                if (!targetHosue)
                {
                    targetHosue = targetHouseGameObject.GetComponentInParent<House>();
                }
            }
            return targetHosue;
        }
    }
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

    public void Deliver()
    {
        if (currentLine)
        {
            Destroy(currentLine);
        }
    }

    //highlight the hosue adding the line
    public void HighlightTargetHouse(bool add)
    {
        if (add)
        {
            TargetHouse.HighlightHouse(highlightMaterial);
            SetLineRenderer();

        }
        else
        {
            TargetHouse.RemoveHighlight();
            if (currentLine)
            {
                Destroy(currentLine.gameObject);
            }
        }
    }

    //handle the line renderer to the house
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
        currentLine.SetPosition(1, targetHouseGameObject.transform.position);
    }
}
