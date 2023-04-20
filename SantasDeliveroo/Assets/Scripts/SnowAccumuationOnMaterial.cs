using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowAccumuationOnMaterial : MonoBehaviour
{
    [SerializeField]
    MeshRenderer renderer;

    [SerializeField]
    float snowAcculuationSpeed;

    private void Update()
    {
        Color currentColor = renderer.material.color;
        currentColor += Color.white * Time.deltaTime *snowAcculuationSpeed;
        currentColor.r = currentColor.r >1? 1: currentColor.r;
        currentColor.g = currentColor.g >1? 1: currentColor.g;
        currentColor.b = currentColor.b >1? 1: currentColor.b;
        renderer.material.color = currentColor;
    }
}
