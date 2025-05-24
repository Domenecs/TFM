using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class FishingInputController : MonoBehaviour
{
    [SerializeField]
    private InputActionReference _triggerAction;


    [Header("XR Interactor")]
    [SerializeField] private XRBaseInteractor interactor;


    [Header("Throwing Logic")]
    [SerializeField] private SpinningLineController lineController;
    [SerializeField] private Transform castDirectionReference;
    [SerializeField] private float maxHoldTime = 2f;
    [SerializeField] private float maxLaunchForce = 10f;

    

    private float holdStartTime;
    private bool isHoldingRod = false;

    private void OnEnable()
    {
        _triggerAction.action.started += TriggerPressed;
        _triggerAction.action.canceled += TriggerReleased;

    }

    private void OnDisable()
    {
        _triggerAction.action.started -= TriggerPressed;
        _triggerAction.action.canceled += TriggerReleased;

    }


    private void TriggerPressed(InputAction.CallbackContext context)
    {
        if(interactor  == null)
        {
            Debug.LogWarning("No interactor assigned");
            return;
        }
     var interactable = interactor.firstInteractableSelected;

        if (interactable != null)
        {
            GameObject heldObject = interactable.transform.gameObject;
            if (heldObject.CompareTag("SpinningRod"))
            {
                isHoldingRod = true;
                holdStartTime = Time.time;
                Debug.Log("Started charging the cast");

            }
        }
    }

    private void TriggerReleased(InputAction.CallbackContext context)
    {
        if (!isHoldingRod)
            return;




        float holdDuration = Time.time - holdStartTime;
        //Cap the launchforce.
        float percent = Mathf.Clamp01(holdDuration / maxHoldTime);
        float force = percent * maxLaunchForce;

        Vector3 launchDir = castDirectionReference.forward;
        lineController.ReleaseCasting(launchDir * force);
        Debug.Log($"Released cast with force: {force:F2}");

        isHoldingRod = false;
    }
}
