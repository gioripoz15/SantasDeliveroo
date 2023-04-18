using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    [SerializeField]
    private float strenght;

    [SerializeField]
    private float time;

    [SerializeField]
    private Vector3 localAxis;

    [SerializeField]
    private AnimationCurve animationCurve;

    private void Start()
    {
        localAxis = localAxis.normalized;
        StartCoroutine(cFloating());
    }

    IEnumerator cFloating()
    {
        float timer = 0;
        int invert = 1;
        for (; ; )
        {
            transform.localPosition += localAxis * strenght * invert * Time.deltaTime * animationCurve.Evaluate(timer/time);
            timer += Time.deltaTime;
            if (timer > time)
            {
                timer = 0;
                invert = -invert;
            }
            yield return null;
        }
    }
}
