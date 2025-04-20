using System.Collections;
using UnityEngine;

public class LineEnd : MonoBehaviour
{
    [SerializeField]
    private FishingLineController refFishingController;


    private float elapsedTime;
    private float randomTime;

    private bool isInMiniGame;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            refFishingController.FreezeBobber();

            elapsedTime = 0;
            randomTime = Random.Range(5f, 30f);
            isInMiniGame = false;
            //Detect the position where it collided.
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            if (!isInMiniGame) elapsedTime += Time.deltaTime;
            if (elapsedTime >= randomTime)
            {
                StartCoroutine(nameof(FishingMiniGame));
                isInMiniGame = true;
                elapsedTime = 0;
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInMiniGame = false;
            StopAllCoroutines();
        }
    }


    IEnumerator FishingMiniGame()
    {

        yield return null;
    }
}
