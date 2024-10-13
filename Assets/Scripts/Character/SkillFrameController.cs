using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SkillFrameController : MonoBehaviour
{
    public Animator anim;
    public PlayerInputType playerLastInput = PlayerInputType.None;
    public SkillDoDamageHandler OnDamageHandler = null;

    void Start()
    {
        
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        //if(Player.Instance.playerCombatState == PlayerCombatStatusState.CombatIdle)
        //{
        //    if(Input.GetMouseButtonDown(0))
        //    {
        //        anim.SetTrigger("LAttack");
        //    }
        //}

        if (PlayerController.Instance.allowInput == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                playerLastInput = PlayerInputType.LAttack;
                Debug.Log("Get PlayerLastInput [LAttack]");
            }
            if (Input.GetMouseButtonDown(1))
            {
                playerLastInput = PlayerInputType.HAttack;
                Debug.Log("Get PlayerLastInput [HAttack]");
            }
        }
    }

    /// <summary>
    /// 攻击判断开始
    /// </summary>
    void SetHitStartup()
    {
        //Debug.Log("SetHitStartup");
        PlayerController.Instance.playerWeaponController.SetCurrentWeaponCollider(true);
    }

    /// <summary>
    /// 攻击判断结束
    /// </summary>
    void SetHitEnd()
    {
        //Debug.Log("SetHitEnd");
        PlayerController.Instance.playerWeaponController.SetCurrentWeaponCollider(false);
    }

    /// <summary>
    /// 允许玩家输入操控角色
    /// </summary>
    void SetAllowInput()
    {
        //Debug.Log("SetAllowInput");
        playerLastInput = PlayerInputType.None;
        PlayerController.Instance.allowInput = true;
    }

    /// <summary>
    /// 拒绝玩家输入操控角色
    /// </summary>
    void SetNotAllowInput()
    {
        //Debug.Log("SetNotAllowInput");
        PlayerController.Instance.allowInput = false;
    }

    /// <summary>
    /// 输入玩家输入的最新指令到动画上
    /// </summary>
    void SetNextAction()
    {
        //Debug.LogFormat("SetNextAction, playerLastInput({0})", playerLastInput);

    }

    /// <summary>
    /// 角色一套动作结束后归位idle
    /// </summary>
    void SetIdleRecovery()
    {
        //Debug.Log("SetIdleRecovery");
    }

    /// <summary>
    /// 禁用角色w,a,s,d位移
    /// </summary>
    void SetCannotMove()
    {
        //Debug.Log("SetCannotMove");
        //GetComponent<AnimationAndMovementController>().enabled = false;
    }

    /// <summary>
    /// 技能开始
    /// </summary>
    void SetSkillStartup()
    {
        //Debug.Log("SetSkillStartup");
        //Player.Instance.canMove = false;
        //Player.Instance.playerCombatState = PlayerCombatStatusState.Nonbreaking;
    }

    /// <summary>
    /// 蓄力技能
    /// </summary>
    void BeginHolding(int button)
    {
        this.anim.speed = 0;
        StartCoroutine(CheckHolding(button));
    }

    IEnumerator CheckHolding(int button)
    {
        yield return new WaitForSeconds(0.2f);
        if (!Input.GetMouseButton(button))
        {
            this.anim.speed = 1;
            this.anim.SetBool("HAttackHold", false);
        }
    }
}
