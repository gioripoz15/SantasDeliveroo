using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("")]
public class SantaHandler : MonoBehaviour
{
    private LevelSettings.LevelEntitySettings santasSettings;
	public LevelSettings.LevelEntitySettings SantasSettings
    {
        get
        {
            return santasSettings;
        }
        set
        {
            santasSettings = value;
            GenerateSantas();
        }
    }

	private List<Santa> santas = new List<Santa>();

    private void GenerateSantas()
    {
        for (int i = 0; i< santasSettings.amount; i++)
        {
            Santa newSanta = (Instantiate(santasSettings.prefab)).GetComponent<Santa>();
            newSanta.speed = santasSettings.speed;
            newSanta.transform.position = GetRandomSpawnPoint();
            santas.Add(newSanta);
        }
    }

    private Vector3 GetRandomSpawnPoint()
    {
        Vector3 spawnPoint = Vector3.zero;
        Vector3 areaCenter = santasSettings.spawnAreaCenter;
        Vector3 area = santasSettings.spawnArea;
        spawnPoint.x = Random.Range(areaCenter.x - (area.x / 2), areaCenter.x + (area.x / 2));
        spawnPoint.y = Random.Range(areaCenter.y - (area.y / 2), areaCenter.y + (area.y / 2));
        spawnPoint.z = Random.Range(areaCenter.z - (area.z / 2), areaCenter.z + (area.z / 2));

        return spawnPoint;
    }
}
