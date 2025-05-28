using System.Collections.Generic;
using UnityEngine;
using static SimpleRodManager;

[CreateAssetMenu(fileName = "FishData", menuName = "Scriptable Objects/FishData")]
public class FishData : ScriptableObject
{
    [Header("Value")]
    public int fishValue;

    public string fishName;
    public GameObject prefab;

    [System.Serializable]
    public class BaitProbability
    {
        public FishingData.BaitType baitType;
        [Range(0, 100)]
        public float probability;
    }

    public List<BaitProbability> baitChances;

}
