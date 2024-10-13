using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }
    public override void EnterState()
    {
        Debug.Log("EnterState : PlayerJumpState");
        Ctx.Animator.SetBool("InAir", true);
        HandleJump();
    }

    public override void ExitState()
    {
        //Ctx.Animator.SetBool(Ctx.IsJumpingHash, false);
        if (Ctx.IsJumpPressed)
        {
            Ctx.RequireNewJumpPress = true;
        }
        if (Ctx.JumpCount != 3)
        {
            //Ctx.CurrentJumpResetRoutine = Ctx.StartCoroutine(jumpResetRoutine());
        }
        if (Ctx.JumpCount == 3)
        {
            Ctx.JumpCount = 0;
            //Ctx.Animator.SetInteger(Ctx.JumpCountHash, Ctx.JumpCount);
        }
    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        HandleRotation();
        HandleMove();

        HandleGravity();
        CheckSwitchState();
    }

    public override void CheckSwitchState()
    {
        if (Ctx.PlayerCharacterController.isGrounded)
        {
            SwitchState(Factory.Grounded());
        }
    }

    void HandleJump()
    {
        if (Ctx.JumpCount < 3 && Ctx.CurrentJumpResetRoutine != null)
        {
            Ctx.StopCoroutine(Ctx.CurrentJumpResetRoutine);
        }
        //Ctx.Animator.SetBool(Ctx.IsJumpingHash, true);
        Ctx.IsJumping = true;
        Ctx.JumpCount += 1;
        //Ctx.Animator.SetInteger(Ctx.JumpCountHash, Ctx.JumpCount);
        Ctx.CurrentMovementY = Ctx.InitialJumpVelocities[Ctx.JumpCount] * PlayerController.Instance.jmpFactor;
        Ctx.AppliedMovementY = Ctx.InitialJumpVelocities[Ctx.JumpCount] * PlayerController.Instance.jmpFactor;
    }

    void HandleGravity()
    {
        //Debug.LogFormat("isgrounded: {0}", Ctx.PlayerCharacterController.isGrounded);
        bool isFalling = Ctx.CurrentMovementY <= 0.0f || !Ctx.IsJumpPressed;
        float fallMultiplier = 2.0f;

        if (isFalling)
        {
            float previousYVelocity = Ctx.CurrentMovementY;
            float newYVelocity = Ctx.AppliedMovementY + (Ctx.JumpGravities[Ctx.JumpCount] * fallMultiplier * Time.deltaTime);
            float nextYVelocity = Mathf.Max((previousYVelocity + newYVelocity) * .5f, -20.0f);
            Ctx.CurrentMovementY = nextYVelocity;
            Ctx.AppliedMovementY = nextYVelocity;
        }
        else
        {
            float previousYVelocity = Ctx.CurrentMovementY;
            float newYVelocity = Ctx.AppliedMovementY + (Ctx.JumpGravities[Ctx.JumpCount] * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * .5f;
            Ctx.CurrentMovementY = nextYVelocity;
            Ctx.AppliedMovementY = nextYVelocity;
        }

    }

    IEnumerator jumpResetRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        Ctx.JumpCount= 0;
        //Ctx.Animator.SetInteger(Ctx.JumpCountHash, Ctx.JumpCount);
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
