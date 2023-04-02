using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region State Variables
    //Player state machine
    public PlayerStateMachine StateMachine { get; private set; }
    //Player States
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    //Player data for reading and configuring player attributes
    [SerializeField]
    private PlayerData playerData;
    #endregion

    #region Components
    //Animator
    public Animator Anim { get; private set; }
    //Input handler
    public PlayerInputHandler InputHandler { get; private set; }
    //Rigidbody 2D
    public Rigidbody2D RB { get; private set; }
    #endregion
    
    #region Check Transforms
    //Ground check transform for checking if the player is grounded
    [SerializeField]
    private Transform groundCheck;
    //Wall check transform for checking if the player is touching a wall
    [SerializeField]
    private Transform wallCheck;
    #endregion

    #region Other Variables
    //Current x and y velocity of the player
    public Vector2 CurrentVelocity { get; private set; }
    //The direction the player is facing (-1 left, 1 right)
    public int FacingDirection { get; private set; }
    //Temporary vector for workspace calculations
    private Vector2 workspace;
    #endregion
    
    #region Unity Callback Functions
    private void Awake()
    {
        //Initialize the player state machine and states
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");
    }

    private void Start()
    {
        //Getting Animator, PlayerInputHandler and Rigidbody2D components
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        //Setting player direction to facing right on game start
        FacingDirection = 1;
        //Initializing IdleState on game start
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        //Get the current velocity of the player
        CurrentVelocity = RB.velocity;
        //Update the logic of the current state
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        //Update the physics of the current state
        StateMachine.CurrentState.PhysicsUpdate();

    }
    #endregion

    #region Set Functions
    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workspace.Set(angle.x *velocity * direction, angle.y * velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public void SetVelocityX(float velocity)
    {
        //Set the x velocity of the player
        workspace.Set(velocity, CurrentVelocity.y);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }
    
    public void SetVelocityY(float velocity)
    {
        //set the y velocity of the player
        workspace.Set(CurrentVelocity.x, velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }
    #endregion

    #region Check Functions
    public bool CheckIfGrounded()
    {
        //Checking if the player is grounded using the groundCheck position, groundCheckRadius and defining what ground is
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    public bool CheckIfTouchingWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public bool CheckIfTouchingWallBack()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public void CheckIfShouldFlip(int xInput)
    {
        //If player is hold the left or right key and facing in the opposite direction
        //Player is flipped
        if(xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    #endregion

    #region Other Functions

    //Modifies player gravity when falling, making the player fall faster.
    public void FallMultiplier() => CurrentVelocity += Vector2.up * Physics2D.gravity.y * (playerData.fallMultiplier) * Time.deltaTime;

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger(); //Called when an animation event is triggered

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger(); //Called when an animation finishes

    private void Flip()
    {
        //Rotate the transform of the player and set FacingDirection accordingly
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    #endregion
}
