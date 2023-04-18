using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Befana : MonoBehaviour
{
    public float speed;

    private Vector3 patrollingArea = Vector3.zero;
    private Vector3 patrollingAreaCenter = Vector3.zero;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip caughtClip;

    private void Start()
    {
        StartCoroutine(cPatrol());

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //startFollow
            StopAllCoroutines();
            StartCoroutine(cFollow(other.transform));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(cPatrol());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            //kidnap the poor santa
            Santa kidnappedSanta = collision.gameObject.GetComponent<Santa>();
            KidnapSanta(kidnappedSanta);
            if(kidnappedSanta == SelectionHandler.Instance.SelectedSanta)
            {
                SelectionHandler.Instance.SelectedSanta = null;
            }
            Destroy(kidnappedSanta);
            var jail = LevelManager.Instance.Jail;
            transform.position = GetPointInArea(jail.center + jail.transform.position, jail.size);
            StopAllCoroutines();
            PlayCaughtAudio(caughtClip);
            Destroy(this);
        }
    }

    public void SetAreaDatas(Vector3 area, Vector3 center)
    {
        patrollingArea = area;
        patrollingAreaCenter = center;
    }


    private void KidnapSanta(Santa santa)
    {
        santa.StopCorutines();
        var jail = LevelManager.Instance.Jail;
        santa.transform.position = GetPointInArea(jail.center + jail.transform.position, jail.size);
        santa.transform.eulerAngles = new Vector3(0, santa.transform.eulerAngles.y, 0);
        LevelManager.Instance.SantaHandler.RemoveSanta(santa);
    }

    private IEnumerator cFollow(Transform santa)
    {
        for (; ; )
        {
            transform.position += (santa.position -transform.position).normalized * speed * Time.deltaTime;
            transform.LookAt(santa);
            yield return null;
        }
    }

    private IEnumerator cPatrol()
    {
        float lerpTime = 0;
        Vector3 startPosition = transform.position;
        Vector3 target = GetPointInArea(patrollingAreaCenter, patrollingArea);
        transform.LookAt(target);
        float distance = (startPosition - target).magnitude;
        for (; ; )
        {
            transform.position = Vector3.Lerp(startPosition, target, lerpTime);
            lerpTime += (Time.deltaTime * speed / distance);

            //if lerpo is 1 it means it reached the position
            if (lerpTime > 1)
            {
                startPosition = target;
                target = GetPointInArea(patrollingAreaCenter, patrollingArea);
                distance = (startPosition - target).magnitude;
                transform.LookAt(target);
                lerpTime = 0;
            }
            yield return null;
        }
    }

    private Vector3 GetPointInArea(Vector3 center, Vector3 area)
    {
        Vector3 point = Vector3.zero;
        point.x = Random.Range(center.x - (area.x / 2), center.x + (area.x / 2));
        point.y = Random.Range(center.y - (area.y / 2), center.y + (area.y / 2));
        point.z = Random.Range(center.z - (area.z / 2), center.z + (area.z / 2));
        return point;
    }

    private void PlayCaughtAudio(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
