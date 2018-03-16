using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class PinchZoom : MonoBehaviour
// Movement that emulates the pinch to zoom feature of touch interfaces
// User sets their max position with hand trigger, then holds down left trigger
// To zoom in and out of a set location
public class PinchZoom : MonoBehaviour
{
    // Walk - status of button for zooming
    public OVRInput.Button walk = OVRInput.Button.PrimaryIndexTrigger;

    // setDist - button to set distance between controllers
    public OVRInput.Button setDist = OVRInput.Button.PrimaryHandTrigger;

	// lController - left controller values
    public GameObject lController;

	// rController - right controller values
    public GameObject rController;

    //keeps track of view and desired location
    public GameObject target;
    public GameObject Indicator;

    // player - player object
    public GameObject player;
    
	// hit - raycast to get point to oscillate from
    public RaycastHit hit;

	// maxDistance - maxDistance of raycast
    public int maxDistance;

	// maxControl - max distance between controller
    public float maxControl;

	// isMoving - value to keep track of movement
    public bool isMoving;

	// loc - location of raycast hit
    public Vector3 loc;

	//player location
    public Vector3 startLoc;

	// Height - set height
    public float Height = 1.5f;


    private Vector3 leftPrevLocalPos;
    private Vector3 rightPrevLocalPos;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

		//hit raycast at point player is looking at
        Ray ray = new Ray(target.transform.position, target.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.tag == "Floor")
            {
                Indicator.SetActive(true);
                if (!isMoving)
                {
                    Indicator.transform.position = hit.point;
                }
            }
        }
        else
        {
            Indicator.SetActive(false);
        }


        //find spot
        if (OVRInput.GetDown(walk))
        {
            Findspot(ray, hit);
        }

		//if true move
        if (isMoving)
        {
            MovePlayer();
        }

		//calibrate distance between two controllers
        if (OVRInput.Get(setDist))
        {
            callibrate();
        }


    }

	//Find spot that the raycast hit
    public void Findspot(Ray ray, RaycastHit hit)
    {
        if (Indicator.activeSelf)
        {
            isMoving = true;
            loc = hit.point;
            loc = new Vector3(loc.x, loc.y + Height, loc.z);
            startLoc = player.transform.position;
        }

    }

    //move based on distance between the controllers and your set target spot
    public void MovePlayer()
    {
        float oldDist = Vector3.Distance(leftPrevLocalPos, rightPrevLocalPos);
        float curDist = Vector3.Distance(lController.transform.position, rController.transform.position);

        float distProp = curDist / maxControl;

        player.transform.position = Vector3.Lerp(startLoc, loc, distProp);

        if (!OVRInput.Get(walk))
        {
            isMoving = false;
        }

        

    }

	//callibrate max controller distance to be between the two controller distances
    public void callibrate()
    {
        maxControl = Vector3.Distance(lController.transform.position, rController.transform.position);
    }

}
