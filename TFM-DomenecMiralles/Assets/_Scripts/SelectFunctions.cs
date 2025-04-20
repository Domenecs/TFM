using UnityEngine;

public class SelectFunctions : MonoBehaviour
{

    [SerializeField]
    private GameObject rightControllerVisual;


    [SerializeField]
    private GameObject leftControllerVisual;
    public void ToggleRControllerVisual()
    {
        
        rightControllerVisual.SetActive(!rightControllerVisual.activeSelf);
    }


    
}
