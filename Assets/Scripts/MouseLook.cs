using System;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
	public Transform player_hand;
	public Transform playerhand_cube;
	// This is the button to activate the Calculate Result
	public OVRInput.Button button = OVRInput.Button.PrimaryIndexTrigger;
	public OVRInput.Button blankScreenButton;
	public OVRInput.Button forwardButton;
	public OVRInput.Button backwardButton;
	/*	public float mouseSensitivity = 100.0f;
	public float clampAngle = 80.0f;


	private float rotY = 0.0f;
	// rotation around the up/y axis
	private float rotX = 0.0f;
	// rotation around the right/x axis

	void Start ()
	{
		Vector3 rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;
	}*/

	void Update ()
	{
/*		float mouseX = Input.GetAxis ("Mouse X");
		float mouseY = -Input.GetAxis ("Mouse Y");

		rotY += mouseX * mouseSensitivity * Time.deltaTime;
		rotX += mouseY * mouseSensitivity * Time.deltaTime;

		rotX = Mathf.Clamp (rotX, -clampAngle, clampAngle);

		Quaternion parentRotation = Quaternion.Euler (0f, rotY, 0f);
		transform.parent.rotation = parentRotation;

		Quaternion localRotation = Quaternion.Euler (rotX, transform.rotation.eulerAngles.y, 0.0f);
		transform.rotation = localRotation;*/
		if (OVRInput.GetUp (forwardButton)) {
			if (GameManager.GM.markerPointer >= GameManager.GM.maxPathLength - 1)
				return;
			foreach (GameObject r in GameObject.FindGameObjectsWithTag ("Mark")) {
				r.SetActive (false);
			}
			GameManager.GM.nextPoint (true);

		}
		if (OVRInput.GetUp (backwardButton)) {
			GameManager.GM.goBackwards ();
		}
		if (OVRInput.GetDown (blankScreenButton)) {
			GameManager.GM.BlackScreenEnclosure.SetActive (!GameManager.GM.BlackScreenEnclosure.activeSelf);
		}
		if (GameManager.GM.PlayerActionLock && !GameManager.GM.ClickedOnce) {
			if (OVRInput.GetDown (button)) {
				GameManager.GM.ClickedOnce = true;
				GameManager.GM.BlackScreenEnclosure.SetActive (false);
				GameManager.GM.CalculateResult (player_hand, playerhand_cube);
			}
		}
	}
		
}
