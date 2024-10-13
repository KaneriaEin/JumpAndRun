using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwordFFXV : MonoBehaviour
{
    private void Awake()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyCharacter enemy;
        enemy = other.gameObject.GetComponent<EnemyCharacter>();
        if(enemy != null)
        {
            PlayerController.Instance.playerWeaponController.OnWeaponTriggerEnter(enemy);
        }
    }
}
