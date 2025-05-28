using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FishCubeManager : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI ValueText;
    [SerializeField] private TextMeshProUGUI FishCapacityText;

    [Header("Bucket settings")]
    [SerializeField] private int fishCapacity;
    private int _bucketScore;


    // ------------ COLLISION FUNCTIONS -----------//
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fish"))
        {
            FishFunctions refScript = other.gameObject.GetComponent<FishFunctions>();
            if (refScript != null)
            {
                if(fishCapacity > 0)
                {
                    fishCapacity--;
                    _bucketScore += refScript.GetFishValue();
                    UpdateUI();
                }

            }
            else
            {
                Debug.LogWarning("Couldn't find the fishFunctions in the trigger.");
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Fish"))
        {
            FishFunctions refScript = other.gameObject.GetComponent<FishFunctions>();
            if (refScript != null)
            {
                    fishCapacity++;
                    _bucketScore -= refScript.GetFishValue();    
                    UpdateUI();
            }
            else
            {
                Debug.LogWarning("Couldn't find the fishFunctions in the trigger.");
            }

        }
    }

    //UI Functions

    private void UpdateUI()
    {
        ValueText.text = _bucketScore.ToString();
        FishCapacityText.text = fishCapacity.ToString();
    }



    public void FinishSession()
    {
        Unlockables unlocks =  LoadSaveManager.Instance.LoadProgress();
        unlocks.currency += _bucketScore;
        LoadSaveManager.Instance.SaveProgress(unlocks);
        SceneManager.LoadScene("Lobby");
    }
}
