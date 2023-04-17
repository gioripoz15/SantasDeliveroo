using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
	public List<Santa> Santas
    {
        get
        {
            if (santas != null && santas.Count > 0)
            {
                List<int> indexToRemove = new List<int>();
                for(int i=0; i< santas.Count-1; i++)
                {
                    if (santas[i] == null)
                    {
                        indexToRemove.Add(i);
                    }
                }
                foreach(int i in indexToRemove)
                {
                    santas.RemoveAt(i);
                }
            }
            return santas;
        }
    }

    public System.Action<Santa> RemovedSanta;

    public void RemoveSanta(Santa santa)
    {
        Santas.Remove(santa);
        RemovedSanta?.Invoke(santa);
    }
    
    private void GenerateSantas()
    {
        for (int i = 0; i< santasSettings.amount; i++)
        {
            Santa newSanta = (Instantiate(santasSettings.prefab)).GetComponent<Santa>();
            newSanta.speed = santasSettings.speed;
            newSanta.transform.position = GetRandomSpawnPoint();
            newSanta.transform.parent = transform;
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
