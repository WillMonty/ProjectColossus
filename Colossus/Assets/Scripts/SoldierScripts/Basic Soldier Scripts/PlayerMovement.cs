using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;


public class PlayerMovement : MonoBehaviour {


    const float JUMP_FORCE = 3f;
    const float GRAVITY_FORCE = 9.81f;

    CharacterController player;
    Rigidbody body;

    // Player variables
    public float maxSpeed;
    float speed;
    Vector3 movement;
    public float MoveSpeed
    {
        get {
            movement.y = 0f;
            return movement.magnitude;
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

    // Movement Variables
    float moveFB; // Movement forward and backwards
    float moveLR; // Movement left and right

    float rotX; // rotation by the X axis
    float rotY; // rotation by the Y axis


    // Animation Stuff
    bool jumped = false;
    public bool Jumped
    {
        get { return jumped; }

    }

    bool inAir;
    public bool InAir
    {
        set { inAir = value; }
        get { return inAir; }

    }

    int dir = 0;
    public int AnimDir
    {

        get { return dir; }

    }

    #region rigidbody movement WIP

    Vector3 velocity;
    bool isGrounded;

    float rayDist;
    Ray moveRay1;
    Ray moveRay2;
    Ray moveRay3;
    RaycastHit hitData;
    Vector3 rayDir;

    float totalMass;
    Vector3 totalVelocity;
    Vector3 bodyTotalVel;
    Vector3 jumpForce = new Vector3(0, 1000f, 0);
    #endregion

    // Use this for initialization
    void Start ()
    {
        player = GetComponent<CharacterController>();
        body = GetComponent<Rigidbody>();

    }
	
	// Update is called once per frame
	void Update ()
    {      
        state = GetComponent<PlayerInput>().State;
        prevState = GetComponent<PlayerInput>().PrevState;

        if (GameManagerScript.instance.currentGameState == GameState.InGame && GetComponent<PlayerInput>().PlayerIndexSet)
        {
            RotateCamera();
            Jump();
            if (player.enabled)
            {
                
                Move();
            }
            else
            {
                MoveBody();
            }
          
          
        }
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

    /// <summary>
    /// Method to handle jumping 
    /// </summary>
    private void Jump()
    {
        jumped = false;

        if (player.isGrounded && inAir)
            inAir = false;

        if (player.enabled)
        {
            if (player.isGrounded)
            {
                if (GetComponent<PlayerInput>().JumpState == 1)
                {
                    verticalVelocity = JUMP_FORCE;
                    jumped = true;
                    inAir = true;
                }


            }
            else
                verticalVelocity -= GRAVITY_FORCE * Time.deltaTime;


            verticalVelocity = Mathf.Clamp(verticalVelocity, -5f, 5f);
            return;
        }


        if (GetComponent<PlayerInput>().JumpState == 1)
        {
            body.AddForce(jumpForce);
            jumped = true;
        }

        if (!player.isGrounded && !inAir)
            inAir = true;

        
    }

    /// <summary>
    /// Handles all movement calls
    /// </summary>
    private void Move()
    {
        #region Handles Running
        // Handles Running
        if(state.Triggers.Right>0)
        {
            speed = maxSpeed *0.3f;
        }
        else if (state.Buttons.LeftStick == ButtonState.Pressed)
        {
            speed = maxSpeed * 1.3f;
        }
        else
        {
            speed = maxSpeed;
        }
        #endregion

        #region Getting Input
        // Movement variables/axis
        dir = 0;

        if (state.ThumbSticks.Left.Y > .2 || state.ThumbSticks.Left.Y < -.2)
        {
            moveFB = state.ThumbSticks.Left.Y * speed;

        }
        else
            moveFB = 0;

        if (state.ThumbSticks.Left.X > .2 || state.ThumbSticks.Left.X < -.2)
        {
            moveLR = state.ThumbSticks.Left.X * speed;

        }
        else
            moveLR = 0;



        //8 Directional movement for animation    
        if (moveFB > 0 && moveLR == 0)
            dir = 1;
        else if (moveFB > 0 && moveLR > 0)
            dir = 2;
        else if (moveFB == 0 && moveLR > 0)
            dir = 3;
        else if (moveFB < 0 && moveLR > 0)
            dir = 4;
        else if (moveFB < 0 && moveLR == 0)
            dir = 5;
        else if (moveFB < 0 && moveLR < 0)
            dir = 6;
        else if (moveFB == 0 && moveLR < 0)
            dir = 7;
        else if (moveFB > 0 && moveLR < 0)
            dir = 8;
        #endregion

        #region Applying movement
        // Create a vector movement
        movement = Vector3.zero;
        movement = transform.rotation * new Vector3(moveLR, verticalVelocity, moveFB);


        // Apply the final movement to the player
        player.Move(movement * Time.deltaTime);

        #endregion
    }


    // WIP RIGID BODY CONTROLLER STUFF
    private void MoveBody()
    {
        
        velocity = Vector3.zero;
        if (state.ThumbSticks.Left.Y > .2 || state.ThumbSticks.Left.Y < -.2)
        {
            velocity += transform.forward*state.ThumbSticks.Left.Y * maxSpeed;

        }
        if (state.ThumbSticks.Left.X > .2 || state.ThumbSticks.Left.X < -.2)
        {
            velocity += transform.right * state.ThumbSticks.Left.X * maxSpeed;
        }

        ValidateVelocity();
        transform.position+= velocity*Time.deltaTime;
        //body.MovePosition(transform.position + velocity * Time.deltaTime);

        transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, 0.0f);


    }

    private void ValidateVelocity()
    {
        rayDir = velocity.normalized;
        rayDist = 0.2f;

        moveRay1.origin = transform.position + new Vector3(0, 0.2f, 0);
        moveRay2.origin = transform.position + new Vector3(0, 0, 0);
        moveRay3.origin = transform.position + new Vector3(0, -0.2f, 0);

        moveRay1.direction = moveRay2.direction = moveRay3.direction = rayDir;

        if (Physics.Raycast(moveRay1,out hitData,rayDist) || Physics.Raycast(moveRay2, out hitData, rayDist) || Physics.Raycast(moveRay3, out hitData, rayDist))
        {
            if (hitData.collider.gameObject.GetComponent<Rigidbody>())
            {
               // ResolveCollision(hitData.collider.gameObject.GetComponent<Rigidbody>());
                return;

            }

            if (rayDir.x != 0) 
                velocity.x = 0;

            if (rayDir.z != 0)
                velocity.z = 0;
            
        }
    }

    private void ResolveCollision(Rigidbody obstacle)
    {
        
        if (obstacle.tag != "colossusplayer" && tag != "colossusarm" && tag != "colossushead" && tag != "projectile")
        {
            totalMass = body.mass + obstacle.mass;
            bodyTotalVel = velocity + body.velocity;
            totalVelocity = bodyTotalVel + obstacle.velocity;
            totalVelocity *= 0.8f / 2f;


            
        }
    }


}
