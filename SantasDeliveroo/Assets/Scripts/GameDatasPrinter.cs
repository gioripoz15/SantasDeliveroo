using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameDatasPrinter : MonoBehaviour
{
    [SerializeField]
    TMP_Text timer;

    [SerializeField]
    TMP_Text gift;

    private void Update()
    {
        timer.text = $"{Mathf.CeilToInt(LevelManager.Instance.Settings.timeLimit -LevelManager.Instance.Timer)}";
        gift.text = $"Delivered {LevelManager.Instance.DeliveredGifts}/{LevelManager.Instance.Settings.giftToWin}";
    }
}
