using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : Action
{
    protected Animator animator;
    protected PlayerController player;

    public override void OnAwake()
    {
        player = PlayerController.Instance;
        animator = gameObject.GetComponent<Animator>();
    }
}
