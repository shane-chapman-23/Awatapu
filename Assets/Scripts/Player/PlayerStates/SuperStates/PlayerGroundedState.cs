using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int xInput;

    private bool jumpInput;
    private bool isGrounded;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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

        xInput = player.InputHandler.NormInputX;
        jumpInput = player.InputHandler.JumpInput;

        if(jumpInput)
        {
            //Setting JumpInput to false after use
            player.InputHandler.UseJumpInput();
            stateMachine.ChangeState(player.JumpState);
        } 
        else if(!isGrounded)
        {
            player.InAirState.StartCoyoteTime();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }


}
