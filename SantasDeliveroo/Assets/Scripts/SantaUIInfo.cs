using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class SantaUIInfo : MonoBehaviour
{
    [SerializeField]
    private Button button;

    [SerializeField]
    private Image sprite;

    public Santa referredSanta;

    public Action<Santa> santaSelected;

    private void Start()
    {
        SetColor();
        button.onClick.AddListener(()=>santaSelected?.Invoke(referredSanta));
    }

    public void SetColor()
    {
        if (referredSanta)
        {
            sprite.color = referredSanta.lightColor;
        }
    }
}
