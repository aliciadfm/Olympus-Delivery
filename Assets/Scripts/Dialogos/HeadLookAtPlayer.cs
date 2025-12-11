using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class HeadLookAtPlayer : MonoBehaviour
{
    public Transform headBone;
    private Transform player;
    public float turnSpeed = 5f;
    public float maxAngle = 60f;
    public float stopLookingDelay = 2f;
    private Coroutine stopLookCoroutine;
    private bool isLooking = false;

    void Start()
    {   
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void LateUpdate()
    {
        if (!isLooking || player == null) return;

        Vector3 dir = player.position - headBone.position;
        Quaternion targetRot = Quaternion.LookRotation(dir);
        Quaternion limitedRot = LimitRotation(headBone.rotation, targetRot, maxAngle);

        headBone.rotation = Quaternion.Slerp(
            headBone.rotation,
            limitedRot,
            Time.deltaTime * turnSpeed
        );
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

    public void StartLooking()
    {
        isLooking = true;

        if (stopLookCoroutine != null)
        {
            StopCoroutine(stopLookCoroutine);
            stopLookCoroutine = null;
        }
    }

    public void StopLookingWithDelay()
    {
        if (stopLookCoroutine != null)
            StopCoroutine(stopLookCoroutine);

        stopLookCoroutine = StartCoroutine(StopLookingAfterDelay());
    }

    private IEnumerator StopLookingAfterDelay()
    {
        yield return new WaitForSeconds(stopLookingDelay);
        isLooking = false;
        stopLookCoroutine = null;
    }
}

