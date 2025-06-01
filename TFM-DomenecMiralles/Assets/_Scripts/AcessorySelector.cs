using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class AcessorySelector : XRBaseInteractable
{
    [Header("NON XR SETTINGS")]
    [SerializeField] private AccesoriesManager accesoriesManager;
    [SerializeField] private int index = 0;

    [SerializeField] private GameObject canvas;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if(transform.parent.name.Equals("Hooks"))
        {
            accesoriesManager.ChangeEndLine(index);
        }else if (transform.parent.name.Equals("Baits"))
        {
            accesoriesManager.ChangeBait(index);
        }
        
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        canvas.SetActive(true);
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        canvas.SetActive(false);
    }

}
