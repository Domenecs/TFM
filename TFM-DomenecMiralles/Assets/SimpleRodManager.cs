using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.XR.PXR;
using UnityEngine;
using static FishingData;

public class SimpleRodManager : MonoBehaviour
{

    // 0 left , 1 right
    private int selectHandId = 1;
    private FishSize generatedFishSize;

    [SerializeField]
    Animator simpleRodAnimator;


    [SerializeField]
    private AccesoriesManager accesoriesManager;
    [SerializeField] private List<FishData> allFishData;



    //Readonly
    public bool IsFishHooked { get; private set; }

    [SerializeField]
    private RodType rodtype;

    //Used to set which hand is holding to cane to send the haptic feedback.
    public void SetSelectedHand(int id)
    {
        selectHandId = id;
    }


    private IEnumerator FishBite(float duration = 0.5f,
        float hapticAmplitude = 0.5f, int frequency = 500, string trigger = "")
    {
        // Haptic feedback section
        int hapticDuration = (int)(duration * 1000);
        var vibrateType = (selectHandId == 0) ? PXR_Input.VibrateType.LeftController : PXR_Input.VibrateType.RightController;
        PXR_Input.SendHapticImpulse(vibrateType, hapticAmplitude, hapticDuration, FishingData.Frequencies[generatedFishSize]);

        simpleRodAnimator.SetTrigger(trigger);


        yield return new WaitForSeconds(duration);
        simpleRodAnimator.SetTrigger("ResetAnimation");

    }





    public IEnumerator FishingSequence(int sequence)
    {
        FishingData.HookType currentHook = accesoriesManager.GetCurrentHook();
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
        simpleRodAnimator.SetTrigger("FishHooked");
        //Move the line
        //Send Haptic Feedback.
        float RandomActionTime = Random.Range(2000, 3000);
        var vibrateType = (selectHandId == 0) ? PXR_Input.VibrateType.LeftController : PXR_Input.VibrateType.RightController;
        PXR_Input.SendHapticImpulse(vibrateType, Random.Range(0.8f, 1f), (int)RandomActionTime, FishingData.Frequencies[generatedFishSize]);
        yield return new WaitForSeconds(RandomActionTime / 1000);
        IsFishHooked = false;
        simpleRodAnimator.SetTrigger("ResetAnimation");
        Debug.Log("The fish is no longer on the hook");

        yield return null;
    }


    private void GenerateFishSize()
    {
        //% chances for the current rod.
        var chances = RodSizeChances[rodtype];
        float rand = Random.Range(0f, 100f);
        float acumulative = 0f;

        foreach (var key in chances)
        {
            acumulative += key.Value;
            if (rand <= acumulative)
                generatedFishSize = key.Key;
        }
        //Per si decás.
    }


    public void OnPlayerPulledRod()
    {
        Debug.Log("Got the fish!");
        StopAllCoroutines();

        BaitType currentBait = accesoriesManager.GetCurrentBait();
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

        if(fishCandidates.Count == 0)
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
        GameObject hookParent = accesoriesManager.GetHookGameObject();
        Instantiate(selectedFish.prefab, hookParent.transform);
        IsFishHooked = false;
        simpleRodAnimator.SetTrigger("ResetAnimation");

    }


}

