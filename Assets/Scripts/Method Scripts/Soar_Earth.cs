using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soar_Earth : MonoBehaviour {
	public Transform Player;
	public Transform Facing;
	public float Speed = 0.25f;
	public float Height = 1.5f;

	public float jumpHeight = 100f;

	public OVRInput.Button upButton = OVRInput.Button.PrimaryIndexTrigger;


	void Start() {}

	void Update(){
		if (OVRInput.GetDown(upButton)) {
			Vector3 jump = Player.position;
			jump.y += jumpHeight;
			Player.position = jump;
		}

		Vector2 newPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
		if (newPosition.x != 0 || newPosition.y != 0)
		{
			Vector3 dir = Facing.forward;
			dir = Player.InverseTransformDirection(dir);
			dir.Normalize();
			Player.position += dir * newPosition.y * Speed;
			Player.position += newPosition.x * Facing.right * Speed;

			if (Player.position.y < Terrain.activeTerrain.SampleHeight (Player.position)) {
				Vector3 temp = new Vector3 (Player.position.x, 0f, Player.position.z);
				temp.y = Terrain.activeTerrain.SampleHeight (temp) + Height;
				Player.position = temp;
				Speed = 0.25f;
			} else {
				Speed = 0.25f;
			}
			
		}
	
	
	}

}