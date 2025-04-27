using UnityEngine;

public class SelectFunctions : MonoBehaviour
{

    [SerializeField]
    private GameObject rightControllerVisual;


    [SerializeField]
    private GameObject leftControllerVisual;


    [SerializeField]
    private RodBender refRodBender;


    public void ToggleRightControllerVisual()
    {
        
        rightControllerVisual.SetActive(!rightControllerVisual.activeSelf);
        refRodBender.SetSelectedHand(1);
    }

    public void ToggleLeftControllerVisual()
    {
        leftControllerVisual.SetActive(!leftControllerVisual.activeSelf);
        refRodBender.SetSelectedHand(0);
    }

    
}
