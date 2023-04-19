using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class EndgameAnimations : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;

    public Action finishedAnimations;

    [SerializeField]
    private GameObject winScreen;

    [SerializeField]
    private GameObject looseScreen;

    [SerializeField]
    private GameObject winConfettis;

    [SerializeField]
    private float animationTime;

    [SerializeField]
    private GameObject returnToMainMenu;

    [SerializeField]
    private AudioClip winClip;
    [SerializeField]
    private AudioClip looseClip;

    public void StartAnimations(bool win)
    {
        StartCoroutine(cAnimations(win));
    }

    IEnumerator cAnimations(bool win)
    {
        Button returnButton = Instantiate(returnToMainMenu).GetComponentInChildren<Button>();
        returnButton.onClick.AddListener(()=>finishedAnimations?.Invoke());
        bool finish = false;
        returnButton.onClick.AddListener(()=>finish = true);

        if (win)
        {
            Instantiate(winScreen);
            Instantiate(winConfettis).GetComponent<Canvas>().worldCamera = Camera.main;
            source.PlayOneShot(winClip);
        }
        else
        {
            Instantiate(looseScreen);
            source.PlayOneShot(looseClip);
        }

        yield return new WaitUntil(()=>finish==true);
    }
}
