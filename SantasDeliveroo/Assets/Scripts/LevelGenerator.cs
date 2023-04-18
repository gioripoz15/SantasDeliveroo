using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private List<House> houses = new List<House>();

    [SerializeField]
    private Gift giftPrefab;
    //randomGenerator in the city area?
    private List<Gift> gifts = new List<Gift>();

    [SerializeField]
    private Vector2Int giftSpawnArea = Vector2Int.zero;

    [SerializeField]
    private BoxCollider jail;

    public BoxCollider Jail
    {
        get { return jail; }
    }

    [SerializeField]
    List<Color> santaColors = new List<Color>();

    public List<Color> SantaColors
    {
        get { return santaColors; }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0,0,1,0.2f);
        Gizmos.DrawCube(transform.position, new Vector3(giftSpawnArea.x, 0, giftSpawnArea.y));
    }


    public void GenerateLevel(int housesAmount, int giftsAmount)
    {
        int maxGiftPerHouse = Mathf.CeilToInt((float)giftsAmount/(float)housesAmount);
        SpawnGifts(giftsAmount);
        foreach (var gift in gifts)
        {
            int randomHouseIndex = Random.Range(0, houses.Count - 1);
            gift.TargetHouse = houses[randomHouseIndex].GetComponentInChildren<Collider>().gameObject;
            houses[randomHouseIndex].assignedGifts.Add(gift);
            if (houses[randomHouseIndex].assignedGifts.Count >= maxGiftPerHouse)
            {
                houses.RemoveAt(randomHouseIndex);
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
