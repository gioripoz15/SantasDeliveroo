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
    private Image linePrefab;

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
        foreach (var goList in pathObjects.Values)
        {
            foreach (var go in goList)
            {
                Destroy(go);
            }
        }
        pathObjects.Clear();
        lastAddedPathPoint = new PathPoint();
        if (selectedSantaMarker)
        {
            Destroy(selectedSantaMarker.gameObject);
        }
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
        }
        pathObjects.Add(point, CreatePointObjects(previousPoint, point));
        lastAddedPathPoint = point;
        UpdatePoint(point);
    }

    private List<GameObject> CreatePointObjects(PathPoint previousPoint, PathPoint point)
    {
        List<GameObject> pointObjects = new List<GameObject>();
        UIPointMarker currentMarker = Instantiate(markerPrefab);
        RectTransform currentMarkerRectTransform = currentMarker.GetComponent<RectTransform>();
        currentMarker.transform.parent = transform;

        currentMarkerRectTransform.anchoredPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, point.position) - canvas.sizeDelta / 2;
        Image line = CreateLine(previousPoint.position, point.position);

        currentMarker.SetColor(!previousPoint.targettedObject ? currentActionColor : plannedActionColor);
        line.color = !previousPoint.targettedObject ? currentActionColor : plannedActionColor;
        pointObjects.Add(currentMarker.gameObject);
        pointObjects.Add(line.gameObject);
        if (!previousPoint.targettedObject)
        {
            //to roundSanta
            currentMarker = Instantiate(markerPrefab);
            currentMarkerRectTransform = currentMarker.GetComponent<RectTransform>();
            currentMarker.transform.parent = transform;
            currentMarkerRectTransform.anchoredPosition = RectTransformUtility.WorldToScreenPoint(Camera.main,
                SelectionHandler.Instance.SelectedSanta.transform.position) - canvas.sizeDelta / 2;
            selectedSantaMarker = currentMarkerRectTransform;
            currentMarker.SetColor(currentActionColor);
            currentMarker.DisableArrow();
            //pointObjects.Add(currentMarker.gameObject);
        }

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

    private Image CreateLine(Vector3 startPoint, Vector3 endPoint)
    {
        Image line = Instantiate(linePrefab);
        line.transform.parent = transform;
        Vector2 startPointCanvas = RectTransformUtility.WorldToScreenPoint(Camera.main, startPoint) - canvas.sizeDelta / 2;
        Vector2 endPointCanvas = RectTransformUtility.WorldToScreenPoint(Camera.main, endPoint) - canvas.sizeDelta;
        Vector2 direction = startPointCanvas - endPointCanvas;
        float distance = direction.magnitude;
        line.transform.up = direction.normalized;
        RectTransform lineRect = line.GetComponent<RectTransform>();
        lineRect.anchoredPosition = direction / 2;
        line.transform.localScale = new Vector3(line.transform.localScale.x, line.transform.localScale.y, line.transform.localScale.z);

        return line;
    }

    private void UpdatePoint(PathPoint point)
    {
        foreach (var go in pathObjects[point])
        {
            var images = go.GetComponentsInChildren<Image>();
            foreach (var img in images)
            {
                img.color = point.previousPathPoint != null ? plannedActionColor : currentActionColor;
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
            currentMarkerRectTransform.anchoredPosition = RectTransformUtility.WorldToScreenPoint(Camera.main,
                SelectionHandler.Instance.SelectedSanta.transform.position) - canvas.sizeDelta / 2;
            selectedSantaMarker = currentMarkerRectTransform;
            currentMarker.SetColor(currentActionColor);
            currentMarker.DisableArrow();
        }
        selectedSantaMarker.anchoredPosition = RectTransformUtility.WorldToScreenPoint(Camera.main,
            SelectionHandler.Instance.SelectedSanta.transform.position) - canvas.sizeDelta / 2;

        //updateOther?
    }

}
