using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gioripoz;
using System;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private LevelSettings settings;
    public LevelSettings Settings => settings;

    private SantaHandler santaHandler;
    public SantaHandler SantaHandler => santaHandler;
    private BefanaHandler befanaHandler;

    [SerializeField]
    private LevelGenerator levelGenerator;

    public LevelGenerator LevelGenerator
    {
        get
        {
            if (!levelGenerator)
            {
                levelGenerator = FindObjectOfType<LevelGenerator>();
            }
            return levelGenerator;
        }
    }

    public List<Color> SantaColors
    {
        get
        {
            return LevelGenerator.SantaColors;
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
    public float Timer
    {
        get
        {
            if (timer/settings.timeLimit % 3 == 0)
            {
                audioSource?.PlayOneShot(audioTimer);
            }
            return timer;
        }
    }

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip audioTimer;

    [SerializeField]
    private EndgameAnimations endgameAnimationsPrefab;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= settings.timeLimit)
        {
            GameFinished(deliveredGifts >= settings.giftToWin);
        }
    }

    public void StartLevel()
    {
        if (hasFinishedCreation) return;//to prevent multiple spawns
        CreateSantas();
        CreateBefanas();
        CreateLevel();
        finishCreation?.Invoke();
        hasFinishedCreation = true;
    }


    public void SceneLoaded(Scene scene)
    {
        //laod the level only if the scene loaded is the levelscene
        if (scene.name == "LevelScene")
        {
            StartLevel();
        }
    }

    public void SetLevelSetting(LevelSettings settings)
    {
        this.settings = settings;
    }

    private void OnDrawGizmosSelected()
    {
        if (settings)
        {
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
        LevelGenerator.GenerateLevel(settings.houseAmount, settings.giftsAmount);
    }

    public void SantaDeliveredAGift(Santa santa, Gift gift)
    {
        if (santa && gift)
        {
            deliveredGifts++;
        }
        if (deliveredGifts >= settings.giftToWin)
        {
            GameFinished(true);
        }
    }

    public void SantaRemoved(Santa santa)
    {
        if (Santas.Count == 0)
        {
            GameFinished(false);
        }
    }

    //handle the end game
    private void GameFinished(bool win)
    {
        StartCoroutine(cEndGame(win));
    }

    IEnumerator cEndGame(bool win)
    {
        //do stuff
        //playfireworks
        EndgameAnimations animations = Instantiate(endgameAnimationsPrefab);
        animations.StartAnimations(win);
        bool finished = false;
        animations.finishedAnimations += () => finished = true;
        yield return new WaitUntil(() => finished);
        DestroyWord();
    }

    private void DestroyWord()
    {
        SceneManager.LoadScene("MainScene");
        SceneManager.sceneLoaded -= (Scene scene, LoadSceneMode mode) => Destroy(gameObject);
        Destroy(gameObject);
    }
}
