using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "LevelSettings", menuName = "Deliveroo/LevelSettings", order = 1)]
public class LevelSettings : ScriptableObject
{
	public int giftsAmount;
    public int houseAmount;
    public LevelEntitySettings santasSettings;
    public LevelEntitySettings befanasSettings;
    public Vector3 playArea;
    public Vector3 playAreaCenter;

    [Serializable]
    public struct LevelEntitySettings
    {
        public GameObject prefab;
        public int amount;
        public float speed;
        public Vector3 spawnArea;
        public Vector3 spawnAreaCenter;
    }
}
