using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    //引用
    protected Animator _animator;
    protected AudioSource _audioSource;

    //攻击者
    protected Transform currentAttacker;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponentInChildren<AudioSource>();
    }

    void Update()
    {
        
    }

    public void TakeDamage(int damage, Transform attacker)
    {
        Debug.Log("TakeDamage:" + damage);

        //------------播放受击动画、声音------------
        _animator.Play("Enemy_Hit", 0, 0f);
        GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.hit);

        //---------------记录战斗状态---------------
        currentAttacker = attacker;

    }
}
