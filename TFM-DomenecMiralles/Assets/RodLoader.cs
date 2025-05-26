using UnityEngine;

public class RodLoader : MonoBehaviour
{
    [SerializeField] private GameObject basicRodGO;
    [SerializeField] private GameObject basicRodLineEnd;


    [SerializeField] private GameObject betterRodGO;
    [SerializeField] private GameObject betterRodLineEnd;


    [SerializeField] private GameObject spinningRodGO;
    [SerializeField] private GameObject spinningRodLineEnd;





    private void Start()
    {
        LoadRodData();
    }



    private void LoadRodData()
    {
        Unlockables unlockables = LoadSaveManager.Instance.LoadProgress();

        basicRodGO.SetActive(unlockables.fishingRods.basicRod);
        basicRodLineEnd.SetActive(unlockables.fishingRods.basicRod);


        spinningRodGO.SetActive(unlockables.fishingRods.spinningRod);
        spinningRodLineEnd.SetActive(unlockables.fishingRods.spinningRod);

    }


}
