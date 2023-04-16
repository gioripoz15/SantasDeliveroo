using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gioripoz;
using System;

public class SelectionHandler : Singleton<SelectionHandler>
{
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

    public void DeselectAll()
    {
        SelectedGift = null;
        SelectedSanta = null;
        Deselect?.Invoke();
    }

    public Action<Santa> SantaSelected;
    public Action<Gift> GiftSelected;

    public Action Deselect;

}
