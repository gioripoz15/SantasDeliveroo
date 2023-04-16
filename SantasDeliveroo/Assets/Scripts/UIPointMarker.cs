using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPointMarker : MonoBehaviour
{
    [SerializeField]
    private Image circle;
    [SerializeField]
    private Image arrow;
    public Image Arrow => arrow;

    public Vector3 EndLinePoint
    {
        get { return arrow.transform.position; }
    }

    public void SetColor(Color color)
    {
        circle.color = color;
        arrow.color = color;
    }

    public void DisableArrow()
    {
        arrow.gameObject.SetActive(false);
    }
}
