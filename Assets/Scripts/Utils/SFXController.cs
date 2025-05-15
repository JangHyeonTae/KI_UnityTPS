using DesignPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : PooledObject
{
    private AudioSource _audioSource;
    private float _currentCount;

    private void Awake() => Init();
    private void Init()
    {
        _audioSource = GetComponent<AudioSource>();
        //자주 실행되기 때문에 coroutine사용안함 -> coroutine class 힙에 계속 할당
        
    }

    private void Update()
    {
        _currentCount -= Time.deltaTime;

        if (_currentCount <= 0)
        {
            ReturnPool();
        }
    }

    public void Play(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();

        _currentCount = clip.length;
    }
}
