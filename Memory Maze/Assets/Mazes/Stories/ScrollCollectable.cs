using UnityEngine;

public class ScrollCollectable : MonoBehaviour
{
	public int length;

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player")) return;
		other.GetComponent<PlayerUI>().CollectStory(length);
		Destroy(gameObject);
	}
}