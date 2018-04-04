using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerControl : MonoBehaviour
{
	public Color red;
	public Color blue;
	public Color yellow;
	public GameObject[] Godrayz;

	void OnCollisionEnter (Collision coll)
	{
		if (coll.collider.tag == "Player") {
			// Player enter marker, proceed to next marker
			GameManager.GM.nextPoint (false);
			gameObject.SetActive (false);
		}
	}

	void OnTriggerEnter (Collider coll)
	{
		if (coll.tag == "Player") {
			// Player enter marker, proceed to next marker
			GameManager.GM.nextPoint (false);
			gameObject.SetActive (false);
		}
	}

	void OnEnable ()
	{
		changeColor ();
	}

	void changeColor ()
	{
		Color thisColor = new Color ();
		if (GameManager.GM.markerPointer % 3 == 0) {
			thisColor = red;
		} else if (GameManager.GM.markerPointer % 3 == 1) {
			thisColor = blue;
		} else {
			thisColor = yellow;
		}
		foreach (GameObject grz in Godrayz) {
			grz.GetComponent<MeshRenderer> ().material.SetColor ("_TintColor", thisColor);
		}
	}
}
