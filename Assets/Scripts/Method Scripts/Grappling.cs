using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Grappling
 *
 * Uses the Left Controller to point and shoot a grappling hook at a target location.
 * As the trigger is held down, the Player will move toward the target location
 * in a smoothed motion.
 */
public class Grappling : MonoBehaviour
{
    public Transform Player;
    // This is the button to activate the Grappling
    public OVRInput.Button button = OVRInput.Button.PrimaryIndexTrigger;
    // Sprite for the target indicator
    public GameObject target;
    public RaycastHit hit;

    public LayerMask cullingmask;
    // Max distance to allow grappling
    public int maxDistance;

    public bool isMoving;
    public Vector3 loc;
    // Speed of motion
    public float speed = 1f;
    // Height of the Player
    public float Height = 1.5f;
    public GameObject Indicator;

    public LineRenderer LR;
    // The lower this number is the fast you move, the higher the number the more motion is smoothed
    public float smoothTime = 0.3F;
    private float xVelocity = 0.0F;
    private float yVelocity = 0.0F;
    private float zVelocity = 0.0F;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Cast a ray in the direction of the controller
        Ray ray = new Ray(target.transform.position, target.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            // Only move when the target is on the floor
            if (hit.collider.tag == "Floor")
            {
                Indicator.SetActive(true);
                if (!isMoving)
                {
                    Indicator.transform.position = hit.point;
                }
            }
        }
        else    // target out of range
        {
            Indicator.SetActive(false);
        }

        if (OVRInput.GetDown(button))
        {
			LR.SetPosition (0, target.transform.position);
            Findspot(ray, hit);
        }

        if (isMoving)
            Moving();


    }

    // Find the target location
    public void Findspot(Ray ray, RaycastHit hit)
    {
        if (Indicator.activeSelf)
        {
			LR.enabled = true;
            isMoving = true;
            loc = hit.point;
			LR.SetPosition(1, loc);
            loc = new Vector3(loc.x, loc.y + Height, loc.z);
        }
        if (Physics.Raycast(target.transform.position, target.transform.forward, out hit, maxDistance))
        {
            if (hit.collider.tag == "Floor")
            {
                isMoving = true;
                loc = hit.point;
				LR.SetPosition(1, loc);
                loc = new Vector3(loc.x, loc.y + Height, loc.z);
            }
            //LR.enabled = true;
        }
    }

    // Move the Player
    public void Moving()
    {
        // uses Mathf.SmoothDamp to smooth the motion of the Player using the smoothTime variable
        float newX = Mathf.SmoothDamp(Player.position.x, loc.x, ref xVelocity, smoothTime);
        float newY = Mathf.SmoothDamp(Player.position.y, loc.y, ref yVelocity, smoothTime);
        float newZ = Mathf.SmoothDamp(Player.position.z, loc.z, ref zVelocity, smoothTime);
        Player.position = new Vector3(newX, newY, newZ);
		LR.SetPosition(0, target.transform.position);
        // stop motion when the Player is within 0.5f of the target location or when the button is released
        if (Vector3.Distance(transform.position, loc) < 0.5f || !OVRInput.Get(button))
        {
            isMoving = false;
            LR.enabled = false;
        }
    }
}
