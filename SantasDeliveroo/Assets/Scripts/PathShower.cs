using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class PathShower : MonoBehaviour
{
    [SerializeField]
    private RectTransform canvas;
    [SerializeField]
    private UIPointMarker markerPrefab;
    [SerializeField]
    private LineRenderer linePrefab;

    [SerializeField]
    private Color currentActionColor;
    [SerializeField]
    private Color plannedActionColor;

    private bool show;

    private Dictionary<PathPoint, List<GameObject>> pathObjects = new Dictionary<PathPoint, List<GameObject>>();

    private PathPoint lastAddedPathPoint = new PathPoint();

    private RectTransform selectedSantaMarker;

    private void Start()
    {
        //subscribe to  all events to update path
        SelectionHandler.Instance.SantaSelected -= SantaSelected;
        SelectionHandler.Instance.SantaSelected += SantaSelected;
        SelectionHandler.Instance.SantaSelected += (Santa santa) => show = true;
        SelectionHandler.Instance.SantaSelected += (Santa santa) => santa.pointRemovedFromList -= RemovePoint;
        SelectionHandler.Instance.SantaSelected += (Santa santa) => santa.pointRemovedFromList += RemovePoint;
        SelectionHandler.Instance.SantaSelected += (Santa santa) => santa.pointAddedToList -= AddPoint;
        SelectionHandler.Instance.SantaSelected += (Santa santa) => santa.pointAddedToList += AddPoint;
        SelectionHandler.Instance.Deselect -= Clear;
        SelectionHandler.Instance.Deselect += Clear;
        SelectionHandler.Instance.Deselect += () => show = false;
    }

    private void Update()
    {
        if (show)
        {
            UpdateSantaMarker();
            UpdateLine();
        }
    }

    private void SantaSelected(Santa santa)
    {
        Clear();
        DrawSelectedSantaPath(santa.PathPoints);
    }

    private void Clear()
    {
        //cycle on dictionary
        if (selectedSantaMarker)
        {
            Destroy(selectedSantaMarker.gameObject);
        }
        foreach (var goList in pathObjects.Values)
        {
            foreach (var go in goList)
            {
                Destroy(go);
            }
        }
        pathObjects.Clear();
        lastAddedPathPoint = new PathPoint();
        
    }

    private void RemovePoint(PathPoint point)
    {
        if (pathObjects.TryGetValue(point, out List<GameObject> objectsPoints))
        {
            foreach (var obj in objectsPoints)
            {
                Destroy(obj);
            }
            objectsPoints.Clear();
            pathObjects.Remove(point);
        }
        if (point.nextPathPoint != null)
        {
            PathPoint pointToUpdate = point.nextPathPoint;
            pointToUpdate.previousPathPoint = null;
            UpdatePoint(pointToUpdate);
        }
    }

    private void AddPoint(PathPoint point)
    {
        PathPoint previousPoint = new PathPoint();
        if (SelectionHandler.Instance.SelectedSanta.PathPoints.Count <= 1)
        {
            Clear();
        }
        if (pathObjects.Count > 0)
        {
            previousPoint = lastAddedPathPoint;
            if (point.previousPathPoint != null)
            {
                previousPoint = point.previousPathPoint;
            }
        }
        pathObjects.Add(point, CreatePointObjects(previousPoint, point));
        lastAddedPathPoint = point;
        UpdatePoint(point);
    }

    //used to create a new point in the path
    private List<GameObject> CreatePointObjects(PathPoint previousPoint, PathPoint point)
    {
        List<GameObject> pointObjects = new List<GameObject>();
        UIPointMarker currentMarker = Instantiate(markerPrefab);
        RectTransform currentMarkerRectTransform = currentMarker.GetComponent<RectTransform>();
        currentMarker.transform.parent = transform;

        currentMarkerRectTransform.anchoredPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, point.position) - canvas.sizeDelta / 2;
        

        currentMarker.SetColor(!previousPoint.targettedObject ? currentActionColor : plannedActionColor);
        
        pointObjects.Add(currentMarker.gameObject);
        
        if (!selectedSantaMarker)
        {
            //to roundSanta
            UIPointMarker santaMarker = Instantiate(markerPrefab);
            RectTransform currentSantaMarkerRectTransform = santaMarker.GetComponent<RectTransform>();
            santaMarker.transform.parent = transform;
            currentSantaMarkerRectTransform.anchoredPosition = RectTransformUtility.WorldToScreenPoint(Camera.main,
                SelectionHandler.Instance.SelectedSanta.transform.position) - canvas.sizeDelta / 2;
            selectedSantaMarker = currentSantaMarkerRectTransform;
            santaMarker.SetColor(currentActionColor);
            santaMarker.DisableArrow();
            previousPoint.position = SelectionHandler.Instance.SelectedSanta.transform.position;
        }
        RectTransform lookAtRect = selectedSantaMarker;
        if (pathObjects.TryGetValue(previousPoint, out List<GameObject> objects))
        {
            foreach (var obj in objects)
            {
                if (obj.TryGetComponent<UIPointMarker>(out UIPointMarker marker))
                {
                    lookAtRect = marker.GetComponent<RectTransform>();
                    break;
                }
            }
        }
        LineRenderer line = CreateLine(previousPoint.position, point.position);
        pointObjects.Add(line.gameObject);
        line.material.color = !previousPoint.targettedObject ? currentActionColor : plannedActionColor;
        UIYLookAt(currentMarkerRectTransform, lookAtRect);

        return pointObjects;
    }

    private void DrawSelectedSantaPath(List<PathPoint> path)
    {
        PathPoint prevPoint = new PathPoint();
        Clear();
        foreach (var point in path)
        {
            pathObjects.Add(point, CreatePointObjects(prevPoint, point));
            prevPoint = point;
        }
    }

    //create the line between two point
    private LineRenderer CreateLine(Vector3 pos1, Vector3 pos2)
    {
        LineRenderer line = Instantiate(linePrefab);
        line.transform.parent = transform;
        line.SetPosition(0, pos1);
        line.SetPosition(1, pos2);
        return line;
    }

    //update the point color
    private void UpdatePoint(PathPoint point)
    {
        if (!pathObjects.ContainsKey(point)) return;
        foreach (var go in pathObjects[point])
        {
            var images = go.GetComponentsInChildren<Image>();
            foreach (var img in images)
            {
                img.color = point.previousPathPoint != null ? plannedActionColor : currentActionColor;
            }
            if(go.TryGetComponent(out LineRenderer line))
            {
                line.material.color = point.previousPathPoint != null ? plannedActionColor : currentActionColor;
            }
        }
    }

    private void UpdateSantaMarker()
    {
        //update santa
        if (selectedSantaMarker == null)
        {
            //to roundSanta
            UIPointMarker currentMarker = Instantiate(markerPrefab);
            RectTransform currentMarkerRectTransform = currentMarker.GetComponent<RectTransform>();
            currentMarker.transform.parent = transform;
            //set the position to the corresponding world position of santa
            currentMarkerRectTransform.anchoredPosition = RectTransformUtility.WorldToScreenPoint(Camera.main,
                SelectionHandler.Instance.SelectedSanta.transform.position) - canvas.sizeDelta / 2;
            selectedSantaMarker = currentMarkerRectTransform;
            currentMarker.SetColor(currentActionColor);
            currentMarker.DisableArrow();
        }
        selectedSantaMarker.anchoredPosition = RectTransformUtility.WorldToScreenPoint(Camera.main,
            SelectionHandler.Instance.SelectedSanta.transform.position) - canvas.sizeDelta / 2;
    }

    //update all line renderers
    private void UpdateLine()
    {

        foreach (var point in pathObjects)
        {
            if(point.Key.previousPathPoint == null)
            {
                foreach(var p in point.Value)
                {
                    if(p.TryGetComponent(out LineRenderer line))
                    {
                        line.SetPosition(0, SelectionHandler.Instance.SelectedSanta.transform.position);
                        break;
                    }
                }
            }
        }
        if (SelectionHandler.Instance.SelectedGift)
        {
            SelectionHandler.Instance.SelectedGift.SetLineRenderer();
        }
        if (SelectionHandler.Instance.SelectedHouse)
        {
            SelectionHandler.Instance.SelectedHouse.SetLineRenderer(true);
        }
    }

    //rotate the ui object to look at another
    private void UIYLookAt(RectTransform rect, RectTransform target)
    {
        rect.up = (rect.position - target.position).normalized;
    }

}
