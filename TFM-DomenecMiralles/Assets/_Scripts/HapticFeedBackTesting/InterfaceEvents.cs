using Unity.XR.PXR;
using UnityEngine;
using static Unity.XR.PXR.PXR_Input;

public class InterfaceEvents : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float amplitude = 0.1f;
    private int frequency = 50;
    private int duration = 100;

    //To enable non-buffered haptics for the right controller, set the amplitude to 0.5, the duration to 500ms, and the frequency to 100Hz


    public void SetAmplitude(int index)
    {
        switch (index)
        {
            case 0: amplitude = 0.1f;
                break;
            case 1: amplitude = 0.3f;
                break;
            case 2: amplitude = 0.5f;
                break;
            case 3: amplitude = 0.7f;
                break;
            case 4: amplitude = 1.0f;
                break;
            default:
                break;
        }
    }

    public void SetFrequency(int index)
    {
        switch (index)
        {
            case 0:
                frequency = 50;
                break;
            case 1:
                frequency = 100;
                break;
            case 2:
                frequency = 150;
                break;
            case 3:
                frequency = 200;
                break;
            case 4:
                frequency = 300;
                break;
            default:
                break;
        }
    }


    public void SetDuration(int index)
    {
        switch (index)
        {
            case 0:
                duration = 100;
                break;
            case 1:
                duration = 500;
                break;
            case 2:
                duration = 1000;
                break;

        }
    }


    public void PlayHapticFeedBack()
    {
        PXR_Input.SendHapticImpulse(PXR_Input.VibrateType.RightController, amplitude, duration, frequency);

    }

}
