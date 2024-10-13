using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


class EnemyY_Move_RollBack : EnemyAction
{
    int angle = 0;
    Vector3 pos = new Vector3(0,0.5f,0);

    public override void OnStart()
    {
        //看向玩家
        this.transform.LookAt(player.PlayerPos);
        angle = 0;
        //this.transform.LookAt(pos);
        this.animator.CrossFade("Enemy_Walk_Left", 0.1f);
    }

    public override TaskStatus OnUpdate()
    {
        angle += 1;
        this.transform.RotateAround(pos, new Vector3(0, 1, 0), 0.01f);
        return angle == 3000 ? TaskStatus.Success : TaskStatus.Running;
    }

    public override void OnEnd()
    {
        this.animator.CrossFade("Enemy_Idle", 0.1f);
    }

}
