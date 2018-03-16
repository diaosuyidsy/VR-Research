using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * Allows the player to teleport to a target location.
 * Controlled with the left controller and left index trigger by default.
 * Hold down trigger to designate target location, release to teleport.
 */

public class Teleporter : MonoBehaviour
{

    // Target - Sprite to illustrate target location
    public GameObject Target;
    // Player - Position of base player object (ie. OVRCameraRig)
    public Transform Player;
    // RayLength - Range of teleportation
    public float RayLength = 20f;
    // button - Button to be pressed to teleport
    public OVRInput.Button button = OVRInput.Button.PrimaryIndexTrigger;
    // Height - Height of the player
    public float Height = 2.0f;
    // Controller - Controller from which the ray will be cast to teleport
    public GameObject Controller;
    // Line - Draws a line from Controller to Target
    public LineRenderer Line;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
		// Cast out a ray from the controller to see where it is pointing
        Ray ray = new Ray(Controller.transform.position, Controller.transform.forward);
        RaycastHit hit;
		// Check if the button is pressed and the ray hit an object
		// If so, make the line renderer visible
        if (Physics.Raycast(ray, out hit, RayLength) &&
            OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            Line.enabled = true;
            Line.SetPosition(0, Controller.transform.position);
            Line.SetPosition(1, hit.point);
			// Check if the object the ray hit is the ground
            if (hit.collider.tag == "Floor")
            {
				// If it is, make the target visible to visualize the target location
                if (!Target.activeSelf)
                {
                    Target.SetActive(true);
                }
                Target.transform.position = hit.point;
            }
            else
            {
                Target.SetActive(false);
            }
        }
		// Check if the ray hit an object AND the button was just RELEASED
        else if (Physics.Raycast(ray, out hit, RayLength) &&
          OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {
			// Check if the object the ray hit is the ground
            if (hit.collider.tag == "Floor")
            {
				// Move the player to that position
                Vector3 targetPosition = Target.transform.position;
				Player.position = new Vector3(targetPosition.x, targetPosition.y + Height, targetPosition.z); 	
                Target.SetActive(false);
                Line.enabled = false;
            }
        }
		// Make the target and line renderer invisible
        else
        {
            Target.SetActive(false);
            Line.enabled = false;
        }
    }
}
