using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;


public class PlayerMovement : MonoBehaviour {



    const float GRAVITY = 7f;

    CharacterController player;
    Rigidbody body;

    public float MoveSpeed
    {
        get {
            targetVelocity.y = 0f;
            return targetVelocity.magnitude;
        }
    }
    public float lookSensitivityX = .001f;
    public float lookSensitivityY = .001f;

    float xAxisClamp;

    GamePadState state;
    GamePadState prevState;

    // Movement Behavior Variables
    public float verticalVelocity;
    public float VerticalVelocity
    {
        get { return verticalVelocity; }
        set { verticalVelocity = value; }
    }

    float rotX;
    float rotY;
    // Player variables
    public float maxSpeed;
    float speed;

    // Animation Variables
    bool jumped = false;
    public bool Jumped
    {
        get { return jumped; }

    }

    int dir = 0;
    public int AnimDir
    {

        get { return dir; }

    }

    int turnDir = 0;
    public int TurnDir
    {

        get { return turnDir; }

    }

    #region rigidbody movement WIP

    public Vector3 targetVelocity;
    public Vector3 velocity;
    public Vector3 velocityDelta;
    Vector3 gravityForce = new Vector3(0, -GRAVITY, 0);
    public bool isGrounded;
    public bool IsGrounded
    {
        get { return isGrounded; }
    }

    float jumpHeight = 1.2f;
    float jumpSpeed;
    float downDist;
    float sideDist;
    Vector3 raySpawn;



    #endregion

    // Use this for initialization
    void Start ()
    {
        player = GetComponent<CharacterController>();
        body = GetComponent<Rigidbody>();
        downDist = GetComponent<Collider>().bounds.extents.y + 0.02f;
        sideDist = GetComponent<Collider>().bounds.extents.x;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {      
        state = GetComponent<PlayerInput>().State;
        prevState = GetComponent<PlayerInput>().PrevState;
        isGrounded = CheckGrounded();
        body.isKinematic = false;

        if (GameManagerScript.instance.currentGameState == GameState.InGame && GetComponent<PlayerInput>().PlayerIndexSet)
        {
            speed = maxSpeed;

            RotateCamera();
            if (isGrounded)
            {
                JumpBody();
            }

                         
            MoveBody();
            //Gravity
            body.AddForce(gravityForce,ForceMode.Acceleration);
        }
        else
            body.isKinematic = true;
    }

    private void RotateCamera()
    {
        // Rotate the player
        // Look axis
        if (state.ThumbSticks.Right.X > .2 || state.ThumbSticks.Right.X < -.2)
        {
            rotX = state.ThumbSticks.Right.X * lookSensitivityX;
        }
        else
        {
            rotX = 0;
        }
        if (state.ThumbSticks.Right.Y > .2 || state.ThumbSticks.Right.Y < -.2)
        {
            rotY = -state.ThumbSticks.Right.Y * lookSensitivityY;
        }
        else
        {
            rotY = 0;
        }

        turnDir = 0;
        if (rotX > 0)
            turnDir = 1;
        else if (rotX < 0)
            turnDir = -1;

        // Adding an xAxis
        xAxisClamp += rotY;

        // Set a temporary vector3 for rotation
        Vector3 targetCamRot = GetComponent<PlayerData>().eyes.transform.rotation.eulerAngles;
        Vector3 targetBodyRot = transform.rotation.eulerAngles;

        // Up/Down Rotation
        targetCamRot.x += rotY;
        targetCamRot.z = 0;
        targetBodyRot.y += rotX;

        // Clamping the look rotation
        if (xAxisClamp > 60)
        {
            xAxisClamp = 60;
            targetCamRot.x = 60;
        }
        else if (xAxisClamp < -60)
        {
            xAxisClamp = -60;
            targetCamRot.x = 300;
        }

        // Applying the rotations to the player
        GetComponent<PlayerData>().eyes.transform.rotation = Quaternion.Euler(targetCamRot);

        transform.rotation = Quaternion.Euler(targetBodyRot);
    }

  



    // RIGID BODY CONTROLLER STUFF
    private void MoveBody()
    {
        //Calculate the velocity we want to move in
        targetVelocity = Vector3.zero;
        if (state.ThumbSticks.Left.Y > .2 || state.ThumbSticks.Left.Y < -.2)
        {
            targetVelocity += transform.forward*state.ThumbSticks.Left.Y * speed;

        }
        if (state.ThumbSticks.Left.X > .2 || state.ThumbSticks.Left.X < -.2)
        {
            targetVelocity += transform.right * state.ThumbSticks.Left.X * speed;
        }

        
        

        //Get current body velocity and calculate required velocity to achive the change
        velocity = body.velocity;
        velocityDelta = targetVelocity - velocity;
      
        

        velocityDelta.y = 0;

        //Add the calculated velocity as a velocity change force to the rigidbody
        body.AddForce(velocityDelta, ForceMode.VelocityChange);



        //8 Directional movement for animation    
        dir = 0;
        if (state.ThumbSticks.Left.Y > 0 && state.ThumbSticks.Left.X == 0)
            dir = 1;
        else if (state.ThumbSticks.Left.Y > 0 && state.ThumbSticks.Left.X > 0)
            dir = 2;
        else if (state.ThumbSticks.Left.Y == 0 && state.ThumbSticks.Left.X > 0)
            dir = 3;
        else if (state.ThumbSticks.Left.Y < 0 && state.ThumbSticks.Left.X > 0)
            dir = 4;
        else if (state.ThumbSticks.Left.Y < 0 && state.ThumbSticks.Left.X == 0)
            dir = 5;
        else if (state.ThumbSticks.Left.Y < 0 && state.ThumbSticks.Left.X < 0)
            dir = 6;
        else if (state.ThumbSticks.Left.Y == 0 && state.ThumbSticks.Left.X < 0)
            dir = 7;
        else if (state.ThumbSticks.Left.Y > 0 && state.ThumbSticks.Left.X < 0)
            dir = 8;
    }

    private void JumpBody()
    {
        jumpSpeed = 0;
        jumped = false;


        if (GetComponent<PlayerInput>().JumpState == 1)
        {
            jumped = true;
            body.AddForce(new Vector3(0, 2.5f, 0), ForceMode.VelocityChange);
        }
    }
    public bool CheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + downDist * -Vector3.up);
        //OOO
        //OXO
        //OOO
        raySpawn = transform.position;
        if (Physics.Raycast(raySpawn, -Vector3.up, downDist))
            return true;

        //OOO
        //OOX
        //OOO
        raySpawn.x += sideDist;
        if (Physics.Raycast(raySpawn, -Vector3.up, downDist))
            return true;
        raySpawn = transform.position;
        //OOO
        //XOO
        //OOO
        raySpawn.x -= sideDist;
        if (Physics.Raycast(raySpawn, -Vector3.up, downDist))
            return true;
        raySpawn = transform.position;
        //OXO
        //OOO
        //OOO
        raySpawn.z += sideDist;
        if (Physics.Raycast(raySpawn, -Vector3.up, downDist))
            return true;
        raySpawn = transform.position;

        //OOO
        //OOO
        //OXO
        raySpawn.z -= sideDist;
        if (Physics.Raycast(raySpawn, -Vector3.up, downDist))
            return true;
        



        return false;

    }



}
