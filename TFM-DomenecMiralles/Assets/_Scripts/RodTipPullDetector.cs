using UnityEngine;

public class RodTipPullDetector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpinningRodManager spinningRodManager;
    [SerializeField] private SimpleRodManager simpleRodManager;

    [Header("Detection Settings")]
    [SerializeField] private float checkInterval = 0.05f;
    [SerializeField] private float pullThreshold = 1.5f;

    private Vector3 lastPosition;
    private float timer = 0f;
    private bool isDetecting = false;

    private void Update()
    {

        if (!isDetecting && spinningRodManager != null)
        {
            if (spinningRodManager.IsFishHooked)
            {
                StartDetecting();
            }
        }

        if (!isDetecting && simpleRodManager != null)
        {
            if (simpleRodManager.IsFishHooked)
            {
                StartDetecting();
            }
        }

        if (!isDetecting) return;

        timer += Time.deltaTime;
        if (timer >= checkInterval)
        {
            Vector3 currentPosition = transform.position;
            Vector3 velocity = (currentPosition - lastPosition) / timer;

            if (velocity.magnitude >= pullThreshold)
            {
                Debug.Log("�Tir�n detectado desde la ca�a!");
                if(spinningRodManager != null) spinningRodManager.OnPlayerPulledRod();
                if (simpleRodManager != null) simpleRodManager.OnPlayerPulledRod();
                StopDetecting(); 
                return;
            }

            timer = 0f;
            lastPosition = currentPosition;
        }

        if (spinningRodManager != null)
        {
            if(!spinningRodManager.IsFishHooked)
            {
                StopDetecting();
            }
        }

        if (simpleRodManager != null)
        {
            if (!simpleRodManager.IsFishHooked)
            {
                StopDetecting();
            }
        }
    }

    private void StartDetecting()
    {
        isDetecting = true;
        lastPosition = transform.position;
        timer = 0f;
    }

    private void StopDetecting()
    {
        isDetecting = false;
        timer = 0f;
    }
}
