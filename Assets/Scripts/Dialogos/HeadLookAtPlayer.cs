using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class HeadLookAtPlayer : MonoBehaviour
{
    public Transform headBone;
    private Transform player;
    public float turnSpeed = 5f;
    public float maxAngle = 60f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float stopLookingAfterSeconds = 30f;
    private Coroutine stopLookCoroutine;
    private bool isTalking = false;

    void Start()
    {   
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void LateUpdate()
    {
        if(player.IsUnityNull())
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null) 
            {
                player = playerObj.transform;
            }
        }
        if (!isTalking) return;

        // Dirección hacia el jugador
        Vector3 dir = player.position - headBone.position;
        Quaternion targetRot = Quaternion.LookRotation(dir);

        // LIMITAR ángulos para que no gire el cuello demasiado
        Quaternion limitedRot = LimitRotation(headBone.rotation, targetRot, maxAngle);

        // Interpolación suave
        headBone.rotation = Quaternion.Slerp(headBone.rotation, limitedRot, Time.deltaTime * turnSpeed);
    }

    Quaternion LimitRotation(Quaternion current, Quaternion target, float maxAngle)
    {
        float angle = Quaternion.Angle(current, target);

        if (angle > maxAngle)
        {
            return Quaternion.Slerp(current, target, maxAngle / angle);
        }

        return target;
    }


public void StartDialogue()
{
    isTalking = true;

    if (stopLookCoroutine != null)
    {
        StopCoroutine(stopLookCoroutine);
        stopLookCoroutine = null;
    }
}

public void EndDialogue()
{
    if (stopLookCoroutine != null)
        StopCoroutine(stopLookCoroutine);

    stopLookCoroutine = StartCoroutine(StopLookingAfterDelay());
}

private IEnumerator StopLookingAfterDelay()
{
    yield return new WaitForSeconds(stopLookingAfterSeconds);
    isTalking = false;
    stopLookCoroutine = null;
}



}

