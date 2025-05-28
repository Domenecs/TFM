using UnityEngine;

public class FishFunctions : MonoBehaviour
{

    [SerializeField] private int fishValue;


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

}
