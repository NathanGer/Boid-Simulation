using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBehaviour : MonoBehaviour
{
    #region Attributs
    public FishType CurrentFishType { get; set; }
    [SerializeField] private List<FishData> CurrentShoalData;
    [SerializeField] private List<FishData> CurrentSharkData;

    [Header("Speed ratio")]
    [Range(0,10)] [SerializeField] private float MoovingFishSpeed ;
    [Range(0,10)] [SerializeField] private float RotationSpeed = 1;
    [Range(0,10)] [SerializeField] private float RandomTurnSensitivity;

    [Header("Angle mort")]
    [Range(0, 1)][SerializeField] private float DetectionAngle = 145f;

    [Header("Distance ratio")]
    [Range(0, 10)] [SerializeField] private float AlignmentDistance = 2f;
    [Range(0, 10)] [SerializeField] private float ConvergenceDistance = 3f;
    [Range(0, 10)] [SerializeField] private float RepulsionDistance = 1f;
    [Range(0, 10)] [SerializeField] private float SharkRepulsionDistance = 8f;
    [Range(0, 10)] [SerializeField] float BoundLimits = 4.5f;

    [Header("Force ratio")]
    [Range(0, 10)][SerializeField] private float AlignementForceRatio =1;
    [Range(0, 10)][SerializeField] private float ConvergenceForceRatio =1;
    [Range(0, 10)][SerializeField] private float BoundRepulsionForceRatio = 1;
    [Range(0, 10)][SerializeField] private float RepulsionForceRatio = 1;
    [Range(0, 10)][SerializeField] private float SharkRepulsionForceRatio = 1;

    Vector3 ResultanteForce;

    #endregion

    #region Initialisation
    public void InitializeFish(FishType fishType,List<FishBehaviour> fishShoal,List<FishBehaviour>  sharkList = null)
    {
        CurrentShoalData = new List<FishData>();
        foreach (var fish in fishShoal)
        {
            if (fish != this)
            {
                FishData newFishData = new FishData(fish,
                                                    Vector3.zero,
                                                    0,
                                                    0,
                                                    fish.CurrentFishType);
                CurrentShoalData.Add(newFishData);
            }
        }

        CurrentSharkData = new List<FishData>();
        if (sharkList != null)
        {
            foreach (var shark in sharkList)
            {
     
                    FishData newSharkData = new FishData(shark,
                                                        Vector3.zero,
                                                        0,
                                                        0,
                                                        shark.CurrentFishType);
                    CurrentSharkData.Add(newSharkData);  
            }
        }
    }
    #endregion

    #region DataCalcul


    private void CalculDistance(List<FishData> shoal)
    {
        for (int i = 0; i < shoal.Count; i++)
        {
            FishData data = new FishData(shoal[i]);
            data.Distance = Vector3.Distance(this.transform.position, shoal[i].FishBehaviour.transform.position);
            shoal[i] = data;
        }
    }

    private void CalculDirection(List<FishData> shoal)
    {
        for (int i = 0; i < shoal.Count; i++)
        {
            FishData data = new FishData(shoal[i]);
            data.DirectionToTheFish = (data.FishBehaviour.transform.position - this.transform.position).normalized;
            shoal[i] = data;
        }
    }    
    
    private void CalculAngle(List<FishData> shoal)
    {
        for (int i = 0; i < shoal.Count; i++)
        {
            FishData data = new FishData(shoal[i]);
            data.Angle = Vector3.Angle(this.transform.forward, data.DirectionToTheFish);
            shoal[i] = data;
        }
    }
    #endregion

    #region Movement
    private void MooveForward(float magnitude = 1)
    {
        this.transform.position += this.transform.forward * Time.deltaTime * MoovingFishSpeed * magnitude;
    }

    private void RandomOrientation()
    {
        this.transform.localRotation = Quaternion.Euler(this.transform.localRotation.eulerAngles.x,
                                                        this.transform.localRotation.eulerAngles.y + Random.Range(-RandomTurnSensitivity, RandomTurnSensitivity),
                                                        this.transform.localRotation.eulerAngles.z + Random.Range(-RandomTurnSensitivity, RandomTurnSensitivity));
    }
    #endregion

    #region ForceCalculation
    private Vector3 CalculRepulsionForce(List<FishData> shoal, float distance)
    {
        Vector3 RepulsionForceSum = Vector3.zero;
        if (shoal.Count > 0)
        {
            for (int i = 0; i < shoal.Count; i++)
            {
                if (shoal[i].Distance < distance)
                {
                    RepulsionForceSum += -shoal[i].DirectionToTheFish;
                }
            }
        }
        return RepulsionForceSum.normalized;
    }
    private Vector3 CalculAlignementForce(List<FishData> shoal, float distance)
    {
        Vector3 AlignementForceSum = Vector3.zero;
        for (int i = 0; i < shoal.Count; i++)
        {
            if (shoal[i].Distance < distance && shoal[i].Distance>RepulsionDistance)
            {
                AlignementForceSum += shoal[i].FishBehaviour.transform.forward;
            }
        }
        return AlignementForceSum.normalized;
    }
    private Vector3 CalculConvergenceForce(List<FishData> shoal, float distance)
    {
        Vector3 ConvergenceForceSum = Vector3.zero;
        for (int i = 0; i < shoal.Count; i++)
        {
            if (shoal[i].Distance < distance && shoal[i].Distance > RepulsionDistance)
            {
                ConvergenceForceSum += shoal[i].DirectionToTheFish;
            }
        }
        return ConvergenceForceSum.normalized;
    }
    private Vector3 CheckOutOfTheBox()
    {
        Vector3 force = Vector3.zero;
        if (this.transform.position.x > BoundLimits)
        {
            force = Vector3.left * BoundRepulsionForceRatio;
        }
        if (this.transform.position.x < -BoundLimits)
        {
            force = Vector3.right * BoundRepulsionForceRatio;
        }
        if (this.transform.position.z > BoundLimits)
        {
            force = Vector3.back * BoundRepulsionForceRatio;
        }
        if (this.transform.position.z < -BoundLimits)
        {
            force = Vector3.forward * BoundRepulsionForceRatio;
        }
        if (this.transform.position.y > BoundLimits)
        {
            force = Vector3.down * BoundRepulsionForceRatio;
        }
        if (this.transform.position.y < -BoundLimits)
        {
            force = Vector3.up * BoundRepulsionForceRatio;
        }
        return force;
    }

    #endregion

    private void Update()
    {
        ResultanteForce = Vector3.zero;
        if(CurrentShoalData != null)
        {
            CalculDirection(CurrentSharkData);
            CalculAngle(CurrentSharkData);
            CalculDistance(CurrentSharkData); 
            
            CalculDirection(CurrentShoalData);
            CalculAngle(CurrentShoalData);
            CalculDistance(CurrentShoalData);

            ResultanteForce += RepulsionForceRatio * CalculRepulsionForce(CurrentShoalData,RepulsionDistance);
            ResultanteForce += SharkRepulsionForceRatio * CalculRepulsionForce(CurrentSharkData, SharkRepulsionDistance);
            ResultanteForce += AlignementForceRatio * CalculAlignementForce(CurrentShoalData,AlignmentDistance);
            ResultanteForce += ConvergenceForceRatio * CalculConvergenceForce(CurrentShoalData,ConvergenceDistance);
            ResultanteForce += BoundRepulsionForceRatio * CheckOutOfTheBox();

            ApplyForce(ResultanteForce);
            MooveForward();
        }

    }

    private void ApplyForce(Vector3 ResultanteForce)
    {
        float AppliedNorme = ResultanteForce.magnitude;
        Vector3 AppliedRotation = ResultanteForce.normalized;
        float singleStep = RotationSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, ResultanteForce, singleStep, 0.0f);
        transform.LookAt(this.transform.position+ newDirection);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.05f);
        //Gizmos.DrawSphere(this.transform.position, ConvergenceDistance);        
        Gizmos.color = new Color(1f, 0, 0, 0.1f);
        Gizmos.DrawSphere(this.transform.position, RepulsionDistance);
        Gizmos.color = new Color(1.0f, 0.64f, 0.0f, 0.1f);
        //Gizmos.DrawSphere(this.transform.position, AlignmentDistance);

        for (int i = 0; i < CurrentShoalData.Count; i++)
        {
            if (CurrentShoalData[i].Angle < 145 && CurrentShoalData[i].Distance<RepulsionDistance)
            {
                Gizmos.color = Color.cyan ;
            }
            else if (CurrentShoalData[i].Angle < 145 && CurrentShoalData[i].Distance> RepulsionDistance)
            {
                //Gizmos.color = Color.green;
                Gizmos.color = Color.clear;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            if (CurrentShoalData[i].Distance <= ConvergenceDistance)
            {
                Gizmos.DrawLine(this.transform.position, this.transform.position + CurrentShoalData[i].DirectionToTheFish);
            }
            if (ResultanteForce != Vector3.zero)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(this.transform.position, this.transform.position + ResultanteForce);

            }

        }
    }
}

public struct FishData
{
    public FishBehaviour FishBehaviour { get; set; }
    public Vector3 DirectionToTheFish;
    public float Distance { get; set; }
    public float Angle;
    public FishType FishType;

    public FishData(FishBehaviour fishBehaviour,
                    Vector3 directionToTheFish,
                    float distance,
                    float angle,
                    FishType fishType)
    {
        FishBehaviour = fishBehaviour;
        DirectionToTheFish = directionToTheFish;
        Distance = distance;
        Angle = angle;
        FishType = fishType;
    }

    public FishData(FishData data)
    {
        FishBehaviour = data.FishBehaviour;
        DirectionToTheFish = data.DirectionToTheFish;
        Distance = data.Distance;
        Angle = data.Angle;
        FishType = data.FishType;
    }
}

public enum FishType
{
    Blue,
    Shark
}