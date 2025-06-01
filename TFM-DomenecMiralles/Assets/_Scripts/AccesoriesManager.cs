using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using static FishingData;

public class AccesoriesManager : MonoBehaviour
{
    [SerializeField]
    private string _lastSelectedCaneName = "SimpleRodLineEnd";

    [SerializeField] 
    private List<GameObject> LineHiders;


    [SerializeField]
    private FishingData.BaitType currentBait = FishingData.BaitType.Gacha;
    [SerializeField]
    private FishingData.HookType currentHook = FishingData.HookType.SmallHook;

    [Header("Save Load data settings")]
    [SerializeField]
    private GameObject baitParent;
    [SerializeField]
    private GameObject hooksParent;

    private void Start()
    {
        LoadProgress();
    }

    public void ChangeEndLine(int index)
    {
        Debug.Log("Changing the end line..");
        GameObject lineEnd = GameObject.Find(_lastSelectedCaneName);
        if (lineEnd == null)
        {
            Debug.LogWarning("Couldn't find the cane!");
            return;
        }
        GameObject hooksParent = lineEnd.transform.GetChild(0).gameObject; 
        
        foreach (Transform child in hooksParent.transform)
        {
            child.gameObject.SetActive(false);
        }
        hooksParent.transform.GetChild(index).gameObject.SetActive(true);
        currentHook = IndexToHookType(index);
        DisableLineHider();
    }

    private void DisableLineHider()
    {
        foreach (GameObject lineHider in LineHiders)
        {
            lineHider.gameObject.SetActive(false);
        }
    }

    public void ChangeBait(int index)
    {
        GameObject lineEnd = GameObject.Find(_lastSelectedCaneName);
        if (lineEnd == null)
        {
            Debug.LogWarning("Couldn't find the cane!");
            return;
        }
        GameObject baitParent = lineEnd.transform.GetChild(1).gameObject;

        foreach (Transform child in baitParent.transform)
        {
            child.gameObject.SetActive(false);
        }
        baitParent.transform.GetChild(index).gameObject.SetActive(true);
        currentBait = IndexToBaitType(index);
    }


    public void ResetRod()
    { 
        GameObject lineEnd = GameObject.Find(_lastSelectedCaneName);
        GameObject hooksParent = lineEnd.transform.GetChild(0).gameObject; 
        if (lineEnd == null) Debug.LogWarning("Couldn't find the cane!");
        
        foreach (Transform child in hooksParent.transform)
        {
            child.gameObject.SetActive(false);
        }

        GameObject baitParent = lineEnd.transform.GetChild(1).gameObject;

        foreach (Transform child in baitParent.transform)
        {
            child.gameObject.SetActive(false);
        }

    }

    public void ChangeSelectedRod(string newRodName)
    {
        Debug.Log("Changed the last selected rod");
        _lastSelectedCaneName = newRodName;
    }

    private FishingData.BaitType IndexToBaitType(int index)
    {
        switch (index)
        {
            case 0: return FishingData.BaitType.Gacha;
            case 1: return FishingData.BaitType.Mandarina;
            case 2: return FishingData.BaitType.Gusano;
            case 3: return FishingData.BaitType.Jig;
            default: return FishingData.BaitType.Gacha;
        }
    }

    private FishingData.HookType IndexToHookType(int index)
    {
        // Asignar el tipo de gancho basado en el índice
        switch (index)
        {
            case 0: return FishingData.HookType.SmallHook;
            case 1: return FishingData.HookType.BigHook;
            case 2: return FishingData.HookType.TripleHook;
            default: return FishingData.HookType.SmallHook;
        }
    }


    public FishingData.BaitType GetCurrentBait()
    {
        return currentBait;
    }

    public FishingData.HookType GetCurrentHook()
    {
        return currentHook; 
    }

    public GameObject GetHookGameObject()
    {
        GameObject lineEnd = GameObject.Find(_lastSelectedCaneName);
        if (lineEnd == null)
        {
            Debug.LogWarning("Coudln't fin the cane: " + _lastSelectedCaneName);
            return null;
        }

        GameObject hooksParent = lineEnd.transform.GetChild(0).gameObject; // Asegúrate de que el índice 0 corresponde a los hooks

        foreach (Transform child in hooksParent.transform)
        {
            if (child.gameObject.activeSelf)
            {
                return child.gameObject;
            }
        }

        Debug.LogWarning("No hay ningún hook activo.");
        return null;
    }



    private void LoadProgress()
    {
      Unlockables unlockables =   LoadSaveManager.Instance.LoadProgress();
        HandleBaitUnlocks(unlockables.baits);
        HandleHookUnlocks(unlockables.hooks);
        Debug.Log(Application.persistentDataPath);
    }

    private void HandleBaitUnlocks(Baits baits)
    {
        foreach(Transform bait in baitParent.transform)
        {
            switch (bait.name.ToLower())
            {
                case "gacha":
                    bait.gameObject.SetActive(baits.gacha);
                    break;
                case "mondarina":
                    bait.gameObject.SetActive(baits.mondarina);
                    break;
                case "worm":
                    bait.gameObject.SetActive(baits.worm);
                    break;
                default:
                    Debug.LogWarning($"Bait GameObject '{bait.name}' no reconocido.");
                    break;
            }
        }
    }


    private void HandleHookUnlocks(Hooks hooks)
    {
        foreach (Transform hook in hooksParent.transform)
        {
            switch (hook.name.ToLower())
            {
                case "smallhook":
                    hook.gameObject.SetActive(hooks.smallhook);
                    break;
                case "bighook":
                    hook.gameObject.SetActive(hooks.bighook);
                    break;
                case "triplehook":
                    hook.gameObject.SetActive(hooks.triplehook);
                    break;
                default:
                    Debug.LogWarning($"Bait GameObject '{hook.name}' no reconocido.");
                    break;
            }
        }
    }
}
