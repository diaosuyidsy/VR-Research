using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
JetPack
This class allows the user to navigate a system using head movements, tilting of the left
controller. If the user holds down the left trigger the movement will activate and and tilting the
left controller accelerates movement forward and upward (like a jetpack) and release kills the jetpack engines
and brings the user back to the ground
*/

public class JetPack : MonoBehaviour
{
    // The left trigger finter
    public OVRInput.Button button = OVRInput.Button.PrimaryIndexTrigger;

    public Transform Player;
    public Transform Facing;
    public float Speed = 0.1f;
    public float Height = 1.5f;

    // Variables to help calcualte the pitch
    private const float MaxAngle = 60f;
    private const float DeadZone = 0.2f;
    private const float PitchAngleOffset = 0.0f;

    // Jets would be used for animation in the future
    public GameObject Jets;
    public GameObject Controller;

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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float yaw = 0, pitch = 0, roll = 0;

        Quaternion rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTrackedRemote);
        CalculateYawPitchRoll(rotation, out yaw, out pitch, out roll);

        // If left trigger is pressed, begin movemenet
        // If the trigger is not being pressed, bring the user back to the ground if not already there
        if (OVRInput.Get(button))
        {

            Vector3 temp = new Vector3(Player.position.x, 0f, Player.position.z);
            temp.y = Terrain.activeTerrain.SampleHeight(temp) + Height;

            // Only start moving up if the user is above the ground
            if (Player.position.y >= temp.y)
            {
                MoveUp(pitch);
            }

        }
        else
        {
            Vector3 temp = new Vector3(Player.position.x, 0f, Player.position.z);
            temp.y = Terrain.activeTerrain.SampleHeight(temp) + Height;

            // If they are above the terrain, bring them back to earth
            if (Player.position.y > temp.y)
            {
                DownToEarth(pitch);
            }

            // If for some reason they are below the terrain, move them to the ground level immediately
            if (Player.position.y < temp.y)
            {
                // Back to earth
                Player.position = temp;
            }
        }
    }

    // A function to bring the user back down to the ground after the jetpack is killed
    // The pitch is used as the "speed" of the descent
    public void DownToEarth(float speed)
    {
        Vector3 dir = Facing.forward;
        dir = Player.InverseTransformDirection(dir);
        dir.Normalize();
        dir.y = 0;
        Player.position += dir * 1 * speed * .05f;
        Vector3 temp = new Vector3(Player.position.x, Player.position.y + -.03f, Player.position.z);
        Player.position = temp;
    }

    // A function to make the user fly (move up and foward)
    // The pitch is used as the "speed" of the ascent
    public void MoveUp(float speed)
    {
        Vector3 dir = Facing.forward;
        dir = Player.InverseTransformDirection(dir);
        dir.Normalize();
        Player.position += dir * 1 * speed * Speed;
        Vector3 temp = new Vector3(Player.position.x, Player.position.y, Player.position.z);
        Player.position = temp;
    }
}
