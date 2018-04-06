using System;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
	public Transform player_hand;
	public Transform playerhand_cube;
	// This is the button to activate the Calculate Result
	public OVRInput.Button button = OVRInput.Button.PrimaryIndexTrigger;
	float timer = 0.5f;
	bool startTimer = false;

	void Update ()
	{
		if (startTimer) {
			timer -= Time.deltaTime;
			if (timer <= 0f) {
				startTimer = false;
			}
				
		}
		// If get an input right arrow, advance to the next scene
		if (Input.GetKeyUp (KeyCode.RightArrow)) {
			GameManager.GM.changeScene (1);
		}
		// If get an input left arrwo, devance to the last scene
		if (Input.GetKeyUp (KeyCode.LeftArrow)) {
			GameManager.GM.changeScene (-1);
		}

		if (Input.GetKeyUp (KeyCode.Space)) {
			GameManager.GM.BlackScreenEnclosure.SetActive (!GameManager.GM.BlackScreenEnclosure.activeSelf);
		}
		if (GameManager.GM.PlayerActionLock && !GameManager.GM.ClickedOnce && timer <= 0f) {
			if (OVRInput.GetDown (button)) {
				timer = 1f;
				GameManager.GM.ClickedOnce = true;
				GameManager.GM.BlackScreenEnclosure.SetActive (false);
				GameManager.GM.CalculateResult (player_hand, playerhand_cube);
			}
		}
	}

	public void OnStartTimer ()
	{
		startTimer = true;
	}
}
