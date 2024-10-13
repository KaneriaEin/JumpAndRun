using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        Debug.Log("EnterState : PlayerIdleState");
        Ctx.Animator.SetBool(Ctx.IsWalkingHash, false);
        Ctx.Animator.SetBool(Ctx.IsRunningHash, false);
        Ctx.Animator.SetFloat("Speed", 0);
        Ctx.AppliedMovementX = 0;
        Ctx.AppliedMovementZ = 0;
    }

    public override void ExitState()
    {
    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void CheckSwitchState()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    SwitchState(Factory.Sword_LAtk_1());
        //}

        if (Ctx.IsMovementPressed && Ctx.IsWalkPressed)
        {
            SwitchState(Factory.Walk());
        }
        else if (Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Run());
        }
    }
}
