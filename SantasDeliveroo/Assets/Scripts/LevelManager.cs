using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gioripoz;
public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private LevelSettings settings;

    private SantaHandler santaHandler;
    private BefanaHandler befanaHandler;

    private void Start()
    {
        CreateSantas();
        CreateBefanas();
    }

    private void OnDrawGizmos()
    {
        if(settings)
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawCube(settings.santasSettings.spawnAreaCenter, settings.santasSettings.spawnArea);
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(settings.befanasSettings.spawnAreaCenter, settings.befanasSettings.spawnArea);
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
}
