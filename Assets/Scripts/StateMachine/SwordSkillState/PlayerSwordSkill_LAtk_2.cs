using Assets.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordSkill_LAtk_2 : PlayerBaseState, ISkillState
{
    public PlayerSwordSkill_LAtk_2(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        Debug.Log("EnterState : PlayerSwordSkill_LAtk_2");
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
        //if ���򵽣��л��ܻ�״̬
        //

        //��������
        AnimatorStateInfo clip = Ctx.Animator.GetCurrentAnimatorStateInfo(1);
        if (clip.normalizedTime >= 0.7 && clip.IsName("LAttack2"))
        {
            //Debug.Log("Now player can do next action, last input is " + Ctx.SkillCtl.playerLastInput);
            switch (Ctx.SkillCtl.playerLastInput)
            {
                case PlayerInputType.LAttack:
                    SwitchState(Factory.Sword_LAtk_3());
                    break;
                default:
                    break;
            }
        }

        if (Ctx.IsDuringAnim)
        {
            return;
        }
        SwitchState(Factory.Grounded());
    }
    public void ISkillDoDamage(EnemyCharacter enemy, Transform attacker)
    {
        Debug.LogFormat("PlayerSwordSkill_LAtk_2 SkillDoDamage From[{0}] To [{1}]", Ctx.gameObject.name, enemy.gameObject.name);
        enemy.TakeDamage(12, attacker);
        //CameraManager.Instance.ScreenShake();
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
