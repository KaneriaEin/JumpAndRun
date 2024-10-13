using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }
    public override void EnterState()
    {
        Ctx.Animator.SetBool("InAir", false);
        PlayerController.Instance.allowInput = true;
        Ctx.CurrentMovementY = Ctx.GroundedGravity;
        Ctx.AppliedMovementY = Ctx.GroundedGravity;
    }

    public override void ExitState()
    {
    }

    public override void InitializeSubState()
    {
        //Debug.LogFormat("PlayerGroundedState:InitializeSubState move[{0}] walk[{1}] spd[{2}]", Ctx.IsMovementPressed, Ctx.IsWalkPressed, Ctx.PlayerSpeed);
        Ctx.Animator.SetFloat("Speed", Ctx.PlayerSpeed);
        if (!Ctx.IsMovementPressed && !Ctx.IsWalkPressed)
        {
            SetSubState(Factory.Idle());
        }
        else if(Ctx.IsMovementPressed && !Ctx.IsWalkPressed)
        {
            SetSubState(Factory.Run());
        }
        else
        {
            SetSubState(Factory.Walk());
        }
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void CheckSwitchState()
    {
        if (PlayerController.Instance.allowInput == false)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            SwitchState(Factory.Sword_LAtk_1());
        }

        if (Input.GetMouseButtonDown(1))
        {
            SwitchState(Factory.Sword_HAtk_1());
        }

        //if player is grounded and jump is pressed, switch to jump state
        if (Ctx.IsJumpPressed && !Ctx.RequireNewJumpPress)
        {
            //Debug.LogFormat("PlayerGroundedState:CheckSwitchState SwitchState(_factory.Jump());");
            SwitchState(Factory.Jump());
        }
    }
}
