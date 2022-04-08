using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBehaviour : MonoBehaviour
{
    #region Attributs
    private FishType CurrentFishType;
    [SerializeField] private List<FishBehaviour> CurrentShoal;
    
    [Range(0,10)]
    [SerializeField] private float CurrentFishSpeed;
    
    [Range(0,10)]
    [SerializeField] private float RandomTurnSensitivity;
    #endregion

    #region Initialisation
    public void InitializeFish(FishType fishType,ref List<FishBehaviour> fishShoal)
    {
        CurrentShoal = fishShoal;
        CurrentFishType = fishType;
    }
    #endregion

    #region Movement
    private void RandomOrientation()
    {
        this.transform.localRotation = Quaternion.Euler(this.transform.localRotation.eulerAngles.x,
                                                        this.transform.localRotation.eulerAngles.y+Random.Range(-RandomTurnSensitivity,RandomTurnSensitivity),
                                                        this.transform.localRotation.eulerAngles.z);
    }

    private void MooveForward()
    {
        this.transform.position += this.transform.right * Time.deltaTime * CurrentFishSpeed;
    }
    #endregion

    private void Update()
    {
        RandomOrientation();
        MooveForward();
        CheckOutOfTheBox();
    }

    private void CheckOutOfTheBox()
    {
        if(this.transform.position.x > 5)
        {
            this.transform.position -= Vector3.right * 10;
        }
        if(this.transform.position.x < -5)
        {
            this.transform.position += Vector3.right * 10;
        }
        if(this.transform.position.z > 5)
        {
            this.transform.position -= Vector3.forward * 10;
        }        
        if(this.transform.position.z < -5)
        {
            this.transform.position += Vector3.forward * 10;
        }
        if(this.transform.position.y > 5)
        {
            this.transform.position -= Vector3.up * 10;
        }        
        if(this.transform.position.y < -5)
        {
            this.transform.position += Vector3.up * 10;
        }

    }
}

public enum FishType
{
    Blue,
    Shark
}