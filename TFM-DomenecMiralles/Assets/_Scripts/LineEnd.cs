using System.Collections;
using UnityEngine;

public class LineEnd : MonoBehaviour
{


    [SerializeField]
    private RodBender refRodBender;
    [SerializeField]
    private SpinningRodManager refSpinningRodManager;

    private float elapsedTime;
    private float randomTime;
    //TODO POLIMORFISMO DE ESTAS CLASES.

    private int iterationCount = 0;

    [Tooltip("Prefabs of the fishes you that can be spawned into the hook")]
    [SerializeField]
    private GameObject[] fishPrefabs;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Water"))
        {
            Debug.Log("The line end is on the water");
            elapsedTime = 0;
            iterationCount = 0;
            randomTime = Random.Range(5f, 20f); // Fix later depending on the lure used.
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        

        if (other.CompareTag("Water"))
        {
            elapsedTime += Time.deltaTime;

            if (refRodBender != null)
            {
                if (elapsedTime >= randomTime && !refRodBender.IsFishHooked)
                {
                    Debug.Log("Calling RodBender");
                    elapsedTime = 0;
                    StartCoroutine(refRodBender.FishingSequence(iterationCount));
                    iterationCount++;
                    if (iterationCount > 3) iterationCount = 0;
                    randomTime = Random.Range(5f, 10f);
                }
            }
            else if (refSpinningRodManager != null)
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
        if (other.CompareTag("Water"))
        {

            if(refRodBender != null)
            {
                if (refRodBender.IsFishHooked)
                {
                    Debug.Log("you caught the fish");
                    int randomIndex = Random.Range(0, fishPrefabs.Length);

                    Instantiate(fishPrefabs[randomIndex], transform.position, Quaternion.identity);
                    refRodBender.StopAllCoroutines();
                }
                else
                {
                    Debug.Log("pulled to early");
                    refRodBender.StopAllCoroutines();
                }
            }else if(refSpinningRodManager != null)
            {
                if (refSpinningRodManager.IsFishHooked)
                {
                    Debug.Log("you caught the fish");
                    int randomIndex = Random.Range(0, fishPrefabs.Length);

                    Instantiate(fishPrefabs[randomIndex], transform.position, Quaternion.identity);
                    refSpinningRodManager.StopAllCoroutines();
                }
                else
                {
                    Debug.Log("pulled to early");
                    refSpinningRodManager.StopAllCoroutines();
                }
            }


        }
    }



}
