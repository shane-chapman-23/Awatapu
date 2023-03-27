//This code defines the base class for all states that the player can be in.
//It contains shared functionality that all states can use.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    //The player, state machine, and player data that this state will interact with.
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;
    //Whether the animation has finished
    protected bool isAnimationFinished;
    //The time at which the state was entered
    protected float startTime;
    //The name of the boolean parameter in the animator that corresponds to the states animation
    private string animBoolName;

    //Constructor for the PlayerState class
    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    //Called when the state is entered
    public virtual void Enter()
    {
        //Performs checks for the state
        DoChecks();
        //Set the animation boolean for the state to true
        player.Anim.SetBool(animBoolName, true);
        //Record the start time of the state
        startTime = Time.time;
        //Set isAnimationFinished to false since the animation has just started
        isAnimationFinished = false;

    }

    //Called when the state is exited
    public virtual void Exit()
    {
        //Setting the animation boolean to false on exit
        player.Anim.SetBool(animBoolName, false);
    }

    //Called every frame to update the logic for the state
    public virtual void LogicUpdate()
    {

    }

    //Called every physics update the update the physics of the state
    public virtual void PhysicsUpdate()
    {
        //Perform checks for the state every physics update
        DoChecks();
    }

    //Performs any checks needed for the state
    public virtual void DoChecks()
    {

    }

    //Called when the animation event is triggered
    public virtual void AnimationTrigger() {}

    //Called when the animation for the state has finished
    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
   
}
