using Assets.Scripts.Manager;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerSwordSkill_HAtk_1 : PlayerBaseState, ISkillState
{
    public PlayerSwordSkill_HAtk_1(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        UnityEngine.Debug.Log("EnterState : PlayerSwordSkill_HAtk_1");
        Ctx.Animator.SetBool("HAttackHold", true);
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
        CheckHAttackButtonUP();
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
        UnityEngine.Debug.LogFormat("PlayerSwordSkill_HAtk_1 SkillDoDamage From[{0}] To [{1}]", Ctx.gameObject.name, enemy.gameObject.name);
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

    public void CheckHAttackButtonUP()
    {
        //释放技能
        if (Ctx.Animator.speed == 0 && Input.GetMouseButtonUp(1))
        {
            //播放动画
            Ctx.Animator.speed = 1;
            Ctx.Animator.SetBool("HAttackHold", false);

            //处理技能移动
            Ctx.StartCoroutine(SkillMove());

            //处理运镜
            Collider[] attackDetectionTargets = new Collider[4];
            LayerMask layerMask = 1 << 3;
            int counts = Physics.OverlapSphereNonAlloc(Ctx.transform.position, 20, attackDetectionTargets, layerMask);
            UnityEngine.Debug.LogFormat("counts{0}", counts);
            if (counts == 0)
            {
                Ctx.StartCoroutine(EnableVCamera(Ctx.virtualCam));
                Ctx.StartCoroutine(DisableVCamera(Ctx.virtualCam));
            }
        }
    }

    private IEnumerator EnableVCamera(CinemachineVirtualCamera cam)
    {
        yield return new WaitForSeconds(0.2f);

        cam.gameObject.transform.position = Ctx.virtualCam_pos["HAttack1_Fin"].transform.position;
        cam.gameObject.transform.rotation = Ctx.virtualCam_pos["HAttack1_Fin"].transform.rotation;
        cam.gameObject.SetActive(true);
    }
    private IEnumerator DisableVCamera(CinemachineVirtualCamera cam)
    {
        yield return new WaitForSeconds(0.9f);

        cam.gameObject.SetActive(false);
    }

    private IEnumerator SkillMove()
    {
        yield return new WaitForSeconds(0.3f);
        int speed = 30;
        float s = 0;
        while(s < 4)
        {
            Ctx.PlayerCharacterController.Move(Ctx.transform.forward * Time.deltaTime * speed);
            s += Time.deltaTime * speed;
            yield return null;
        }
    }
}
