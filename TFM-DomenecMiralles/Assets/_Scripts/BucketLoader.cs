using System.Collections.Generic;
using UnityEngine;

public class BucketLoader : MonoBehaviour
{
    [SerializeField] private GameObject smallBucket;
    [SerializeField] private GameObject mediumBucket;
    [SerializeField] private GameObject largeBucket;
    private void Start()
    {
        LoadRodData();
    }



    private void LoadRodData()
    {
        Unlockables unlockables = LoadSaveManager.Instance.LoadProgress();

        //Unlock the last that's true.

        var buckets = new List<(bool unlocked, GameObject bucket)>
    {
        (unlockables.buckets.smallBucket, smallBucket),
        (unlockables.buckets.mediumBucket, mediumBucket),
        (unlockables.buckets.largeBucket, largeBucket)
    };

        smallBucket.SetActive(false);
        mediumBucket.SetActive(false);
        largeBucket.SetActive(false);

        GameObject lastUnlocked = null;
        foreach (var (unlocked, bucket) in buckets)
        {
            if (unlocked)
                lastUnlocked = bucket;
        }

        if (lastUnlocked != null)
            lastUnlocked.SetActive(true);
    }

}
