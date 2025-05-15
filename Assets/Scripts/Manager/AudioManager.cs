using DesignPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _bgmSource;
    private ObjectPool _sfxPool;

    [SerializeField] private List<AudioClip> _bgmList = new();
    [SerializeField] private SFXController _sfxPrefab;

    private void Awake() => Init();
    private void Init()
    {
        _bgmSource = GetComponent<AudioSource>();

        _sfxPool = new ObjectPool(transform,_sfxPrefab, 10);
    }

    public void BgmPlay(int index)
    {
        if (0 <= index && index < _bgmList.Count)
        {
            _bgmSource.Stop();
            _bgmSource.clip = _bgmList[index];
            _bgmSource.Play();
        }
        
    }

    //bgm주세요
    public SFXController GetSFX()
    {
        //풀에서 꺼내와서 반환
        PooledObject po = _sfxPool.PopPool();
        return po as SFXController;
    }
}
