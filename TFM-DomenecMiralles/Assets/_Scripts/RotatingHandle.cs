using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class RotatingHandle : XRBaseInteractable
{
    [SerializeField] private Transform handleTransform;
    [SerializeField] private float rotationSpeed = 1.8f;

    [SerializeField] private Transform rotatingPieceTransform;



    [SerializeField] private Transform movingPieceTransform;
    [SerializeField] private float verticalMovementAmplitude = 0.02f; //Desplazamiento total.
    [SerializeField] private float verticalMovementFrequency = 1f; //Velocity of the cycle
    public UnityEvent<float> OnWheelRotated;

    private Vector3 previousDirection;
    private float totalRotatedAngle = 0f;

    [Header("Line handling")]
    [SerializeField] private SpinningLineController refSpinningLineController;
    [SerializeField] private float segmentLengthStep = 0.001f;
    [SerializeField] private float angleThreshold = 5.0f; // puedes ajustar este valor


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // Dirección inicial desde interactor al centro del objeto
        Vector3 handPos = args.interactorObject.transform.position;
        previousDirection = (handPos - transform.position).normalized;

        Debug.Log("Grabbing the object");
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic && isSelected)
        {
            RotateWheel();
        }
    }

    private void RotateWheel()
    {
        if (interactorsSelecting.Count == 0)
            return;

        var interactor = interactorsSelecting[0];
        Vector3 currentDirection = (interactor.transform.position - transform.position).normalized;

        // Rotar alrededor del eje X local del handle
        Vector3 rotationAxis = handleTransform.right;

        float angle = Vector3.SignedAngle(previousDirection, currentDirection, rotationAxis);
        handleTransform.Rotate(rotationAxis, angle * rotationSpeed, Space.World);

        //Incrementar la longitud de la línea.
        if (refSpinningLineController != null && Mathf.Abs(angle) > angleThreshold)
        {
            //Se esta girando hacia delante.
            if (angle > 0f)
            {
                refSpinningLineController.ChangeLineLength(segmentLengthStep);
            }
            else if (angle < 0f)
            {
                refSpinningLineController.ChangeLineLength(-segmentLengthStep);
            }
        }



        //Rotar la pieza que se mueve
        rotationAxis = rotatingPieceTransform.up;
        rotatingPieceTransform.Rotate(rotationAxis, angle * rotationSpeed, Space.World);

        //Mover la pieza arriba y abajo.
        totalRotatedAngle += angle * rotationSpeed;
        float yOffset = Mathf.Sin(totalRotatedAngle * verticalMovementFrequency * Mathf.Deg2Rad) * verticalMovementAmplitude;

        // Posición original + offset
        Vector3 localPos = movingPieceTransform.localPosition;
        localPos.y = yOffset;
        movingPieceTransform.localPosition = localPos;


        previousDirection = currentDirection;
        OnWheelRotated?.Invoke(angle);

    }
}
