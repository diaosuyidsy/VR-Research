using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// public class Controller : MonoBehavior
// Controller class that uses a traditional joystick to move player
// Movement is relative to player, left thumbstick is used for input
public class Controller : MonoBehaviour {

	// Player - Position of base player object (ie. OVRCameraRig)
    public Transform Player;

	// Facing - Transform to get forward facing position of player
    public Transform Facing;

	// Speed - Constant speed
    public float Speed = 0.1f;

	// Height - Constant height
	public float Height = 1.5f;

	// Step - Max height increase
	public float Step = 0.5f;

	// PrevHeight - Previous height
	private float prevHeight;

	// Use this for initialization
	void Start () {
		Height += Terrain.activeTerrain.transform.position.y;
	}


	// Update is called once per frame
	void Update () {

		prevHeight = Player.position.y;

		//Get thumbstick x and y position
        Vector2 newPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);


        if (newPosition.x != 0 || newPosition.y != 0)
        {
			//get player facing position
            Vector3 dir = Facing.forward;

			//Get relative to environment
            dir = Player.InverseTransformDirection(dir);
            dir.y = 0;
            dir.Normalize();

			//adjust x and y position
			Vector3 temp = new Vector3(Player.position.x, Player.position.y, Player.position.z);
            temp += dir * newPosition.y * Speed;
            temp += newPosition.x * Facing.right * Speed;
            //Vector3 temp = new Vector3(Player.position.x, 0f, Player.position.z);

			//adjust height if terrain changes
			if (Terrain.activeTerrain.SampleHeight (temp) + Height <= prevHeight + Step
			   && Terrain.activeTerrain.SampleHeight (temp) + Height >= prevHeight - Step) {
				temp.y = Terrain.activeTerrain.SampleHeight (temp) + Height;
				//set player position
				Player.position = temp;
			}
        }
    }
}
