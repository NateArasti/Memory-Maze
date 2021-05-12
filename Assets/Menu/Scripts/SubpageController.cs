using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubpageController : MonoBehaviour
{
	public GameObject newSubpage;
	public GameObject[] otherSubpages;

	public void OpenSubpage()
	{
		if (otherSubpages != null)
			foreach (var subpage in otherSubpages)
				subpage.SetActive(false); 
		if (newSubpage != null)
			newSubpage.SetActive(true);
	}
}
