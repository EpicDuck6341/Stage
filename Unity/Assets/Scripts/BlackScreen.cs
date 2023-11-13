using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{
    public GameObject canvas;
    public GameObject camera;
    public GameObject reticle;
    public GameObject bucket;
    private Image imageComponent;
    

    private void Start()
    {
        imageComponent = canvas.GetComponent<Image>();
    }

    public IEnumerator FadeToBlack(float duration,Vector3 pos)
    {
        // Ensure the Image component is not null

        reticle.GetComponent<Image>().enabled = false;
        // Set the initial alpha to 0
        Color startColor = imageComponent.color;
        startColor.a = 0f;
        imageComponent.color = startColor;

        // Calculate the increment per frame
        float alphaIncrement = Time.fixedDeltaTime / duration;

        // Gradually increase the alpha from 0 to 1 over the specified duration
        while (imageComponent.color.a < 1f)
        {
            startColor.a += alphaIncrement;
            imageComponent.color = startColor;
            yield return new WaitForFixedUpdate();
        }

        camera.transform.localPosition = pos;
        yield return new WaitForSeconds(0.3f);

        // Gradually decrease the alpha from 1 to 0 over the specified duration (fade-out)
        while (imageComponent.color.a > 0f)
        {
            startColor.a -= alphaIncrement;
            imageComponent.color = startColor;
            yield return new WaitForFixedUpdate();
        }
        reticle.GetComponent<Image>().enabled = true;
    }
}