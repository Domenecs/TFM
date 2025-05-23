using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.XR.PXR;
using UnityEngine;

public class SpinningRodManager: MonoBehaviour
{

    // 0 left , 1 right
    private int selectHandId = 1;
    private int fishSizeFrequency = 0;


    [SerializeField]
    Animator spinningRodAnimator;

    //Readonly
    public bool IsFishHooked { get; private set; }


    private Dictionary<string, int> fishSizesFrequencies = new Dictionary<string, int>()
    {
        { "S", 250 },
        { "M", 200},
        { "L", 150},
        { "XL", 100},
        { "XXL", 50},
    };





    //Used to set which hand is holding to cane to send the haptic feedback.
    public void SetSelectedHand(int id)
    {
        selectHandId = id;
    }


    private IEnumerator FishBite( float duration = 0.5f,
        float hapticAmplitude = 0.5f, int frequency = 500, string trigger ="")
    {
        // Haptic feedback section
        int hapticDuration = (int)(duration * 1000);
        var vibrateType = (selectHandId == 0) ? PXR_Input.VibrateType.LeftController : PXR_Input.VibrateType.RightController;
            PXR_Input.SendHapticImpulse(vibrateType, hapticAmplitude, hapticDuration, fishSizeFrequency);

       spinningRodAnimator.SetTrigger(trigger);


        yield return new WaitForSeconds(duration);
        spinningRodAnimator.SetTrigger("ResetAnimation");
        
    }





    public IEnumerator FishingSequence(int sequence)
    {
        switch (sequence)
        {
            // 1 to 2  light strokes, 0.1- 0.3 amplitude, 100ms, to 200ms each stroke
            case 0:
                {
                    Debug.Log("Entering case 0");
                    //Generate the size of the fish. smaller having more chances than bigger ones.
                    GenerateFishSize();
                    float duration = Random.Range(0.5f, 1f);                   
                    //Haptic settings
                    float amplitude = Random.Range(0.1f, 0.4f);

                    yield return StartCoroutine(FishBite(duration,
                        amplitude,
                        fishSizeFrequency,
                        "LightStroke"
                        ));
                    yield break;
                }
            // 2 to 3 strokes, 0.4 - 0.6 amplitude, 200-400 ms
            case 1:
                {
                    Debug.Log("Entering case 1");
                    float duration = Random.Range(0.2f, 0.5f);
                    //Haptic settings
                    float amplitude = Random.Range(0.2f, 0.6f);

                    yield return StartCoroutine(FishBite(duration,
                        amplitude,
                        fishSizeFrequency,
                        "LightStroke"
                        ));
                    yield break;
                }
            // In this cane the fish can hook up. Is a dice is rolled to determine if it can be rolled.
            case 2:
                {
                    Debug.Log("Entering case 2");

                    if (Random.Range(0, 101) < 80)
                    {
                        float duration = Random.Range(0.2f, 0.5f);
                        //Haptic settings
                        float amplitude = Random.Range(0.4f, 0.6f);

                        yield return StartCoroutine(FishBite(duration,
                            amplitude,
                            fishSizeFrequency,
                            "HardStroke"
                            ));
                    }
                    else
                    {
                        StartCoroutine(nameof(FishHooked));
                    }
                    yield break;
                }
            case 3:
                {
                    Debug.Log("Entering case 3");
                    if (Random.Range(0, 101) < 50)
                    {
                        StartCoroutine(nameof(FishHooked));
                    }
                    yield break;
                }
        }

    }


    //Used when the fish has hooked into the hook.
    private IEnumerator FishHooked()
    {
        Debug.Log("The fish is on the hook");
        IsFishHooked = true;
        //Bend the cane.
        spinningRodAnimator.SetTrigger("FishHooked");
        //Move the line
        //Send Haptic Feedback.
        float RandomActionTime = Random.Range(2000, 3000);
        var vibrateType = (selectHandId == 0) ? PXR_Input.VibrateType.LeftController : PXR_Input.VibrateType.RightController;
        PXR_Input.SendHapticImpulse(vibrateType, Random.Range(0.8f, 1f), (int)RandomActionTime, fishSizeFrequency);
        yield return new WaitForSeconds(RandomActionTime / 1000);
        IsFishHooked = false;
        Debug.Log("The fish is no longer on the hook");

        yield return null;
    }


    //Picks a random item in the dictionary to determine the fishFrequency. Maybe change later.
    private void GenerateFishSize()
    {
        List<string> fishSizes = new List<string>(fishSizesFrequencies.Keys);
        int randomIndex = Random.Range(0, fishSizes.Count);
        string randomFish = fishSizes[randomIndex];
        fishSizeFrequency = fishSizesFrequencies[randomFish];
    }


    public void StopCourutines()
    {
        StopAllCoroutines();
    }



}

