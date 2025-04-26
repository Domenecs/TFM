using System.Collections;
using UnityEngine;

public class RodBender : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference the shader material used to bend the end portion of the rod.")]
    private Material rodMaterial;

    private void Start()
    {

        float randomStrength = Random.Range(0.5f, 1f);
        float randomDuration = Random.Range(0.07f, 0.12f);
        StartCoroutine(FishBite(randomStrength, randomDuration, 10, 1f));

    }



    public IEnumerator FishBite(float bendAmount = 0.5f, float duration = 0.5f, int numCycles = 1, float delayBetweenBites = 1f,
        float hapticAmplitude = 0.5f, float frequency = 500f, float msDuration = 100)
    {
        float halfDuration = duration / 2f;
        float timer;
        for (int i = 0; i < numCycles; i++)
        {

            timer = 0f;
            // Bend IN
            while (timer < halfDuration)
            {
                timer += Time.deltaTime;
                float t = timer / halfDuration;
                float bend = Mathf.Lerp(0f, bendAmount, t);
                rodMaterial.SetFloat("_PullStrength", bend);
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
            }

            rodMaterial.SetFloat("_PullStrength", 0f); // Reset at end of cycle
            yield return new WaitForSeconds(delayBetweenBites); // Wait before next bite
        }
    }


    public IEnumerator FishingSequence(int sequence)
    {

        switch (sequence)
        {
            //1 quick stroke
            case 0:
                {
                    yield return StartCoroutine(FishBite(0.4f, 0.4f, 1, 1, 0.2f, 200, 100));
                        yield break;
                }
            case 1:
                {
                    yield break; 
                }
            case 2:
                {
                    yield break; 
                }
            case 3:
                {
                    yield break; 
                }







        }





    }

}
