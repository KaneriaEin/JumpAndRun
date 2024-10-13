using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


class EnemyY_Attack3 : EnemyAction
{

    public override void OnStart()
    {
        //看向玩家
        this.transform.LookAt(player.PlayerPos);
        this.animator.CrossFade("Enemy_Attack_3", 0.1f);
    }

    public override TaskStatus OnUpdate()
    {
        AnimatorStateInfo clip = this.animator.GetCurrentAnimatorStateInfo(0);
        if (clip.IsName("Enemy_Attack_3") && clip.normalizedTime >= 1)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Running;
        }
    }

    public override void OnEnd()
    {
        this.animator.CrossFade("Enemy_Idle", 0.1f);
    }

}
