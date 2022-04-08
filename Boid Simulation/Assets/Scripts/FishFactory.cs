using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishFactory : MonoBehaviour
{
    [Header("Choix du nombre de poisson par banc")]
    [Range(0, 100)]
    [SerializeField] int NbrOfFish;

    private void Awake()
    {
        CreateFishShoal();
    }

    private void CreateFishShoal()
    {

    }
}
