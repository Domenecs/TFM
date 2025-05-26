using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class UIUnlockManager : MonoBehaviour
{

    [Header("FishingRods")]
    [SerializeField] private Image simpleRodOutline;
    [SerializeField] private Image betterRodOutline;
    [SerializeField] private Image spinningRodOutline;


    [SerializeField] TextMeshProUGUI moneyText;

    

    private void Start()
    {
        LoadProgress();
    }


    private void LoadProgress()
    {
        Unlockables unlockables = LoadSaveManager.Instance.LoadProgress();
        Debug.Log(Application.persistentDataPath);
        moneyText.text = unlockables.currency.ToString();

        //Rods
        simpleRodOutline.color = unlockables.fishingRods.basicRod ? Color.green : Color.red;
        betterRodOutline.color = unlockables.fishingRods.betterRod ? Color.green : Color.red;
        spinningRodOutline.color = unlockables.fishingRods.spinningRod ? Color.green : Color.red;



    }


}