using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private List<House> houses = new List<House>();

    [SerializeField]
    private Gift giftPrefab;
    private List<Gift> gifts = new List<Gift>();

    [SerializeField]
    private Vector2Int giftSpawnArea = Vector2Int.zero;

    [SerializeField]
    List<Color> santaColors = new List<Color>();

    public List<Color> SantaColors
    {
        get { return santaColors; }
    }

    [SerializeField]
    AreaWithPoints befanasSpawn;
    public AreaWithPoints BefanasSpawn => befanasSpawn;

    [SerializeField]
    AreaWithPoints santasSpawn;
    public AreaWithPoints SantasSpawn => santasSpawn;


    [SerializeField]
    AreaWithPoints jailSpawns;
    public AreaWithPoints JailSpawns => jailSpawns;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0,0,1,0.2f);
        Gizmos.DrawCube(transform.position, new Vector3(giftSpawnArea.x, 0, giftSpawnArea.y));
    }


    public void GenerateLevel(int housesAmount, int giftsAmount)
    {
        int maxGiftPerHouse = Mathf.CeilToInt((float)giftsAmount/(float)housesAmount);
        SpawnGifts(giftsAmount);
        List<House> avaiableHouses = new List<House>(LevelManager.Instance.Settings.houseAmount);
        for (int i = 0; i< LevelManager.Instance.Settings.houseAmount; i++)
        {
            int randomHouseIndex = Random.Range(0, houses.Count - 1);
            avaiableHouses.Add(houses[randomHouseIndex]);
            houses.Remove(houses[randomHouseIndex]);

        }
        foreach (var gift in gifts)
        {
            //rework the way it distributes the gift?
            int randomHouseIndex = Random.Range(0, avaiableHouses.Count - 1);
            gift.targetHouseGameObject = avaiableHouses[randomHouseIndex].GetComponentInChildren<Collider>().gameObject;
            avaiableHouses[randomHouseIndex].assignedGifts.Add(gift);
            avaiableHouses[randomHouseIndex].PermanentHighlight.RecurseHighlight();
            if (avaiableHouses[randomHouseIndex].assignedGifts.Count >= maxGiftPerHouse)
            {
                avaiableHouses.RemoveAt(randomHouseIndex);
            }
        }
    }

    private void SetupHouses(int amount)
    {
        for(int i = 0; i< amount; i++)
        {
            houses.RemoveAt(Random.Range(0,houses.Count-1));
        }
    }

    private void SpawnGifts(int amount)
    {
        gifts.Clear();
        GameObject giftContainer = new GameObject("GiftContainer");
        for (int i = 0; i < amount; i++)
        {
            Gift currentGift = Instantiate(giftPrefab);
            currentGift.transform.position = new Vector3(
                Random.Range(-giftSpawnArea.x/2 + transform.position.x, giftSpawnArea.x/2 + transform.position.x),
                transform.position.y,
                Random.Range(-giftSpawnArea.y/2 + transform.position.z, giftSpawnArea.y/2 + transform.position.z)
                );
            currentGift.transform.parent = giftContainer.transform;
            currentGift.gameObject.SetActive(true);//idk why is disabled when spawned
            gifts.Add(currentGift);
            
        }
    }
}
