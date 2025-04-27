using System.Collections;
using UnityEngine;

public class LineEnd : MonoBehaviour
{
    [SerializeField]
    private LineController refFishingController;

    [SerializeField]
    private RodBender refRodBender;

    private float elapsedTime;
    private float randomTime;


    private int iterationCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            elapsedTime = 0;
            iterationCount = 0;
            randomTime = Random.Range(5f, 20f); // Fix later depending on the lure used.
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            if (elapsedTime >= randomTime)
            {
                elapsedTime = 0;
                refRodBender.FishingSequence(iterationCount);
                iterationCount++;
                if (iterationCount > 3) iterationCount = 0;
                randomTime = Random.Range(5f, 10f); 
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            if (refRodBender.IsFishHooked)
            {
                Debug.Log("you caught the fish");
                //yay , you won!!
                refRodBender.StopAllCoroutines();
            }
            else
            {
                Debug.Log("pulled to early");
                refRodBender.StopAllCoroutines();
            }
        }
    }



}
