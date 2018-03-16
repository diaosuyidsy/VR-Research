using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingArms : MonoBehaviour
{

    public OVRInput.Button walk = OVRInput.Button.PrimaryIndexTrigger;
    public GameObject lController;
    public GameObject rController;
    public GameObject player;
    public GameObject camera;

    private float maxPos = 0.0075f;
    private float maxNeg = -0.01f;
    private int frameUpdate = 0;
    public int smoothing = 1;
    public float height = 2f;
    public float scalar = 4f;
    private float maxSpeed = 0.12f;
    private float minSpeed = 0.005f;
    private float runSpeed = 0.15f;
    private float sprint = 0.15f;
    private bool sprintB = false;
    private float prevSpeed = 0f;
    private Vector3 leftPrevLocalPos;
    private Vector3 rightPrevLocalPos;
    private Vector3 dir;
    private float distance;
	private float prevHeight;
	public float Step = 0.5f;

    // Use this for initialization
    void Start()
    {
		height += Terrain.activeTerrain.transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		prevHeight = player.transform.position.y;
        frameUpdate++;
        // Used to smooth movement
        if (frameUpdate >= smoothing)
        {
            frameUpdate = 0;
            // All movement functionality goes here
            if (OVRInput.GetDown(walk))
            {
                // Start walking
                leftPrevLocalPos = lController.transform.position;
                rightPrevLocalPos = rController.transform.position;
                prevSpeed = 0f;
            }
            else if (OVRInput.Get(walk))
            {
                // Walk while button is pressed
                dir = camera.transform.forward;
                distance = GetDistance();
            }
        }
        if (OVRInput.GetUp(walk) || !OVRInput.Get(walk))
        {
            dir = new Vector3(0, 0, 0);
            distance = 0f;
            prevSpeed = 0f;
            sprintB = false;
        }
        dir = player.transform.InverseTransformDirection(dir);
        dir.y = 0;
        dir.Normalize();
		Vector3 temp = player.transform.position + distance * scalar * dir;;
		if (Terrain.activeTerrain.SampleHeight (temp) + height <= prevHeight + Step
			&& Terrain.activeTerrain.SampleHeight (temp) + height >= prevHeight - Step) {
			temp.y = Terrain.activeTerrain.SampleHeight (temp) + height;
			//set player position
			player.transform.position = temp;
		}
    }

    // Get the distance to move the player
    float GetDistance()
    {
        float oldDist = Vector3.Distance(leftPrevLocalPos, rightPrevLocalPos);
        float newDist = Vector3.Distance(lController.transform.position, rController.transform.position);
        float dist = Mathf.Abs(oldDist - newDist);
        leftPrevLocalPos = lController.transform.position;
        rightPrevLocalPos = rController.transform.position;
        //Debug.Log ("PreDistance: " + dist);
        sprintB = false;
        if (dist > sprint)
        {
            dist = runSpeed;
            sprintB = true;
            //Debug.Log ("Sprint");
        }
        else if (dist - prevSpeed > maxPos)
        {
            dist = prevSpeed + maxPos;
            //Debug.Log ("maxPos");
        }
        else if (dist - prevSpeed < maxNeg)
        {
            dist = prevSpeed + maxNeg;
            //Debug.Log ("maxNeg");
        }
        if (dist < minSpeed)
        {
            prevSpeed = dist;
            dist = 0;
            //Debug.Log ("Zero");
        }
        else if (!sprintB && dist > maxSpeed)
        {
            dist = maxSpeed;
            prevSpeed = dist;
            //Debug.Log ("maxSpeed");
        }
        else
        {
            prevSpeed = dist;
        }
        //prevSpeed = dist;
        //Debug.Log ("PostDistance: " + dist);
        return dist;
    }

    // Get the average distance of the controllers to move the player
    float GetAvgDistance()
    {
        float lDist = Vector3.Distance(leftPrevLocalPos, lController.transform.position);
        float rDist = Vector3.Distance(rightPrevLocalPos, rController.transform.position);
        float dist = (lDist + rDist) / 2;
        leftPrevLocalPos = lController.transform.position;
        rightPrevLocalPos = rController.transform.position;
        return dist;
    }
}
