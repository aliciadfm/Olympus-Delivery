using UnityEngine;

public class HammerTrigger : MonoBehaviour
{
    [SerializeField] private GameObject hammer;
    [SerializeField] private GameObject hammer2;
    [SerializeField] private GameObject characterBefore;
    [SerializeField] private GameObject characterAfter;
    [SerializeField] private GameObject mobile;
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (hammer) hammer.SetActive(false);
	if (hammer) hammer2.SetActive(true);
        if (characterBefore) characterBefore.SetActive(false);
        if (characterAfter) characterAfter.SetActive(true);
        if (mobile) mobile.SetActive(true);
    }
}
