using Unity.XR.PXR;
using UnityEngine;

public class HapticTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PXR_Input.SendHapticImpulse(PXR_Input.VibrateType.LeftController, 0.5f, 500, 100);
    }

}
