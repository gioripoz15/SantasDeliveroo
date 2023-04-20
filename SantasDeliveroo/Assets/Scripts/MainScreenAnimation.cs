using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreenAnimation : MonoBehaviour
{
    //i could have used some spline plugins
    [SerializeField]
    List<Transform> points = new List<Transform>();

    [SerializeField]
    float speed;

    [SerializeField]
    bool animateOnStart;

    private void Start()
    {
        if (animateOnStart) Animate();
    }

    public void Animate()
    {
        StartCoroutine(cAnimate());
    }

    IEnumerator cAnimate()
    {
        float time = 0;
        foreach (var point in points)
        {
            time = 0;
            Vector3 startPosition = transform.position;
            Quaternion startRotation = transform.rotation;
            float distance = Vector3.Distance(startPosition, point.position);
            for(; time / distance < 1; )
            {
                transform.position = Vector3.Lerp(startPosition, point.position, time/distance);
                transform.rotation =  Quaternion.Lerp(startRotation, point.rotation, time/distance);
                time += Time.deltaTime * speed;
                yield return null;
            }
        }
        yield return null;
    }
}
