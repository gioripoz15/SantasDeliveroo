using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader: MonoBehaviour
{
    public Action sceneLoaded;
    private void Start()
    {
        DontDestroyOnLoad(this);
    }
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => sceneLoaded?.Invoke();
    }
}
