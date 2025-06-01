using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeSceneTransition : MonoBehaviour
{
    [SerializeField] private GameObject smallBucket;
    [SerializeField] private GameObject mediumBucket;
    [SerializeField] private GameObject largeBucket;

    private int currencytoAdd;


    public void FinishRun()
    {
        if (smallBucket.activeInHierarchy)
        {
            currencytoAdd = smallBucket.GetComponentInChildren<FishCubeManager>().GetBucketValue();
            Debug.Log("BucketValue" + currencytoAdd);
        }
        if (mediumBucket.activeInHierarchy)
        {
            currencytoAdd = mediumBucket.GetComponentInChildren<FishCubeManager>().GetBucketValue();
            Debug.Log("BucketValue" + currencytoAdd);
        }
        if (largeBucket.activeInHierarchy)
        {
            currencytoAdd = largeBucket.GetComponentInChildren<FishCubeManager>().GetBucketValue();
            Debug.Log("BucketValue" + currencytoAdd);
        }


        Unlockables unlocks = LoadSaveManager.Instance.LoadProgress();
        unlocks.currency += currencytoAdd;

        LoadSaveManager.Instance.SaveProgress(unlocks);


        SceneManager.LoadScene("Lobby");

    }




}
