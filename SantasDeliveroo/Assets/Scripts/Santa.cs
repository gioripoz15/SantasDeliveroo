using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Santa : MonoBehaviour
{
	public float speed;
    private float ProcessedSpeed
    {
        get
        {
            if (gifts.Count == 0)
            {
                return speed;
            }
            return (speed / gifts.Count);
        }
    }
    private List<PathPoint> pathPoints = new List<PathPoint>();
    public List<PathPoint> PathPoints => pathPoints;

    private List<Gift> gifts = new List<Gift>();

    [SerializeField]
    private float lookAtSpeed;

    public Action<PathPoint> pointRemovedFromList;
    public Action<PathPoint> pointAddedToList;

    private void Start()
    {
        StartCoroutine(cMove());
    }

    public void AddPathPoint(PathPoint point)
    {
        if(pathPoints.Count > 0)
        {
            pathPoints[pathPoints.Count-1].nextPathPoint = point;
            point.previousPathPoint = pathPoints[pathPoints.Count - 1];
        }
        pathPoints.Add(point);
        pointAddedToList?.Invoke(point);
    }
    public void AddNewPath(PathPoint point)
    {
        pathPoints = new List<PathPoint> {point };
        pointAddedToList?.Invoke(point);
    }

    private IEnumerator cMove()
    {
        float lerpTime = 0;
        Vector3 startPosition = transform.position;
        Vector3 target = Vector3.zero;
        float distance = 0;
        for(; ; )
        {
            //move only if have path points
            if (pathPoints.Count > 0)
            {
                //if lerp is 0 it means it just started moving and assing new start point for lerp
                if (lerpTime == 0)
                {
                    startPosition = transform.position;
                    distance = (startPosition - pathPoints[0].position).magnitude;
                    StartCoroutine(cSmoothLookAt(pathPoints[0].position));
                }
                if(target != pathPoints[0].position)
                {
                    target = pathPoints[0].position;
                    startPosition = transform.position;
                    lerpTime = 0;
                    distance = (startPosition - pathPoints[0].position).magnitude;
                    StartCoroutine(cSmoothLookAt(pathPoints[0].position));
                }
                transform.position = Vector3.Lerp(startPosition, pathPoints[0].position, lerpTime);
                lerpTime += (Time.deltaTime * speed/ distance);
                //if lerpo is 1 it means it reached the position
                if(lerpTime > 1)
                {
                    ReachedPoint(pathPoints[0]);
                    lerpTime = 0;
                }
            }
            yield return null;
        }
    }

    private IEnumerator cSmoothLookAt(Vector3 lookAtPoint)
    {
        float lerpTimer = 0;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.LookRotation((lookAtPoint- transform.position), Vector3.up);
        while (lerpTimer < 1)
        {
            lerpTimer += Time.deltaTime * lookAtSpeed;
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, lerpTimer);
            yield return null;
        }
    }

    private void ReachedPoint(PathPoint point)
    {
        PathPoint.PointType pointType = point.pointType;
        switch (point.pointType)
        {
            case PathPoint.PointType.POINT:
                break;
            case PathPoint.PointType.GIFT:
                PickUpGift(point.targettedObject);
                break;
            case PathPoint.PointType.HOUSE:
                TryDeliverToHouse(point.targettedObject);
                break;
        }
        pointRemovedFromList?.Invoke(point);
        pathPoints.Remove(point);
    }

    private void PickUpGift(GameObject giftObj)
    {
        Gift gift = giftObj.GetComponent<Gift>();
        if (gift && !gift.Owner)
        {
            gifts.Add(gift);
            gift.Owner = this;
            gift.PickUpGameobject();
        }
    }

    private void TryDeliverToHouse(GameObject house)
    {
        foreach(var g in gifts)
        {
            if(g.TargetHouse && g.TargetHouse == house)
            {
                //DoSomething
            }
        }
    }
    
}
