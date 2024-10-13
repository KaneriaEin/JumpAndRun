using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateFactory
{
    PlayerStateMachine _context;

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
    }

    public PlayerBaseState Idle()
    {
        return new PlayerIdleState(_context, this);
    }
    public PlayerBaseState Walk()
    {
        return new PlayerWalkState(_context, this);
    }
    public PlayerBaseState Run()
    {
        return new PlayerRunState(_context, this);
    }
    public PlayerBaseState Jump()
    {
        return new PlayerJumpState(_context, this);
    }
    public PlayerBaseState Grounded()
    {
        return new PlayerGroundedState(_context, this);
    }
    public PlayerBaseState Sword_LAtk_1()
    {
        return new PlayerSwordSkill_LAtk_1(_context, this);
    }
    public PlayerBaseState Sword_LAtk_2()
    {
        return new PlayerSwordSkill_LAtk_2(_context, this);
    }
    public PlayerBaseState Sword_LAtk_3()
    {
        return new PlayerSwordSkill_LAtk_3(_context, this);
    }
    public PlayerBaseState Sword_HAtk_1()
    {
        return new PlayerSwordSkill_HAtk_1(_context, this);
    }
}
