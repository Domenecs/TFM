using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class AccesoriesManager : MonoBehaviour
{
    [SerializeField]
    private string _lastSelectedCaneName = "SimpleRodLineEnd";
    private string _selectedEndLine;
    private string _selectedBait;

    [SerializeField] 
    private List<GameObject> LineHiders;

    public void ChangeEndLine(int index)
    {
        Debug.Log("Changing the end line..");
        GameObject lineEnd = GameObject.Find(_lastSelectedCaneName);
        GameObject hooksParent = lineEnd.transform.GetChild(0).gameObject; 
        if (lineEnd == null) Debug.LogWarning("Couldn't find the cane!");
        



        foreach (Transform child in hooksParent.transform)
        {
            child.gameObject.SetActive(false);
        }
        hooksParent.transform.GetChild(index).gameObject.SetActive(true);
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
        if (lineEnd == null) Debug.LogWarning("Couldn't find the cane!");
        GameObject baitParent = lineEnd.transform.GetChild(1).gameObject;

        foreach (Transform child in baitParent.transform)
        {
            child.gameObject.SetActive(false);
        }
        baitParent.transform.GetChild(index).gameObject.SetActive(true);
        
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

}
