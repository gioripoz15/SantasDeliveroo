using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("")]
public class BefanaHandler : MonoBehaviour
{
    private LevelSettings.LevelEntitySettings befanasSettings;
    public LevelSettings.LevelEntitySettings BefanasSettings
    {
        get
        {
            return befanasSettings;
        }
        set
        {
            befanasSettings = value;
            GenerateBefanas();
        }
    }
    private List<Befana> befanaList = new List<Befana>();

    private void GenerateBefanas()
    {
        for (int i = 0; i < befanasSettings.amount; i++)
        {
            Befana newBefana = (Instantiate(befanasSettings.prefab)).GetComponent<Befana>();
            newBefana.speed = befanasSettings.speed;
            var spawnPoints = LevelManager.Instance.LevelGenerator.BefanasSpawn.points;
            newBefana.transform.position = spawnPoints[i % spawnPoints.Count].position;
            newBefana.transform.parent = transform;
            newBefana.SetAreaDatas(LevelManager.Instance.Settings.playArea, LevelManager.Instance.Settings.playAreaCenter);
            befanaList.Add(newBefana);
        }
    }
}
