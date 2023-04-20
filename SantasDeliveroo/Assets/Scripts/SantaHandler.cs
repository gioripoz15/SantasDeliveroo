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
        if(Santas.Count == 0)
        {
            LevelManager.Instance.SantaRemoved(santa);
        }
    }
    
    private void GenerateSantas()
    {
        for (int i = 0; i< santasSettings.amount; i++)
        {
            Santa newSanta = (Instantiate(santasSettings.prefab)).GetComponent<Santa>();
            newSanta.speed = santasSettings.speed;
            var spawnPoints = LevelManager.Instance.LevelGenerator.SantasSpawn.points;
            newSanta.transform.position = spawnPoints[i%spawnPoints.Count].position;
            newSanta.transform.parent = transform;
            newSanta.lightColor = LevelManager.Instance.SantaColors[i% LevelManager.Instance.SantaColors.Count];
            santas.Add(newSanta);
        }
    }
}
