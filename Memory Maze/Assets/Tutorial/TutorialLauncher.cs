using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLauncher : MonoBehaviour
{
    private void Start()
    {
        if(!MazeLoader.IsTutorial) gameObject.SetActive(false);
    }
}
