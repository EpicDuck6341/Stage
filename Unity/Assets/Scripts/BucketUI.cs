using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class BucketUI : MonoBehaviour
{
    private Image image;
    private float fillSpeed = 0.5f;
    private float targetFillLevel;

    void Start()
    {
        image = gameObject.GetComponent<Image>();
        targetFillLevel = image.fillAmount;
    }

    
    public void increaseFill(float amount)
    {
        targetFillLevel += 1/amount;
        StartCoroutine(ChangeFillOverTime());
    }

    public void decreaseFill(float amount)
    {
        targetFillLevel -= 1/amount;
        StartCoroutine(ChangeFillOverTime());
    }

    IEnumerator ChangeFillOverTime()
    {
        while (!Mathf.Approximately(image.fillAmount, targetFillLevel))
        {
            image.fillAmount = Mathf.MoveTowards(image.fillAmount, targetFillLevel, fillSpeed * Time.deltaTime);
            yield return null;
        }
    }
}