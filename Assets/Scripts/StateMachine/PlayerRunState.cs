using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        Debug.Log("EnterState : PlayerRunState");
        Ctx.Animator.SetBool(Ctx.IsWalkingHash, false);
        Ctx.Animator.SetBool(Ctx.IsRunningHash, true);
        Ctx.Animator.SetFloat("Speed", Ctx.PlayerSpeed);
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
        HandleRotation();
        HandleMove();
    }

    public override void CheckSwitchState()
    {
        if (!Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Idle());
        }
        else if (Ctx.IsMovementPressed && Ctx.IsWalkPressed)
        {
            SwitchState(Factory.Walk());
        }
    }

    void HandleMove()
    {
        var forward = Ctx.PlayerCamera.transform.forward;
        var right = Ctx.PlayerCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;

        //Debug.LogFormat("forward{0} right{1}", forward, right);
        forward.Normalize();
        right.Normalize();

        forward.y = 1f;
        right.y = 1f;

        Ctx.CurrentMovementZ = forward.z * Ctx.CurrentMovementInput.y + right.z * Ctx.CurrentMovementInput.x;
        Ctx.CurrentMovementX = forward.x * Ctx.CurrentMovementInput.y + right.x * Ctx.CurrentMovementInput.x;
        Ctx.AppliedMovementZ = (forward.z * Ctx.CurrentMovementInput.y + right.z * Ctx.CurrentMovementInput.x) * Ctx.RunMultiplier;
        Ctx.AppliedMovementX = (forward.x * Ctx.CurrentMovementInput.y + right.x * Ctx.CurrentMovementInput.x) * Ctx.RunMultiplier;

        if (Ctx.IsWalkPressed)
        {
            Ctx.PlayerCharacterController.Move(Ctx.CurrentMovement * Time.deltaTime);
        }
        else
        {
            Ctx.PlayerCharacterController.Move(Ctx.AppliedMovement * Time.deltaTime);
        }

    }

    void HandleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = Ctx.CurrentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = Ctx.CurrentMovement.z;

        Quaternion currentRotation = Ctx.transform.rotation;

        if (Ctx.IsMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            Ctx.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Ctx.RotaionFactorPerFrame * Time.deltaTime);
        }
    }

}
