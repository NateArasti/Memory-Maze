using UnityEngine;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
            other.transform.GetComponent<PlayerUI>().Win();
    }

}
