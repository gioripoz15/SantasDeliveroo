using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gioripoz;
using System;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private LevelSettings settings;
    public LevelSettings Settings => settings;

    private SantaHandler santaHandler;
    public SantaHandler SantaHandler => santaHandler;
    private BefanaHandler befanaHandler;

    [SerializeField]
    private BoxCollider jail;
    public BoxCollider Jail => jail;

    [SerializeField]
    private LevelGenerator levelGenerator;

    public PlayCamera mainCamera;

    public List<Santa> Santas
    {
        get
        {
            return santaHandler.Santas;
        }
    }

    public Action finishCreation;
    public bool hasFinishedCreation = false;

    private int deliveredGifts = 0;
    private float timer;
    public float Timer => timer;

    private void Start()
    {
        CreateSantas();
        CreateBefanas();
        CreateLevel();
        finishCreation?.Invoke();
        hasFinishedCreation = true;
    }

    private void OnDrawGizmosSelected()
    {
        if(settings)
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawCube(settings.santasSettings.spawnAreaCenter, settings.santasSettings.spawnArea);
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(settings.befanasSettings.spawnAreaCenter, settings.befanasSettings.spawnArea);
            Gizmos.color = new Color(1, 1, 1, 0.2f);
            Gizmos.DrawCube(settings.playAreaCenter, settings.playArea);
        }
    }

    private void CreateSantas()
    {
        santaHandler = (new GameObject("SanataHandler")).AddComponent<SantaHandler>();
        santaHandler.SantasSettings = settings.santasSettings;
    }
    private void CreateBefanas()
    {
        befanaHandler = (new GameObject("BefanaHandler")).AddComponent<BefanaHandler>();
        befanaHandler.BefanasSettings = settings.befanasSettings;
    }

    private void CreateLevel()
    {
        levelGenerator.GenerateLevel(settings.houseAmount,settings.giftsAmount);
    }

    public void SantaDeliveredAGift(Santa santa, Gift gift)
    {
        if(santa && gift)
        {
            deliveredGifts++;
        }
    }
}
