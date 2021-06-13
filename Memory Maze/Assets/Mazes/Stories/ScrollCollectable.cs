using UnityEngine;

public class ScrollCollectable : MonoBehaviour
{
    public int Length;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        other.GetComponent<PlayerUI>().CollectStory(Length);
        Destroy(gameObject);
    }
}
