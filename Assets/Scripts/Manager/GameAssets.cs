using System.Collections.Generic;
using UnityEngine;

public enum SoundAssetsType
{
    hit,
    swordWave,
    hSwordWave
}

[System.Serializable]
public class SoundAssets
{
    public string assetsName;
    public AudioClip[] assetsClip;
}

public class GameAssets : MonoSingleton<GameAssets>
{
    public List<SoundAssets> sound_assets = new List<SoundAssets>();
    private Dictionary<string, AudioClip[]> assetsDictionary = new Dictionary<string, AudioClip[]>();

    private void Start()
    {
        InitAssets();
    }
    public void InitAssets()
    {
        for (int i = 0; i < sound_assets.Count; i++)
        {
            if (!assetsDictionary.ContainsKey(sound_assets[i].assetsName))
            {
                assetsDictionary.Add(sound_assets[i].assetsName, sound_assets[i].assetsClip);
            }
        }
    }


    #region ��Ч��������

    /// <summary>
    /// �������ֻ�����Ч
    /// </summary>
    /// <param name="audioSource">��Դ</param>
    /// <param name="soundAssetsType">��Ч����</param>
    public void PlaySoundEffect(AudioSource audioSource, SoundAssetsType soundAssetsType)
    {
        audioSource.clip = GetClipAssets(soundAssetsType);
        audioSource.Play();
    }

    private AudioClip GetClipAssets(SoundAssetsType soundAssetsType)
    {
        switch (soundAssetsType)
        {
            case SoundAssetsType.hit:
                return assetsDictionary["Hit"][Random.Range(0, assetsDictionary["Hit"].Length)];
            case SoundAssetsType.swordWave:
                return assetsDictionary["SwordWave"][Random.Range(0, assetsDictionary["SwordWave"].Length)];
            case SoundAssetsType.hSwordWave:
                return assetsDictionary["HSwordWave"][Random.Range(0, assetsDictionary["HSwordWave"].Length)];
            default:
                Debug.Log("û�ҵ�");
                return null;
        }
    }


    #endregion
}
