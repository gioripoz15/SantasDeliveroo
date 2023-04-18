using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelLauncher : MonoBehaviour
{
    [SerializeField]
    private LevelManager levelManager;

    [SerializeField]
    private SceneLoader sceneLoader;

    [SerializeField]
    private string levelSceneName = "LevelScene";

    /*[SerializeField]
    private int difficulties;*/

    private int difficulty;

    [SerializeField]
    private TMP_Dropdown difficultyDropDown;

    /*[SerializeField]
    private AnimationCurve difficultyCurve;*/
    [SerializeField]
    List<LevelSettings> levelSettingsList = new List<LevelSettings>();

    [SerializeField]
    private AudioSource audioSource;

    private void Start()
    {
        difficultyDropDown.ClearOptions();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        for(int i = 0; i< levelSettingsList.Count; i++)
        {
            options.Add(new TMP_Dropdown.OptionData($"Difficulty {i+1}"));
        }
        difficultyDropDown.AddOptions(options);
        difficultyDropDown.onValueChanged.AddListener(SetDifficulty);
    }

    public void SetDifficulty(int diff)
    {
        difficulty = diff;
    }

    public void LoadLevelScene()
    {
        levelManager.SetLevelSetting(levelSettingsList[difficulty]);
        sceneLoader.sceneLoaded += levelManager.StartLevel;
        sceneLoader.LoadScene(levelSceneName);
    }

    public void LoadLevelSceneWithClip(AudioClip clip)
    {
        StartCoroutine(LoadLevelSceneAfterSound(clip));
        
    }

    IEnumerator LoadLevelSceneAfterSound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
        yield return new WaitUntil ( () => audioSource.isPlaying == false );
        LoadLevelScene();
    }
}
