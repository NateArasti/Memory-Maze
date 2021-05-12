using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayController : MonoBehaviour
{
	public GameObject overlay;

	public void ShowOverlay()
	{
		if (overlay != null)
			overlay.SetActive(true);
	}
	
	public void CloseOverlay()
	{
		if (overlay != null)
			overlay.SetActive(false);
	}
}
