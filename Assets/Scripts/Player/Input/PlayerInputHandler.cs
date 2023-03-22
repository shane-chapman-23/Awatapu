using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 RawMovementInput { get; private set; }
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputHeld { get; private set; }

    //the amount of time the player has after hitting the jump button before the JumpInput variable is set to false;
    private float inputHoldTime = 0.2f;
    //the time when the player first presses the jump button
    private float jumpInputStartTime;

    private void Update()
    {
        CheckJumpInputHoldTime();

        Debug.Log(JumpInputHeld);
    }
    //Method is called whenever there is new movement input from the player
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        //Reading the input from the player and storing it in the RawMovementInput variable
        RawMovementInput = context.ReadValue<Vector2>();

        //Normalising the X and Y input to return an integer rather than a float
        //This makes it easier to process and compare the input values
        NormInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
        NormInputY = (int)(RawMovementInput * Vector2.up).normalized.y;
    }

    //Method is called whenever there is new jump input from the player
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            JumpInput = true;
            JumpInputHeld = true;
            jumpInputStartTime = Time.time;
        } 
        //If player releases jump button, JumpInputHeld variable set to false
        else if(context.canceled)
        {
            JumpInputHeld = false;
        }
    }

    //once JumpInput is used set it back to false;
    public void UseJumpInput() => JumpInput = false;
    
    //Jump input is true until use or until time runs out
    //Allowing player to press jump just before landing and the jump is still registered.
    private void CheckJumpInputHoldTime()
    {
        if(Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }
}
