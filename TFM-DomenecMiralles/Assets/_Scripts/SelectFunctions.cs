using UnityEngine;

public class SelectFunctions : MonoBehaviour
{

    [SerializeField]
    private GameObject rightControllerVisual;


    [SerializeField]
    private GameObject leftControllerVisual;


    public void ToggleRightControllerVisual()
    {
        
        rightControllerVisual.SetActive(!rightControllerVisual.activeSelf);
    }

    public void ToggleLeftControllerVisual()
    {
        leftControllerVisual.SetActive(!leftControllerVisual.activeSelf);
    }

    
}
