using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.XR.PXR;
using UnityEngine;

public class RodBender : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference the shader material used to bend the end portion of the rod.")]
    private Material rodMaterial;

    // 0 left , 1 right
    private int selectHandId = 1;
    private int fishSizeFrequency = 0;

    [SerializeField]
    private GameObject lineStart;


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

    public void Start()
    {
        rodMaterial.SetFloat("_PullStrength", 0f);

    }



    private IEnumerator FishBite(float bendAmount = 0.5f, float duration = 0.5f, int numCycles = 1, float delayBetweenBites = 1f,
        float hapticAmplitude = 0.5f, int frequency = 500)
    {
        float halfDuration = duration / 2f;
        float timer;
        int hapticDuration = (int)(halfDuration * 1000);


        //TO move the line with the bending.
        Vector3 offSet = new Vector3(0f, -bendAmount, 0f);  
        Vector3 originalPosition = lineStart.transform.position;



        for (int i = 0; i < numCycles; i++)
        {

            // Haptic feedback section
            var vibrateType = (selectHandId == 0) ? PXR_Input.VibrateType.LeftController : PXR_Input.VibrateType.RightController;
            PXR_Input.SendHapticImpulse(vibrateType, hapticAmplitude, hapticDuration, fishSizeFrequency);

            //Rod Bending section
            timer = 0f;
            // Bend IN
            while (timer < halfDuration)
            {
                timer += Time.deltaTime;
                float t = timer / halfDuration;
                float bend = Mathf.Lerp(0f, bendAmount, t);
                rodMaterial.SetFloat("_PullStrength", bend);


                //Move the line
                Vector3 start = originalPosition;
                Vector3 end = originalPosition + offSet;
                lineStart.transform.position = Vector3.Lerp(start, end, t);

                yield return null;
            }

            timer = 0f;

            // Bend OUT
            while (timer < halfDuration)
            {
                timer += Time.deltaTime;
                float t = timer / halfDuration;
                float bend = Mathf.Lerp(bendAmount, 0f, t);
                rodMaterial.SetFloat("_PullStrength", bend);
                yield return null;


                //Move the line                 
                //Move the line
                Vector3 start = originalPosition + offSet;
                Vector3 end = originalPosition;
                lineStart.transform.position = Vector3.Lerp(start, end, t);


            }

            rodMaterial.SetFloat("_PullStrength", 0f); // Reset at end of cycle
            yield return new WaitForSeconds(delayBetweenBites); // Wait before next bite
        }
    }





    public IEnumerator FishingSequence(int sequence)
    {



        switch (sequence)
        {
            // 1 to 2 strokes, 0.1- 0.3 amplitude, 100ms, to 200ms each stroke
            case 0:
                {
                    Debug.Log("Entering case 0");
                    //Generate the size of the fish. smaller having more chances than bigger ones.
                    GenerateFishSize();
                    float bendAmount = Random.Range(0.3f, 0.5f);
                    float duration = Random.Range(0.1f, 0.3f);
                    int nCycles = (int)Random.Range(1, 3);
                    float delayBetweenBites = Random.Range(0.3f, 0.9f);
                    //Haptic settings
                    float amplitude = Random.Range(0.1f, 0.4f);

                    yield return StartCoroutine(FishBite(bendAmount,
                        duration,
                        nCycles,
                        delayBetweenBites,
                        amplitude,
                        fishSizeFrequency
                        ));
                    yield break;
                }
            // 2 to 3 strokes, 0.4 - 0.6 amplitude, 200-400 ms
            case 1:
                {
                    Debug.Log("Entering case 1");
                    float bendAmount = Random.Range(0.3f, 0.5f);
                    float duration = Random.Range(0.2f, 0.5f);
                    int nCycles = (int)Random.Range(2, 4);
                    float delayBetweenBites = Random.Range(0.3f, 0.9f);
                    //Haptic settings
                    float amplitude = Random.Range(0.1f, 0.4f);

                    yield return StartCoroutine(FishBite(bendAmount,
                        duration,
                        nCycles,
                        delayBetweenBites,
                        amplitude,
                        fishSizeFrequency
                        ));
                    yield break;
                }
                // In this cane the fish can hook up. Is a dice is rolled to determine if it can be rolled.
            case 2:
                {
                    Debug.Log("Entering case 2");

                    if (Random.Range(0, 101) < 80)
                    {
                        float bendAmount = Random.Range(0.3f, 0.5f);
                        float duration = Random.Range(0.2f, 0.5f);
                        int nCycles = (int)Random.Range(2, 4);
                        float delayBetweenBites = Random.Range(0.3f, 0.9f);
                        //Haptic settings
                        float amplitude = Random.Range(0.1f, 0.4f);

                        yield return StartCoroutine(FishBite(bendAmount,
                            duration,
                            nCycles,
                            delayBetweenBites,
                            amplitude,
                            fishSizeFrequency
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
                    if (Random.Range(0,101) < 50)
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
        rodMaterial.SetFloat("_PullStrength", 0.6f);
        //Move the line
        Vector3 offSet = new Vector3(0f, -0.6f, 0f);
        lineStart.transform.position = lineStart.transform.position + offSet;



        //Send Haptic Feedback.
        float RandomActionTime = Random.Range(1000, 1500);
        var vibrateType = (selectHandId == 0) ? PXR_Input.VibrateType.LeftController : PXR_Input.VibrateType.RightController;
        PXR_Input.SendHapticImpulse(vibrateType, Random.Range(0.8f, 1f), (int)RandomActionTime, fishSizeFrequency);
        yield return new WaitForSeconds(RandomActionTime/1000);
        IsFishHooked = false;
        Debug.Log("The fish is no longer on the hook");

        yield return null;
        //Set a variable true I guess. so OnTigger exit works.
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

