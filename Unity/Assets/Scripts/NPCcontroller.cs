using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class NPCcontroller : MonoBehaviour
{
    public GameObject player;
    public Animator anim;
    public GameObject neck;
    private float minDistance;
    private CountdownManager CM;
    private AudioPlayer AP;

    private ObjectNaming ON;
    // Start is called before the first frame update
    void Start()
    {
        
        CM = GameObject.Find("CountdownManager").GetComponent<CountdownManager>();
        AP = GameObject.Find("AudioManager").GetComponent<AudioPlayer>();
        ON = GameObject.Find("FirstPersonController").GetComponent<ObjectNaming>();
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position + new Vector3(0, 0.5f, 0), -gameObject.transform.up, out hit,
                50))
        {
            minDistance = hit.distance;
        }
        
    }


    public IEnumerator MoveNPC(float duration, Vector3 target)
    {
        Vector3 orgPos = gameObject.transform.position;
        Vector3 targetPos = target;
        float minimum = 1.5f;
        anim.SetBool("walk", true);

        // Move forward
        float elapsedTime = 0;
        while (elapsedTime < duration / 2)
        {
            float t = elapsedTime / (duration / 2);
            Vector3 newPosition = Vector3.Lerp(orgPos, targetPos, t);

            // Adjust rotation to face the target on the y-axis only
            Vector3 lookDirection = (targetPos - gameObject.transform.position).normalized;
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(lookDirection.x, 0, lookDirection.z));
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, newRotation, t);
            // Adjust player camera to follow the NPC
            Vector3 playerLookDirection = (gameObject.transform.position - player.transform.position).normalized;
            Quaternion playerRotation =
                Quaternion.LookRotation(new Vector3(playerLookDirection.x, playerLookDirection.y, playerLookDirection.z));
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, playerRotation, t);
            RaycastHit hit;
            if (Physics.Raycast(gameObject.transform.position + new Vector3(0, 0.5f, 0), -gameObject.transform.up,
                    out hit,
                    50))
            {
                if (minDistance < hit.distance)
                {
                    gameObject.transform.localPosition = new Vector3(newPosition.x,
                        gameObject.transform.localPosition.y -
                        (hit.distance - 0.5f - (minDistance - 0.5f)), newPosition.z);
                }
                else if (minDistance > hit.distance)
                {
                    gameObject.transform.localPosition = new Vector3(newPosition.x,
                        gameObject.transform.localPosition.y + (minDistance - 0.5f) - (hit.distance - 0.5f),
                        newPosition.z);
                }
                else
                {
                    gameObject.transform.localPosition = new Vector3(newPosition.x,
                        gameObject.transform.localPosition.y, newPosition.z);
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        anim.SetBool("walk", false);
        yield return new WaitForSeconds(0.3f);

        elapsedTime = 0;
        duration = 2.5f;
        while (elapsedTime < duration / 2)
        {
            float t = elapsedTime / (duration / 2);

            // Adjust rotation to face the player on the y-axis only
            Vector3 lookDirection = (player.transform.position - gameObject.transform.position).normalized;
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(lookDirection.x, 0, lookDirection.z));
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, newRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        AP.playAudio(AP.clips[ON.index]);
        anim.SetBool("talk", true);
        yield return new WaitWhile(() => AP.auds.isPlaying);
        anim.SetBool("talk", false);
        
        
        

        
        CM.StartTimer(5);
    }
}