//This class defines a state machine for the player
//It manages the current state of the player and provides methods for changin states

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    //Current state of the player
    public PlayerState CurrentState { get; private set; }

    //Initializes the state machine with the starting state
    public void Initialize(PlayerState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter(); //Enters the starting state
    }

    //Changes the current state of the player to the new state
    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit(); //Exits current state
        CurrentState = newState; //Sets the new state as the current state
        CurrentState.Enter(); // Enters the new state
    }
}
