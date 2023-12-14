using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightObject : MonoBehaviour
{
    private NPCVoiceLines NPCV;
    public int timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        NPCV = GameObject.Find("NPCVoiceLines").GetComponent<NPCVoiceLines>();
        Invoke("invoker",timer );
    }

    private void invoker()
    {
        if (gameObject.name.Equals("Shovel"))
        {
        NPCV.playAudio(1);
        }
        StartCoroutine(HighlightObjects(gameObject));
    }

    private IEnumerator HighlightObjects(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material mat = renderer.material;

            Color initialColor = mat.color;
            Color targetColor =
                new Color(0f, initialColor.g, 0f, initialColor.a); // Set red and blue components to zero

            float elapsedTime = 0f;
            float totalTime = 1.0f;

            while (elapsedTime < totalTime)
            {
                mat.color = Color.Lerp(initialColor, targetColor, elapsedTime / totalTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure the final color is set to the target color
            mat.color = targetColor;

            // Wait for a moment (you can adjust the duration)
            yield return new WaitForSeconds(0.4f);

            // Change the color back to the initial color over the same duration
            elapsedTime = 0f;
            while (elapsedTime < totalTime)
            {
                mat.color = Color.Lerp(targetColor, initialColor, elapsedTime / totalTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure the final color is set to the initial color
            mat.color = initialColor;
        }
        else
        {
            Debug.LogWarning("Renderer component not found on the GameObject");
        }

        StartCoroutine(HighlightObjects(obj));
    }
}