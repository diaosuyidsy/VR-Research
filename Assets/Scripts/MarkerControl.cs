using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerControl : MonoBehaviour
{

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
}
