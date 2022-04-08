using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishFactory : MonoBehaviour
{
    [Header("Choix du nombre de poisson par banc")]
    [Range(0, 100)]
    [SerializeField] int NbrOfBlueFish;
    
    [Header("Prefab des poissons")]
    [SerializeField] GameObject BlueFish;

    private List<FishBehaviour> BlueFishList = new List<FishBehaviour>();

    private void Awake()
    {
        CreateBlueFishShoal();
    }

    private void CreateBlueFishShoal()
    {
        if (NbrOfBlueFish > 0)
        {
            for (int i = 0; i < NbrOfBlueFish; i++)
            {
                FishBehaviour newFish = InstantiateFish(BlueFish);
                BlueFishList.Add(newFish);
                newFish.InitializeFish(FishType.Blue, ref BlueFishList);                
            }
        }
    }

    private FishBehaviour InstantiateFish(GameObject FishPrefab)
    {
        GameObject newFish = Instantiate(BlueFish,
                                            new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)),
                                            Quaternion.Euler(0, Random.Range(-180,180), 0));
        return newFish.GetComponent<FishBehaviour>();
    }
}
