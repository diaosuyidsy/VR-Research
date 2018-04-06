using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * SkiPole
 * 
 * Uses the Hand Trigger on both controllers to trigger movement.
 * Use the controllers to pull yourself toward a direction, relative to your camera.
 * You can pull with both hands for faster movement.
 */
public class SkiPole : MonoBehaviour
{
	// specify controllers and buttons used
	public OVRInput.Button walk = OVRInput.Button.PrimaryHandTrigger;
	public OVRInput.Controller lHand = OVRInput.Controller.LTouch;
	public OVRInput.Controller rHand = OVRInput.Controller.RTouch;
	public GameObject lController;
	public GameObject rController;
	public GameObject player;
	public GameObject camera;

	// Player height
	public float height = 2f;
	// store the previous left and right velocities
	private Vector3 lPrev;
	private Vector3 rPrev;

	private Vector3 vel;

	public float Step = 0.5f;
	public float MovementBuffer = 1f;
	private float prevHeight;

	// Use this for initialization
	void Start ()
	{
		height += Terrain.activeTerrain.transform.position.y;
		lPrev = lController.transform.position;
		rPrev = rController.transform.position;
	}

	// get the change in position for the left controller
	Vector3 lDelta ()
	{
		return (lPrev - lController.transform.position) * MovementBuffer;
	}

	// get the change in position for the right controller
	Vector3 rDelta ()
	{
		return (rPrev - rController.transform.position) * MovementBuffer;
	}

	// FixedUpdate is called once per frame
	void FixedUpdate ()
	{
		if (GameManager.GM.PlayerActionLock)
			return;
		prevHeight = player.transform.position.y;
		// All movement functionality goes here
		if (OVRInput.GetDown (walk, lHand) || OVRInput.GetDown (walk, rHand)) {
			// Start walking and set the previous positions
			lPrev = lController.transform.position;
			rPrev = rController.transform.position;
		}
        // if either Hand Trigger is pressed down
        else if (OVRInput.Get (walk, lHand) || OVRInput.Get (walk, rHand)) {
			// Walk while button is pressed
			if (OVRInput.Get (walk, lHand) && OVRInput.Get (walk, rHand)) {
				// find left and right velocities
				Vector3 lVel = lDelta () / Time.fixedDeltaTime;
				Vector3 rVel = rDelta () / Time.fixedDeltaTime;
				// move the Player with the faster controller
				if (lVel.magnitude >= rVel.magnitude) {
					vel = lVel;
				} else {
					vel = rVel;
				}
			} else if (OVRInput.Get (walk, lHand)) {
				vel = lDelta () / Time.fixedDeltaTime;
			} else {
				vel = rDelta () / Time.fixedDeltaTime;
			}
			// fix the y position
			vel.y = 0;
			// move the player to the new position using the velocity
			player.transform.position += vel;
			lPrev = lController.transform.position;
			rPrev = rController.transform.position;
		}
		if (OVRInput.GetUp (walk, lHand) || OVRInput.GetUp (walk, rHand) || !OVRInput.Get (walk, lHand) || !OVRInput.Get (walk, rHand)) {
			// stop walking
			lPrev = lController.transform.position;
			rPrev = rController.transform.position;
		}
		vel.y = 0;
		Vector3 temp = new Vector3 (player.transform.position.x, player.transform.position.x, player.transform.position.z);
		// move the Player's height based on the terrain
		if (Terrain.activeTerrain.SampleHeight (temp) + height <= prevHeight + Step
		    && Terrain.activeTerrain.SampleHeight (temp) + height >= prevHeight - Step) {
			temp.y = Terrain.activeTerrain.SampleHeight (temp) + height;
			//set player position
			player.transform.position = temp;
		}
		vel = Vector3.zero;
	}
}
