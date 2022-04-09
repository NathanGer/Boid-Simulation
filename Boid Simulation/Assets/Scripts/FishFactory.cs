using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishFactory : MonoBehaviour
{
    [Header("Choix du nombre de poisson par banc")]
    [Range(0, 1000)]
    [SerializeField] int NbrOfBlueFish;
    [Range(0, 10)]
    [SerializeField] int NbrOfShark;
    
    [Header("Prefab des poissons")]
    [SerializeField] GameObject BlueFish;
    [SerializeField] GameObject Shark;

    private List<FishBehaviour> BlueFishList = new List<FishBehaviour>();
    private List<FishBehaviour> SharkList = new List<FishBehaviour>();

    private void Awake()
    {
        CreateSharkShoal();
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
                newFish.CurrentFishType = FishType.Blue;
            }
        }
        InitializeFish(BlueFishList,SharkList);
    }    
    private void CreateSharkShoal()
    {
        if (NbrOfShark > 0)
        {
            for (int i = 0; i < NbrOfShark; i++)
            {
                FishBehaviour newFish = InstantiateFish(Shark);
                SharkList.Add(newFish);
                newFish.CurrentFishType = FishType.Shark;
            }
        }
        InitializeFish(SharkList);
    }

    private void InitializeFish(List<FishBehaviour>  fishList, List<FishBehaviour>  sharkList = null)
    {
        foreach (var fish in fishList)
        {
            if(sharkList != null)
            {
                fish.InitializeFish(FishType.Blue, fishList, sharkList);
            }
            else
            {
                fish.InitializeFish(FishType.Blue, fishList);
            }
        }
    }
    private FishBehaviour InstantiateFish(GameObject FishPrefab)
    {
        GameObject newFish = Instantiate(FishPrefab,
                                            new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5)),
                                            Quaternion.Euler(0, Random.Range(-180,180), Random.Range(-180, 180)));
        return newFish.GetComponent<FishBehaviour>();
    }
}
