using System.Collections;
using UnityEngine;

public class SpinningLineEnd : MonoBehaviour
{

    [SerializeField]
    private SpinningLineController refSpinningLineController;
    [SerializeField]
    private SpinningRodManager refSpinningRodManager;

    private float elapsedTime;
    private float randomTime;
    //TODO: POLIMORFISMO DE ESTAS CLASES.

    private int iterationCount = 0;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Water"))
        {
            Debug.Log("The line end is on the water");
            refSpinningLineController.OnEnterWater();


            elapsedTime = 0;
            iterationCount = 0;
            randomTime = Random.Range(5f, 10f); 
        }

    }
    private void OnTriggerStay(Collider other)
    {


        if (other.CompareTag("Water"))
        {
            elapsedTime += Time.deltaTime;
            if (refSpinningRodManager != null)
            {
                if (elapsedTime >= randomTime && !refSpinningRodManager.IsFishHooked)
                {
                    Debug.Log("Calling SpinningManager");
                    elapsedTime = 0;
                    StartCoroutine(refSpinningRodManager.FishingSequence(iterationCount));
                    iterationCount++;
                    if (iterationCount > 3) iterationCount = 0;
                    randomTime = Random.Range(5f, 10f);
                }
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water") && refSpinningLineController != null)
        {
            refSpinningLineController.OnExitWater();
        }
    }


}






