using System;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
	public float mouseSensitivity = 100.0f;
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
	}

	void Update ()
	{
		float mouseX = Input.GetAxis ("Mouse X");
		float mouseY = -Input.GetAxis ("Mouse Y");

		rotY += mouseX * mouseSensitivity * Time.deltaTime;
		rotX += mouseY * mouseSensitivity * Time.deltaTime;

		rotX = Mathf.Clamp (rotX, -clampAngle, clampAngle);

		Quaternion parentRotation = Quaternion.Euler (0f, rotY, 0f);
		transform.parent.rotation = parentRotation;

		Quaternion localRotation = Quaternion.Euler (rotX, transform.rotation.eulerAngles.y, 0.0f);
		transform.rotation = localRotation;

		if (GameManager.GM.PlayerActionLock && !GameManager.GM.ClickedOnce) {
			if (Input.GetMouseButtonUp (0)) {
				GameManager.GM.ClickedOnce = true;
				GameManager.GM.CalculateResult (transform.parent, transform.parent.GetChild (2));
			}
		}
	}
}
