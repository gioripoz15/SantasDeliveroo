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

    [SerializeField]
    private Image sprite;

    public Action<Gift> giftSelected;


    private void Start()
    {
        
        button.onClick.AddListener(() => giftSelected?.Invoke(referredGift));
    }

    public void SetColor()
    {
        if(referredGift && referredGift.Owner)
        {
            sprite.color = referredGift.Owner.lightColor;
        }
    }
}
