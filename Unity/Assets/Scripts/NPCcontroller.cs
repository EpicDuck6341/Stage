using System.Collections;
using UnityEngine;

public class NPCcontroller : MonoBehaviour
{
    public GameObject player;
    public GameObject cam;
    public Animator anim;
    private float minDistance;
    private CountdownManager CM;
    private NPCVoiceLines NPCV;

    private static readonly int Walk = Animator.StringToHash("walk");

    // Start is called before the first frame update
    void Start()
    {
        NPCV = GameObject.Find("NPCVoiceLines").GetComponent<NPCVoiceLines>();
        CM = GameObject.Find("CountdownManager").GetComponent<CountdownManager>();
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position + new Vector3(0, 0.5f, 0), -gameObject.transform.up, out hit,
                50))
        {
            minDistance = hit.distance;
        }
    }


    public IEnumerator MoveNPC(float duration, Vector3 target,GameObject obj)
    {
        Vector3 orgPos = gameObject.transform.position;
        Vector3 targetPos = target;
        float minimum = 1.5f;
        player.transform.rotation = Quaternion.Euler(0,0,0);
        cam.transform.rotation = Quaternion.Euler(0,0,0);
        anim.SetBool(Walk, true);


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
            player.transform.LookAt(new Vector3(gameObject.transform.localPosition.x,
                gameObject.transform.localPosition.y + 0.04f, gameObject.transform.localPosition.z));
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

        anim.SetBool(Walk, false);
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
        
        NPCV.playAudio(11);
        anim.SetBool("talk", true);
        yield return new WaitWhile(() => NPCV.aud.isPlaying);
        anim.SetBool("talk", false);
        player.transform.rotation = Quaternion.Euler(0,0,0);
        cam.transform.rotation = Quaternion.Euler(0,0,0);
        player.transform.LookAt(new Vector3(obj.transform.position.x,obj.transform.position.y-0.5f,obj.transform.position.z));
        yield return new WaitForSeconds(5f);
        CM.StartTimer(5);
    }
}