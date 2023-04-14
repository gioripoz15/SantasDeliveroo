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
    private List<Gift> gifts = new List<Gift>();

    [SerializeField]
    private float lookAtSpeed;

    private void Start()
    {
        StartCoroutine(cMove());
    }

    private IEnumerator cMove()
    {
        float lerpTime = 0;
        Vector3 startPosition = transform.position;
        for(; ; )
        {
            //move only if have path points
            if (pathPoints.Count > 0)
            {
                //if lerp is 0 it means it just started moving and assing new start point for lerp
                if (lerpTime == 0)
                {
                    startPosition = transform.position;
                    StartCoroutine(cSmoothLookAt(pathPoints[0].position));
                }
                transform.position = Vector3.Lerp(startPosition, pathPoints[0].position, lerpTime);
                lerpTime += Time.deltaTime;
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
        Quaternion endRotation = transform.rotation * Quaternion.LookRotation((transform.position-lookAtPoint), transform.up);
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
        pathPoints.Remove(point);
    }

    private void PickUpGift(GameObject giftObj)
    {
        Gift gift = giftObj.GetComponent<Gift>();
        if (gift && !gift.Owner)
        {
            gifts.Add(gift);
            gift.Owner = this;
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
