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
            newBefana.transform.position = GetRandomSpawnPoint();
            newBefana.SetAreaDatas(LevelManager.Instance.Settings.playArea, LevelManager.Instance.Settings.playAreaCenter);
            befanaList.Add(newBefana);
        }
    }

    private Vector3 GetRandomSpawnPoint()
    {
        Vector3 spawnPoint = Vector3.zero;
        Vector3 areaCenter = befanasSettings.spawnAreaCenter;
        Vector3 area = befanasSettings.spawnArea;
        spawnPoint.x = Random.Range(areaCenter.x - (area.x / 2), areaCenter.x + (area.x / 2));
        spawnPoint.y = Random.Range(areaCenter.y - (area.y / 2), areaCenter.y + (area.y / 2));
        spawnPoint.z = Random.Range(areaCenter.z - (area.z / 2), areaCenter.z + (area.z / 2));
        return spawnPoint;
    }
}
