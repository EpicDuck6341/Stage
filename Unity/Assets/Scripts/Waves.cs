using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    
    private float raiseHeight = 1.2f; // The height to raise the object to
    private float raiseTime = 5f;   // Time taken to raise the object
    private float lowerTime = 5f;   // Time taken to lower the object

    private void Start()
    {
        StartCoroutine(RaiseAndLowerObject());
    }

    IEnumerator RaiseAndLowerObject()
    {
        while (true)
        {
            // Raise the object
            float timer = 0f;
            while (timer < raiseTime)
            {
                transform.Translate(Vector3.up * Time.deltaTime * (raiseHeight / raiseTime));
                timer += Time.deltaTime;
                yield return null;
            }

            // Wait for a short duration at the raised position
            yield return new WaitForSeconds(1f);

            // Lower the object
            timer = 0f;
            while (timer < lowerTime)
            {
                transform.Translate(Vector3.down * Time.deltaTime * (raiseHeight / lowerTime));
                timer += Time.deltaTime;
                yield return null;
            }

            // Wait for a short duration at the lowered position
            yield return new WaitForSeconds(1f);
        }
    }
}
