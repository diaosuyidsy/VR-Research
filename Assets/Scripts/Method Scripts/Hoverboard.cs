using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
Hoverboard
This class allows the user to navigate a system using head movements and the tilting of the left
controller. If the user holds down the left trigger the movement will activate and and tilting the
left controller accelerates movement forward (tilting backwards accelerates movement backwards)
*/

public class Hoverboard : MonoBehaviour {

	// The left trigger finter
	public OVRInput.Button button = OVRInput.Button.PrimaryIndexTrigger;

	public Transform Player;
	public Transform Facing;
	public float Speed = 0.1f;
	public float Height = 1.5f;
	public float Step = 0.5f;
	private float prevHeight;

	// Variables to help calculate the pitch
	private const float MaxAngle = 60f;
	private const float DeadZone = 0.2f;
	private const float PitchAngleOffset = 0.0f;

	// Function to help you calculate the Yaw, Pitch and Roll from the controller rotation (Quaternion)
	// The Yaw and Roll are not used, but kept in here in case other functionality is implemented
	public static void CalculateYawPitchRoll(Quaternion rotation, out float yaw, out float pitch, out float roll)
	{
		Vector3 forward = rotation * Vector3.forward;

		var pitchProj = Vector3.ProjectOnPlane(forward, Vector3.right);

		if (pitchProj.sqrMagnitude < 0.001f)
		{
			forward = rotation * (forward.x > 0 ? Vector3.left : Vector3.right);
			pitchProj = Vector3.ProjectOnPlane(forward, Vector3.right);
		}

		#if DRAW_DEBUG_LINES
		Debug.DrawLine(Vector3.zero, forward * 3, Color.blue);
		#endif

		float pitchAngle = Mathf.Atan2(pitchProj.y, pitchProj.z);

		// cancel out pitch
		rotation = Quaternion.Euler(pitchAngle * Mathf.Rad2Deg, 0, 0) * rotation;

		forward = rotation * Vector3.forward;

		#if DRAW_DEBUG_LINES
		Debug.DrawLine(Vector3.zero, forward * 3, Color.red);
		#endif

		var rollProj = Vector3.ProjectOnPlane(forward, Vector3.up);

		float rollAngle = Mathf.Atan2(rollProj.x, rollProj.z);

		// cancel out roll
		rotation = Quaternion.Euler(0, -rollAngle * Mathf.Rad2Deg, 0) * rotation;

		Vector3 up = rotation * Vector3.up;

		#if DRAW_DEBUG_LINES
		Debug.DrawLine(Vector3.zero, up * 3, Color.green);
		#endif

		float yawAngle = Mathf.Atan2(up.x, up.y);

		const float MaxYawAngle = 150f;
		// if yaw is greater than MaxYawAngle degrees, it means this is 
		// actually probably a roll > 90 degrees.
		// go back and fix our numbers for yaw, pitch and roll.
		if (Mathf.Abs(yawAngle) > MaxYawAngle * Mathf.Deg2Rad)
		{
			pitchAngle += (pitchAngle < 0 ? Mathf.PI : -Mathf.PI);
			rollAngle = (Mathf.Sign(rollAngle) * Mathf.PI - rollAngle);
			yawAngle = (Mathf.Sign(yawAngle) * Mathf.PI - yawAngle);
		}

		// convert angles into -1, 1 axis range, apply dead zone
		pitch = Mathf.Clamp((PitchAngleOffset - pitchAngle * Mathf.Rad2Deg) / MaxAngle, -1, 1);
		if (Mathf.Abs(pitch) < DeadZone)
			pitch = 0;

		roll = Mathf.Clamp(rollAngle * Mathf.Rad2Deg / MaxAngle, -1, 1);
		if (Mathf.Abs(roll) < DeadZone)
			roll = 0;

		yaw = Mathf.Clamp(-yawAngle * Mathf.Rad2Deg / MaxAngle, -1, 1);
		if (Mathf.Abs(yaw) < DeadZone)
			yaw = 0;
	}

	// Use this for initialization
	void Start () {
		Height += Terrain.activeTerrain.transform.position.y;
	}

	// Update is called once per frame
	void Update () {
		prevHeight = Player.position.y;

		float yaw = 0, pitch = 0, roll = 0;

		// If left trigger is pressed, start calculations and update user position
		if (OVRInput.Get (button)) {
			Quaternion rotation = OVRInput.GetLocalControllerRotation (OVRInput.Controller.LTrackedRemote);
			CalculateYawPitchRoll(rotation, out yaw, out pitch, out roll);

			Moving (pitch);
		}
			

	}

	// Function to actually move the player, using the pitch as the new acceleration speed
	// If speed (pitch) is greater than 0 move foward, less than 0 move backward, 0 no movement
	public void Moving(float speed)
	{
		if (speed != 0.0){
		//if (speed > 0.0) {
			Vector3 dir = Facing.forward;
			dir = Player.InverseTransformDirection(dir);
			dir.y = 0;
			dir.Normalize();
			Vector3 temp = Player.position + dir * 1 * speed * Speed;
			//Player.position += dir * 1 * speed * Speed;
			//Vector3 temp = new Vector3(Player.position.x, 0f, Player.position.z);
			if (Terrain.activeTerrain.SampleHeight (temp) + Height <= prevHeight + Step
				&& Terrain.activeTerrain.SampleHeight (temp) + Height >= prevHeight - Step) {
				temp.y = Terrain.activeTerrain.SampleHeight (temp) + Height;
				//set player position
				Player.position = temp;
			}
		} 

		/*if (speed < 0.0) {
			/*Vector3 dir = Facing.forward;
			dir = Player.InverseTransformDirection(dir);
			dir.y = 0;
			dir.Normalize();
			Vector3 temp = Player.position + dir * 1 * speed * Speed;
			//Player.position += dir * 1 * speed * Speed;
			//Vector3 temp = new Vector3(Player.position.x, 0f, Player.position.z);
			temp.y = Terrain.activeTerrain.SampleHeight (temp) + Height;
			Player.position = temp;
		}*/ 

		if (speed == 0.0) {
			Debug.Log("We have stopped");
		}
	}
}
