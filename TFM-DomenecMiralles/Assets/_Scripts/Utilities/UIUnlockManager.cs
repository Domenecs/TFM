using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;



public class UIUnlockManager : MonoBehaviour
{
    //PAGE 1
    [Header("FishingRods")]
    [SerializeField] private Image simpleRodOutline;

    [SerializeField] private Image betterRodOutline;
    [SerializeField] private Button betterRodButton;

    [SerializeField] private Image spinningRodOutline;
    [SerializeField] private Button spinningRodButton;

    [Header("FishingRodsPrices")]
    [SerializeField] private int betterRodPrice;
    [SerializeField] private int spinningRodPrice;

    // PAGE 2
    [Header("Hooks")]
    [SerializeField] private Image smallHookOutline;

    [SerializeField] private Image bigHookOutline;
    [SerializeField] private Button bigHookButton;

    [SerializeField] private Image tripleHookOutline;
    [SerializeField] private Button tripleHookButton;

    [Header("Hooks Prices")]
    [SerializeField] private int bigHookPrice;
    [SerializeField] private int TripleHookPrice;

    [Header("Buckets")]
    [SerializeField] private Image smallBucketOutline;

    [SerializeField] private Image mediumBucketOutline;
    [SerializeField] private Button mediumBucketButton;

    [SerializeField] private Image bigBucketOutline;
    [SerializeField] private Button bigBucketButton;

    [Header("Bucket Prices")]
    [SerializeField] private int mediumBucketPrice;
    [SerializeField] private int bigBucketPrice;


    [Header("Baits")]
    [SerializeField] private Image gachaOutline;

    [SerializeField] private Image mandarinaOutline;
    [SerializeField] private Button mandarinaButton;

    [SerializeField] private Image wormOutline;
    [SerializeField] private Button wormButton;

    [Header("Baits prices")]
    [SerializeField] private int mandarinaPrice;
    [SerializeField] private int wormPrice;



    [Header("Locations")]
    [SerializeField] private Image rockSeaOutline;

    [SerializeField] private Image riverLocationOutline;
    [SerializeField] private Button riverLocationButton;

    [SerializeField] private Image seaLocationOutline;
    [SerializeField] private Button seaLocationButton;


    [Header("Location prices")]
    [SerializeField] private int riverLocationPrice;
    [SerializeField] private int seaLocationPrice;







    [SerializeField] TextMeshProUGUI moneyText;

    [Header("Modal")]
    [SerializeField] private GameObject modal;


    [Header("TabPages")]
    [SerializeField] private List<GameObject> PageParents;


    private int _money;


    [SerializeField] private LevelSelector _levelSelector;

    private void Start()
    {
        LoadProgress();
    }


    private void LoadProgress()
    {
        Unlockables unlockables = LoadSaveManager.Instance.LoadProgress();
        _money = unlockables.currency;
        moneyText.text = unlockables.currency.ToString();
        LoadRods(unlockables);
        LoadBuckets(unlockables);
        LoadHooks(unlockables); 
        LoadBaits(unlockables);
        LoadLocations(unlockables);
    }


    private void LoadRods(Unlockables unlocks)
    {
        simpleRodOutline.color = unlocks.fishingRods.basicRod ? Color.green : Color.red;

        betterRodOutline.color = unlocks.fishingRods.betterRod ? Color.green : Color.red;
        betterRodButton.enabled = unlocks.fishingRods.betterRod ? false : true;
        spinningRodOutline.color = unlocks.fishingRods.spinningRod ? Color.green : Color.red;
        spinningRodButton.enabled = unlocks.fishingRods.spinningRod ? false : true;
    }
    public void UnlockRod(string rodName)
    {
        Unlockables unlockables = LoadSaveManager.Instance.LoadProgress();

        switch (rodName)
        {
            case "better":
                if(_money >= betterRodPrice) { 
                unlockables.fishingRods.betterRod = true;
                _money -= betterRodPrice;
                    unlockables.currency = _money;
                }
                else
                {
                    ToggleModalVisibility();
                    return;
                }

                break;
            case "spinning":
                if (_money >= spinningRodPrice)
                {
                    Debug.Log("Attemtping");

                    unlockables.fishingRods.spinningRod = true;
                    _money -= spinningRodPrice;
                    unlockables.currency = _money;
                }
                else
                {
                    ToggleModalVisibility();
                    return;
                }
                break;
            default: break;

        }

        LoadSaveManager.Instance.SaveProgress(unlockables);
        LoadProgress(); //Refresh the UI
    }


