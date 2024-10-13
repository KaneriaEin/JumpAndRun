using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillState
{
    void ISkillEnterState();
    void ISkillDoDamage(EnemyCharacter enemy, Transform attacker);
    void ISkillExitState();

}
