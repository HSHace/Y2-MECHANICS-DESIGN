using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public PlayerStates currentState;

    private void Update()
    {
        switch (currentState)
        {
            case PlayerStates.Dashing:
                //logic for dashing

                break;
            case PlayerStates.Jumping:
                break;
            case PlayerStates.Swimming:
                break;
            case PlayerStates.Crouching:
                break;
            case PlayerStates.Move:
                break;
            case PlayerStates.Idle:
                break;
        }

        switch (currentState)
        {
            case PlayerStates.Dashing:
                //logic for dashing
                break;
            case PlayerStates.Jumping:
                break;
            case PlayerStates.Swimming:
                break;
            case PlayerStates.Crouching:
                break;
            case PlayerStates.Move:
                break;
            case PlayerStates.Idle:
                break;
        }

        //if (jumpPressed) 
        //{
        //    currentState = PlayerStates.InitalJump;
        //}
        //else if(vel.y < 0.1) 
        //{
        //    currentState = PlayerStates.Apex;
        //}
        //enable
    }

    void SwitchState(PlayerStates state)
    {
        switch (currentState)
        {
            case PlayerStates.Dashing:
                //logic for dashing

                break;
            case PlayerStates.Jumping:
                break;
            case PlayerStates.Swimming:
                break;
            case PlayerStates.Crouching:
                break;
            case PlayerStates.Move:
                break;
            case PlayerStates.Idle:
                break;
        }

        currentState = state;

        switch (currentState)
        {
            case PlayerStates.Dashing:
                //logic for dashing

                break;
            case PlayerStates.Jumping:
                break;
            case PlayerStates.Swimming:
                break;
            case PlayerStates.Crouching:
                break;
            case PlayerStates.Move:
                break;
            case PlayerStates.Idle:
                break;
        }

    }
}



public enum PlayerStates
{
    Dashing,
    Jumping,
    Swimming,
    Crouching,
    Move,
    Idle
}