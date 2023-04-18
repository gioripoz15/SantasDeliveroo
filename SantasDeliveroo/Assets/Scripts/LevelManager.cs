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

    public BoxCollider Jail
    {
        get
        {
            return levelGenerator.Jail;
        }
    }

    [SerializeField]
    private LevelGenerator levelGenerator;

    public List<Color> SantaColors
    {
        get
        {
            return levelGenerator.SantaColors;
        }
    }

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
    public int DeliveredGifts
    {
        get
        {
            return deliveredGifts;
        }
    }

    private float timer;
    public float Timer => timer;

    [SerializeField]
    private SceneLoader sceneLoader;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= settings.timeLimit)
        {
            GameFinished(deliveredGifts >= settings.giftToWin);
        }
    }

    public void StartLevel()
    {
        if (!levelGenerator)
        {
            levelGenerator = FindObjectOfType<LevelGenerator>();
        }
        CreateSantas();
        CreateBefanas();
        CreateLevel();
        finishCreation?.Invoke();
        hasFinishedCreation = true;
    }

    public void SetLevelSetting(LevelSettings settings)
    {
        this.settings = settings;
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
        if(deliveredGifts >= settings.giftToWin)
        {
            GameFinished(true);
        }
    }

    private void GameFinished(bool win)
    {
        StartCoroutine(cEndGame());
    }

    IEnumerator cEndGame()
    {
        //do stuff
        //playfireworks
        yield return null;
        DestroyWord();
    }

    private void DestroyWord()
    {
        sceneLoader.LoadScene("MainScene");
        sceneLoader.sceneLoaded -= StartLevel;
        sceneLoader.sceneLoaded += () => Destroy(sceneLoader.gameObject);
        Destroy(gameObject);
    }
}
