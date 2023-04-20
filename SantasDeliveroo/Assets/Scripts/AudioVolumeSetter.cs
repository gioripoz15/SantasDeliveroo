using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioVolumeSetter : MonoBehaviour
{
    [SerializeField]
    private Slider volumeSlider;

    [SerializeField]
    private GameObject iconVolume;
    [SerializeField]
    private GameObject iconVolumeOff;

    private void Start()
    {
        volumeSlider.value = AudioListener.volume;
    }

    public void ChangeVolume()
    {
        float value = volumeSlider.value;
        SetVolume(value);
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        iconVolumeOff.SetActive(value <= 0);
        iconVolume.SetActive(value > 0);
    }
}
