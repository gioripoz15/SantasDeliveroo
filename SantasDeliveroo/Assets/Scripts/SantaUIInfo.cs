using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class SantaUIInfo : MonoBehaviour
{
    [SerializeField]
    private Button button;

    public Santa referredSanta;

    public Action<Santa> santaSelected;

    private void Start()
    {
        button.onClick.AddListener(()=>santaSelected?.Invoke(referredSanta));
    }
}
