using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterStatusState
{
    None = 0,
    Invincibility,
    Armor,
    Charge,
    QuickRise,

}

public enum PlayerCombatStatusState
{
    None = 0,
    Idle,
    CombatIdle,
    Nonbreaking,


}

public enum PlayerInputType
{
    None = 0,
    LAttack,
    HAttack,
    LAttackHold,
    HAttackHold
}
public class PlayerController : Singleton<PlayerController>
{
    public float jmpFactor = 1f;
    public SkillFrameController playerSkillController;
    public AudioSource playerAudioSource;

    #region Player States
    public PlayerStateMachine playerStateMachine;
    public PlayerWeaponController playerWeaponController;
    public bool allowInput = false;
    public bool canMove = true;
    public bool inAir = false;
    public PlayerCombatStatusState playerCombatState;
    public Transform PlayerPos { get { return playerStateMachine.transform; } }
    #endregion

    #region Player States gettters and setters
    //public bool InAir {  get { return inAir; }  set { inAir = value;  playerController.anim.SetBool("InAir", value); } }
    #endregion

    void Init()
    {
        playerCombatState = PlayerCombatStatusState.None;

    }



}
