using System;
using UnityEngine;

[Serializable]
public class DropTable
{
    public DropItem[] items;
    
    public void SpawnItem(Monster monster)
    {
        int weightSum = 0;
        foreach (DropItem item in items)
        {
            weightSum += item.weight;
        }
        int randomNum = UnityEngine.Random.Range(0, weightSum);
        int index = 0;
        int weight = items[index].weight;
        while (randomNum > weight)
        {
            randomNum -= weight;
            index++;
            weight = items[index].weight; 
        }

        if (items[index].item == null) return;

        for (int i = 0; i < items[index].amount; i++)
        {
            float offsetX = UnityEngine.Random.Range(-2f, 2f);
            float offsetZ = UnityEngine.Random.Range(-2f, 2f);
            Vector3 spawnPos = monster.transform.position + new Vector3(offsetX, 1f, offsetZ);

            GameObject drop = GameObject.Instantiate(items[index].item.dropPrefab, spawnPos, Quaternion.identity);
            drop.GetComponent<GroundItem>().itemObject = items[index].item;
        }
    }
}
