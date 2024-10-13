using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void SkillDoDamageHandler(EnemyCharacter enemy, Transform attacker);

public class PlayerWeaponController : MonoBehaviour
{
    [Header("Weapons")]
    //Weapon Sword
    public GameObject SwordFFXV_fbx;
    public Collider SwordFFXV_fbx_Collider;
    //public Transform SwordFFXV_trans;

    public Collider CurrentWeapon_Collider;
    public SoundAssetsType CurrentWeapon_Clip;

    public SkillDoDamageHandler DoDamage = null;

    public IWeapon _weapon;

    private void Awake()
    {
        PlayerController.Instance.playerWeaponController = this;
    }


    void Start()
    {
        CurrentWeapon_Collider = SwordFFXV_fbx_Collider;
        CurrentWeapon_Clip = SoundAssetsType.swordWave;
    }

    void Update()
    {
        
    }

    public void SetCurrentWeaponCollider(bool flag)
    {
        CurrentWeapon_Collider.enabled = flag;
        if (flag)
        {
            GameAssets.Instance.PlaySoundEffect(PlayerController.Instance.playerAudioSource, CurrentWeapon_Clip);
        }
    }

    public void SetDamagerHandler(SkillDoDamageHandler e)
    {
        DoDamage = e;
    }

    public void ClearDamageHandler()
    {
        DoDamage = null;
    }
 

    public void OnWeaponTriggerEnter(EnemyCharacter enemy)
    {
        if(DoDamage != null)
        {
            DoDamage(enemy, gameObject.transform);
        }
    }
}
