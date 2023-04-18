using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public List<Gift> Gifts => gifts;

    [SerializeField]
    private int maxGiftCanCarry = 3;

    [SerializeField]
    private float lookAtSpeed;

    [SerializeField]
    private Light light;

    public Color lightColor;

    public Action<PathPoint> pointRemovedFromList;
    public Action<PathPoint> pointAddedToList;

    public Action<Gift> pickedUpGift;
    public Action<Gift> deliveredGift;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip pickUpClip;
    [SerializeField]
    private AudioClip deliverClip;

    [SerializeField]
    private Transform POVtransform;
    public Transform POVTransform
    {
        get
        {
            if (!POVtransform)
            {
                POVtransform = transform;
            }
            return POVtransform;
        }
    }

    private void Start()
    {
        light.color = lightColor;
        StartCoroutine(cMove());
    }

    public void StopCorutines()
    {
        StopAllCoroutines();
    }

    public void AddPathPoint(PathPoint point)
    {
        if(pathPoints.Count > 0)
        {
            if(point.pointType == PathPoint.PointType.GIFT)
            {
                foreach(var p in pathPoints)
                {
                    if(p.targettedObject == point.targettedObject)
                    {
                        Debug.Log("Point Already Added");
                        return;
                    }
                } 
            }
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
                lerpTime += (Time.deltaTime * ProcessedSpeed / distance);

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
        if (maxGiftCanCarry <= gifts.Count) return;
        Gift gift = giftObj.GetComponent<Gift>();
        if (gift && !gift.Owner)
        {
            gifts.Add(gift);
            gift.Owner = this;
            gift.PickUpGameobject();
            pickedUpGift?.Invoke(gift);
            PlayAudio(pickUpClip);
        }
    }

    private void TryDeliverToHouse(GameObject house)
    {
        List<Gift> giftsToRemove = new List<Gift>();
        foreach(var gift in gifts)
        {
            house = house.GetComponentInChildren<Collider>().gameObject;
            if(gift.targetHouse && gift.targetHouse == house)
            {
                //DoSomething
                LevelManager.Instance.SantaDeliveredAGift(this, gift);
                gift.HighlightTargetHouse(false);
                giftsToRemove.Add(gift);
            }
        }
        foreach(var gift in giftsToRemove)
        {
            gifts.Remove(gift);
            Destroy(gift.gameObject);
            deliveredGift?.Invoke(gift);
            PlayAudio(deliverClip);
        }
    }

    private void PlayAudio(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

}
