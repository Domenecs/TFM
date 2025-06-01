using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.XR.PXR;
using UnityEngine;
using static FishingData;

public class SpinningRodManager: MonoBehaviour
{

    // 0 left , 1 right
    private int selectHandId = 1;
    private int fishSizeFrequency = 0;


    [SerializeField]
    Animator spinningRodAnimator;

    [Header("FishData")]
    [SerializeField] private List<FishData> allFishData;
    [SerializeField] private GameObject hookParent;
    private FishSize generatedFishSize;



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
        FishingData.HookType currentHook = HookType.BigHook;
        int hookChance = FishingData.HookChances[currentHook];

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
                        FishingData.Frequencies[generatedFishSize],
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
                       FishingData.Frequencies[generatedFishSize],
                        "LightStroke"
                        ));
                    yield break;
                }
            // In this cane the fish can hook up. Is a dice is rolled to determine if it can be rolled.
            case 2:
                {
                    Debug.Log("Entering case 2");

                    if (Random.Range(0, 101) < hookChance)
                    {
                        float duration = Random.Range(0.2f, 0.5f);
                        //Haptic settings
                        float amplitude = Random.Range(0.4f, 0.6f);

                        yield return StartCoroutine(FishBite(duration,
                            amplitude,
                            FishingData.Frequencies[generatedFishSize],
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
                    if (Random.Range(0, 101) < hookChance)
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
        PXR_Input.SendHapticImpulse(vibrateType, Random.Range(0.8f, 1f), (int)RandomActionTime, FishingData.Frequencies[generatedFishSize]);
        yield return new WaitForSeconds(RandomActionTime / 1000);
        IsFishHooked = false;
        spinningRodAnimator.SetTrigger("ResetAnimation");
        Debug.Log("The fish is no longer on the hook");

        yield return null;
    }


    //Picks a random item in the dictionary to determine the fishFrequency. Maybe change later.
    private void GenerateFishSize()
    {
        //% chances for the current rod.
        var chances = RodSizeChances[RodType.Intermediate];
        float rand = Random.Range(0f, 100f);
        float acumulative = 0f;

        foreach (var key in chances)
        {
            acumulative += key.Value;
            if (rand <= acumulative)
                generatedFishSize = key.Key;
        }
    }


    public void OnPlayerPulledRod()
    {
        Debug.Log("Got the fish!");
        StopAllCoroutines();

        BaitType currentBait = BaitType.Jig;
        //CReate a list of fishes with probability > 0
        List<(FishData fish, float probability)> fishCandidates = new List<(FishData, float)>();

        foreach (var fish in allFishData)
        {
            var baitProb = fish.baitChances.Find(b => b.baitType == currentBait);
            if (baitProb != null && baitProb.probability > 0f)
            {
                fishCandidates.Add((fish, baitProb.probability));
            }
        }

        if (fishCandidates.Count == 0)
        {
            Debug.LogWarning("No fish available for the current bait");
            return;
        }

        float rand = Random.Range(0f, 100f);
        float acumulative = 0f;
        FishData selectedFish = fishCandidates[0].fish; // Just in case.


        foreach (var fish in fishCandidates)
        {
            acumulative += fish.probability;
            if (rand <= acumulative)
            {
                selectedFish = fish.fish;
                break;
            }

        }
        GameObject instiantatedFish = Instantiate(selectedFish.prefab, hookParent.transform);
        instiantatedFish.transform.localScale *= FishingData.ScaleMultipliers[generatedFishSize];
        instiantatedFish.transform.localRotation = Quaternion.Euler(180f, 0f, 0f);
        instiantatedFish.transform.localPosition += Vector3.zero;
        instiantatedFish.transform.localPosition += selectedFish.offSet;

        //Asign its value
        FishFunctions refFishFunction = instiantatedFish.GetComponentInParent<FishFunctions>();
        if (refFishFunction == null)
        {
            Debug.LogWarning("Couldn't find the fishFunction script");
        }
        else
        {
            refFishFunction.SetFishValue((int)(selectedFish.fishValue * instiantatedFish.transform.localScale.x));
        }

        IsFishHooked = false;
        spinningRodAnimator.SetTrigger("ResetAnimation");

    }


}

