using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private int xInput;

    private bool isGrounded;
    private bool coyoteTime;
    private bool jumpInput;
    private bool jumpInputHeld;
    

    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIfGrounded();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();
        JumpModifier();

        xInput = player.InputHandler.NormInputX;
        jumpInput = player.InputHandler.JumpInput;
        jumpInputHeld = player.InputHandler.JumpInputHeld;

        if(isGrounded && player.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.LandState);
        }
        else if(jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else
        {
            player.CheckIfShouldFlip(xInput);
            player.SetVelocityX(playerData.movementVelocity * xInput);

            player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
            player.Anim.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void CheckCoyoteTime()
    {
        if(coyoteTime = true && Time.time > startTime + playerData.coyoteTime)
        {
            coyoteTime = false;
        }
    }

    public void StartCoyoteTime() => coyoteTime = true;

    //Modify player jump.
    //Allows player to control jump height while hoilding jump button
    //increase the gravity when player is falling
    private void JumpModifier()
    {
        //Checking if player is not grounded, if jump is not held and if velocity.y is more than 0.01f
        //Setting Velocity.y to 0 if conditions met
        if(!isGrounded && !jumpInputHeld && player.CurrentVelocity.y > 0.01f)
        {
            player.SetVelocityY(0f);
        }

        //If player is falling
        //Increase player gravity
        if(player.CurrentVelocity.y < 0)
        {
            player.FallMultiplier();
        }
    }
}
