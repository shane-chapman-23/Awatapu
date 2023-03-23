using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoubleJumpState : PlayerAbilityState
{
    private int amountOfJumpsLeft;

    public PlayerDoubleJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        amountOfJumpsLeft = playerData.amountOfJumpsLeft;
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocityY(playerData.jumpVelocity);
        isAbilityDone = true;
        amountOfJumpsLeft--;
    }

    public bool CanJump()
    {
        if(amountOfJumpsLeft > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetAmountOfJumpsLeft() => amountOfJumpsLeft = playerData.amountOfJumpsLeft;

    public void DecreaseAmountOfJumpsLeft() => amountOfJumpsLeft--;
}
