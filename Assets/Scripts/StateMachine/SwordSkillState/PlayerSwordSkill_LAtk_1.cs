using Assets.Scripts.Manager;
using RPGCharacterAnims.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordSkill_LAtk_1 : PlayerBaseState, ISkillState
{
    public PlayerSwordSkill_LAtk_1(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        Debug.Log("EnterState : PlayerSwordSkill_LAtk_1");
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
        /* ���ȼ����ܻ�(�����) >> ���� >> ����ʽ�������� >> �ع�idle��̬ */

        //
        //if ���򵽣��л��ܻ�״̬
        //
        //Debug.Log("Now  AnimatorClipInfo(1)[0] name [" + Ctx.Animator.GetCurrentAnimatorClipInfo(1)[0].clip.name + "]");

        //��������
        AnimatorStateInfo clip = Ctx.Animator.GetCurrentAnimatorStateInfo(1);
        if (clip.normalizedTime >= 0.7 && clip.IsName("LAttack1"))
        {
            //Debug.Log("Now player can do next action, last input is " + Ctx.SkillCtl.playerLastInput);
            switch (Ctx.SkillCtl.playerLastInput)
            {
                case PlayerInputType.LAttack:
                    SwitchState(Factory.Sword_LAtk_2());
                    break;
                default:
                    break;
            }
        }

        //��Ҳ�������Ļ����ȴ���������
        if (Ctx.IsDuringAnim)
        {
            return;
        }
        //���ع�idle
        SwitchState(Factory.Grounded());
    }

    public void ISkillDoDamage(EnemyCharacter enemy, Transform attacker)
    {
        Debug.LogFormat("PlayerSwordSkill_LAtk_1 SkillDoDamage From[{0}] To [{1}]",Ctx.gameObject.name, enemy.gameObject.name);
        enemy.TakeDamage(11, attacker);
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
