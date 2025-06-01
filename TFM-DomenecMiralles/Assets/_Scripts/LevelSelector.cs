using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LevelSelector : XRBaseInteractable
{
    [Header("NON XR SETTINGS")]
    [SerializeField] private GameObject modalGO;

    [Header("Images")]
    [SerializeField] private Image rockSeaImage;
    [SerializeField] private Button rockSeaButton;

    [SerializeField] private Image riverImage;
    [SerializeField] private Button riverButton;

    [SerializeField] private Image deepSeaImage;
    [SerializeField] private Button deepSeaButton;


    private void Start()
    {
        LoadLocationData();
    }

    public void LoadLocationData()
    {
        Unlockables unlocks = LoadSaveManager.Instance.LoadProgress();

        rockSeaImage.color = unlocks.locations.rockSea ? Color.white : Color.grey;
        rockSeaButton.enabled = unlocks.locations.rockSea;

        riverImage.color = unlocks.locations.river ? Color.white : Color.grey;
        riverButton.enabled = unlocks.locations.river;

        deepSeaImage.color = unlocks.locations.deepSea ? Color.white : Color.grey;
        deepSeaButton.enabled = unlocks.locations.deepSea;

    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        Debug.Log("Detected the grab");
        modalGO.SetActive(true);
    }


    //UI functions
    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }
    
}
