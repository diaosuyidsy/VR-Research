using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public OVRInput.Button toggle;
	public Text timer;
	private decimal display;
	private bool ticking = false;
	private float Begin = 0;
	private float Current = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (ticking) {
			Current = Time.time - Begin;
		}
		if (OVRInput.GetDown (toggle) && !ticking) {
			Begin = Time.time;
			ticking = true;
		} else if (OVRInput.GetDown (toggle)) {
			ticking = false;
		}
		display = (decimal)Mathf.Round(Current);
		timer.text = display + " seconds";
	}
}
