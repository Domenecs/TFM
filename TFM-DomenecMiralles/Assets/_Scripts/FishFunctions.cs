using UnityEngine;

public class FishFunctions : MonoBehaviour
{

    [SerializeField] private int fishValue;
    [SerializeField] private GameObject uiParent;

    public void ClearParent()
    {
        transform.parent = null;    
    }


    public void SetFishValue(int value)
    {
        fishValue = value;
    }

    public int GetFishValue()
    {
        return fishValue;
    }


    public void ToggleUIVisiblity()
    {
        uiParent.SetActive(!uiParent.activeInHierarchy);
    }
}
