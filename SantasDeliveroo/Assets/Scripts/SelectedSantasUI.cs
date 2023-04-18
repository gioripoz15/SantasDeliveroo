using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedSantasUI : MonoBehaviour
{
    [SerializeField]
    private Transform santaUIContainer;
    [SerializeField]
    private Transform giftUIContainer;
    [SerializeField]
    private SantaUIInfo santaUIObject;
    [SerializeField]
    private GiftUIInfo giftUIObject;


    private void Start()
    {
        if (!LevelManager.Instance.hasFinishedCreation)
        {
            LevelManager.Instance.finishCreation += () =>
            {
                Initialize();
            };
        }
        else
        {
            Initialize();
        }
        
    }

    private void Initialize()
    {
        UpdateSantasList();
        SubscribeToAllSantaGiftChange();
        LevelManager.Instance.SantaHandler.RemovedSanta += (Santa santa) => UpdateSantasList();
        SelectionHandler.Instance.SantaSelected += (Santa santa) => UpdateSantasList();
    }

    private void SubscribeToAllSantaGiftChange()
    {
        List<Santa> santaList = LevelManager.Instance.SantaHandler.Santas;
        foreach(var santa in santaList)
        {
            santa.pickedUpGift += (Gift gift) => UpdateSantasList();
            santa.deliveredGift += (Gift gift) => UpdateSantasList();
        }

    }

    private void UpdateSantasList()
    {
        if (santaUIContainer.childCount > 0)
        {
            for (int i = 0; i < santaUIContainer.childCount; i++)
            {
                Destroy(santaUIContainer.GetChild(i).gameObject);
            }
        }
        ClearGiftList();
        List<Santa> santas = LevelManager.Instance.Santas;
        foreach (Santa santa in santas)
        {
            SantaUIInfo currentSantaInfo = Instantiate(santaUIObject);
            currentSantaInfo.transform.parent = santaUIContainer;
            currentSantaInfo.referredSanta = santa;
            currentSantaInfo.santaSelected += SelectSanta;
            currentSantaInfo.SetColor();
        }
        SelectSanta(SelectionHandler.Instance.SelectedSanta);
    }

    private void SelectSanta(Santa santa)
    {
        if (!santa) return;
        SelectionHandler.Instance.SelectedSanta = santa;
        SelectionHandler.Instance.SelectedSanta = santa;
        ShowGiftsInfos(santa);
    }

    private void ClearGiftList()
    {
        if (giftUIContainer.childCount > 0)
        {
            for (int i = 0; i < giftUIContainer.childCount; i++)
            {
                Destroy(giftUIContainer.GetChild(i).gameObject);
            }
        }
    }

    private void ShowGiftsInfos(Santa santa)
    {
        if (!santa) return;
        ClearGiftList();
        foreach (var gift in santa.Gifts)
        {

            GiftUIInfo currentGiftInfo = Instantiate(giftUIObject);
            currentGiftInfo.transform.parent = giftUIContainer;
            currentGiftInfo.referredGift = gift;
            currentGiftInfo.giftSelected += SelectGift;
            currentGiftInfo.SetColor();
        }
    }

    private void SelectGift(Gift gift)
    {
        SelectionHandler.Instance.SelectedGift = gift;
    }

}