    private void LoadHooks(Unlockables unlocks)
    {
        smallHookOutline.color = unlocks.hooks.smallhook ? Color.green : Color.red;

        bigHookOutline.color = unlocks.hooks.bighook ? Color.green : Color.red;
        bigHookButton.enabled = unlocks.hooks.bighook ? false : true;

        tripleHookOutline.color = unlocks.hooks.triplehook ? Color.green : Color.red;
        tripleHookButton.enabled = unlocks.hooks.triplehook ? false : true;
    }
    public void UnlockHook(string hookname)
    {
        Unlockables unlockables = LoadSaveManager.Instance.LoadProgress();

        switch (hookname)
        {
            case "bighook":
                if (_money >= bigHookPrice)
                {
                    unlockables.hooks.bighook = true;
                    _money-= bigHookPrice;
                    unlockables.currency = _money;
                }
                else
                {
                    ToggleModalVisibility();
                    return;
                }

                break;
            case "triplehook":
                if (_money >= TripleHookPrice)
                {
                    unlockables.hooks.triplehook = true;
                    _money-= TripleHookPrice;
                    unlockables.currency = _money;

                }
                else
                {
                    ToggleModalVisibility();
                    return;
                }
                break;
            default: break;

        }

        LoadSaveManager.Instance.SaveProgress(unlockables);
        LoadProgress(); //Refresh the UI
    }

    public void UnlockBucket(string bucketname)
    {
        Unlockables unlockables = LoadSaveManager.Instance.LoadProgress();

        switch (bucketname)
        {
            case "mediumbucket":
                if (_money >= mediumBucketPrice)
                {
                    unlockables.buckets.mediumBucket = true;
                    _money-= mediumBucketPrice;
                    unlockables.currency = _money;
                }
                else
                {
                    ToggleModalVisibility();
                    return;
                }

                break;
            case "bigbucket":
                if (_money >= bigBucketPrice)
                {
                    unlockables.buckets.largeBucket = true;
                    _money-= bigBucketPrice;
                    unlockables.currency = _money;
                }
                else
                {
                    ToggleModalVisibility();
                    return;
                }
                break;
            default: break;

        }

        LoadSaveManager.Instance.SaveProgress(unlockables);
        LoadProgress(); //Refresh the UI
    }
    private void LoadBuckets(Unlockables unlocks)
    {
        smallBucketOutline.color = unlocks.buckets.smallBucket ? Color.green : Color.red;

        mediumBucketOutline.color = unlocks.buckets.mediumBucket ? Color.green : Color.red;
        mediumBucketButton.enabled = unlocks.buckets.mediumBucket ? false : true;

        bigBucketOutline.color = unlocks.buckets.largeBucket ? Color.green : Color.red;
        bigBucketButton.enabled = unlocks.buckets.largeBucket ? false : true;
    }

    private void LoadBaits(Unlockables unlocks)
    {
        gachaOutline.color = unlocks.baits.gacha ? Color.green : Color.red;

        mandarinaOutline.color = unlocks.baits.mondarina ? Color.green : Color.red;
        mandarinaButton.enabled = unlocks.baits.mondarina ? false : true;

        wormOutline.color = unlocks.baits.worm ? Color.green : Color.red;
        wormButton.enabled = unlocks.baits.mondarina ? false : true;

    }

    public void UnlockBait(string baitName)
    {

        Unlockables unlockables = LoadSaveManager.Instance.LoadProgress();
        switch (baitName)
        {

            case "mandarina":
                if (_money >= mandarinaPrice)
                {
                    unlockables.baits.mondarina = true;
                    _money -= mandarinaPrice;
                    unlockables.currency = _money;
                }
                else
                {
                    ToggleModalVisibility();
                    return;
                }

                break;
            case "worm":
                if (_money >= wormPrice)
                {
                    unlockables.baits.worm = true;
                    _money -= wormPrice;
                    unlockables.currency = _money;
                }
                else
                {
                    ToggleModalVisibility();
                    return;
                }
                break;
            default: break;
        }

        LoadSaveManager.Instance.SaveProgress(unlockables);
        LoadProgress(); //Refresh the UI
    }

    private void LoadLocations(Unlockables unlocks)
    {
        rockSeaOutline.color = unlocks.locations.rockSea ? Color.green : Color.red;

        riverLocationOutline.color = unlocks.locations.river ? Color.green : Color.red;
        riverLocationButton.enabled = unlocks.locations.river ? false : true;

        seaLocationOutline.color = unlocks.locations.deepSea ? Color.green : Color.red;
        seaLocationButton.enabled = unlocks.locations.deepSea ? false : true;

    }

    public void UnlockLocation(string locationName)
    {

        Unlockables unlockables = LoadSaveManager.Instance.LoadProgress();
        switch (locationName)
        {

            case "river":
                if (_money >= riverLocationPrice)
                {
                    unlockables.locations.river = true;
                    _money -= riverLocationPrice;
                    unlockables.currency = _money;
                }
                else
                {
                    ToggleModalVisibility();
                    return;
                }

                break;
            case "deepsea":
                if (_money >= seaLocationPrice)
                {
                    _money -= seaLocationPrice; 
                    unlockables.locations.deepSea = true;
                    unlockables.currency = _money;
                }
                else
                {
                    ToggleModalVisibility();
                    return;
                }
                break;
            default: break;
        }

        LoadSaveManager.Instance.SaveProgress(unlockables);
        LoadProgress(); //Refresh the UI
        //Refresh the door UI
        _levelSelector.LoadLocationData();
    }

    public void ToggleModalVisibility()
    {
        modal.SetActive(!modal.activeSelf);
    }


    public void NavigateTabs(int index)
    {
        //Disable all pages
        foreach(GameObject parent in PageParents)
        {
            parent.SetActive(false);
        }
        PageParents[index].SetActive(true);

    }



}