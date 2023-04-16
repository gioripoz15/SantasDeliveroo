using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftUIInfo : MonoBehaviour
{
    [SerializeField]
    private Button button;

    public Gift referredGift;

    public Action<Gift> giftSelected;

    private void Start()
    {
        button.onClick.AddListener(() => giftSelected?.Invoke(referredGift));
    }
}
