using UnityEngine;

public class PagesController : MonoBehaviour
{
    public GameObject NextPage;
    public GameObject CurrentPage;

    public void GoToNextPage()
    {
        if (NextPage != null)
            NextPage.SetActive(true);
        if (CurrentPage != null)
            CurrentPage.SetActive(false);
    }
}