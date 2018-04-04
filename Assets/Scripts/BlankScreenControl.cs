using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlankScreenControl : MonoBehaviour
{
	public CanvasGroup myCG;
	private bool flash = false;


	// Update is called once per frame
	void Update ()
	{
		if (flash) {
			myCG.alpha = myCG.alpha - (Time.deltaTime * 0.4f);
			if (myCG.alpha <= 0) {
				myCG.alpha = 0;
				flash = false;
			}
		}
	}

	public void onBlankScreen ()
	{
		flash = true;
		myCG.alpha = 1;
	}
}
