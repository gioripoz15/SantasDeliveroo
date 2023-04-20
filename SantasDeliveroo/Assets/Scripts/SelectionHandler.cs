using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gioripoz;
using System;

public class SelectionHandler : Singleton<SelectionHandler>
{
    //handle the selctions so the user can select various things at the same like a santa and a gift
	private Santa selectedSanta;
	public Santa SelectedSanta
	{
        get
        {
			return selectedSanta;
        }
        set
        {
            if(value != null && value != selectedSanta)
            {
                selectedSanta = value;
                SantaSelected?.Invoke(value);
                return;
            }
            if(value == null)
            {
                Deselect?.Invoke();
            }
            selectedSanta = value;
        }
	}

	private Gift selectedGift;
	public Gift SelectedGift
    {
        get
        {
            return selectedGift;
        }
        set
        {
            if (value != null && value != selectedGift)
            {
                GiftSelected?.Invoke(value);
                if (selectedGift)
                {
                    selectedGift?.HighlightTargetHouse(false);
                }
                value?.HighlightTargetHouse(true);
            }
            if (value == null)
            {
                selectedGift?.HighlightTargetHouse(false);
                Deselect?.Invoke();
            }
            selectedGift = value;
        }
    }

    private House selectedHouse;
    public House SelectedHouse
    {
        get
        {
            return selectedHouse;
        }
        set
        {
            if (value != null && value != selectedHouse)
            {
                HouseSelected?.Invoke(value);
                if (selectedHouse)
                {
                    selectedHouse.SetLineRenderer(false);
                }
                value.SetLineRenderer(true);
            }
            if (value == null)
            {
                if (selectedHouse)
                {
                    selectedHouse.SetLineRenderer(false);
                }
                Deselect?.Invoke();
            }
            selectedHouse = value;
        }
    }

    public void DeselectAll()
    {
        SelectedGift = null;
        SelectedSanta = null;
        SelectedHouse = null;
        Deselect?.Invoke();
    }

    public Action<Santa> SantaSelected;
    public Action<Gift> GiftSelected;
    public Action<House> HouseSelected;

    public Action Deselect;

}
