using UnityEngine;

public class RodLoader : MonoBehaviour
{
    [SerializeField] private GameObject basicRodGO;


    [SerializeField] private GameObject betterRodGO;


    [SerializeField] private GameObject spinningRodGO;





    private void Start()
    {
        LoadRodData();
    }



    private void LoadRodData()
    {
        Unlockables unlockables = LoadSaveManager.Instance.LoadProgress();

        basicRodGO.SetActive(unlockables.fishingRods.basicRod);

        betterRodGO.SetActive(unlockables.fishingRods.betterRod);


        spinningRodGO.SetActive(unlockables.fishingRods.spinningRod);

    }


}
