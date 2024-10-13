using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    //����
    protected Animator _animator;
    protected AudioSource _audioSource;

    //������
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

        //------------�����ܻ�����������------------
        _animator.Play("Enemy_Hit", 0, 0f);
        GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.hit);

        //---------------��¼ս��״̬---------------
        currentAttacker = attacker;

    }
}
