using Assets.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordSkill_LAtk_3 : PlayerBaseState, ISkillState
{
    public PlayerSwordSkill_LAtk_3(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

public override void EnterState()
    {
        Debug.Log("EnterState : PlayerSwordSkill_LAtk_3");
        Ctx.Animator.SetTrigger("LAttack");
        Ctx.IsDuringAnim = true;
        ISkillEnterState();
    }

    public override void ExitState()
    {
        ISkillExitState();
    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        //Ctx.gameObject.transform.RotateAround(Vector3.zero, Vector3.up, 0);
        CheckSwitchState();
    }

    public override void CheckSwitchState()
    {
        //
        //if 被打到，切换受击状态
        //

        if (Ctx.IsDuringAnim)
        {
            return;
        }
        SwitchState(Factory.Grounded());
    }

    public void ISkillDoDamage(EnemyCharacter enemy, Transform attacker)
    {
        Debug.LogFormat("PlayerSwordSkill_LAtk_3 SkillDoDamage From[{0}] To [{1}]", Ctx.gameObject.name, enemy.gameObject.name);
        enemy.TakeDamage(13, attacker);
        CameraManager.Instance.ScreenShake();
    }

    public void ISkillEnterState()
    {
        Ctx.SkillCtl.playerLastInput = PlayerInputType.None;
        PlayerController.Instance.playerWeaponController.SetDamagerHandler(ISkillDoDamage);
        //GameAssets.Instance.PlaySoundEffect(PlayerController.Instance.playerAudioSource, SoundAssetsType.swordWave);
    }

    public void ISkillExitState()
    {
        PlayerController.Instance.playerWeaponController.ClearDamageHandler();
    }
}
