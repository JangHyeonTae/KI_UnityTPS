using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField][Range(0, 100)] private float _attackRange;
    [SerializeField] private int _shootDamage;
    [SerializeField] private float _shootDelay;
    [SerializeField] private AudioClip _shootSFX;
    [SerializeField] private GameObject _fireParticle;

    private CinemachineImpulseSource _impulse;

    private Camera _camera;

    private bool _canShoot { get => _currentCount <= 0; } //_currentCount가 0보다 작으면 true를 반환
    private float _currentCount;

    private void Awake() => Init();
    private void Update() => HandleCanShoot();
    private void Init()
    {
        _camera = Camera.main;
        _impulse = GetComponent<CinemachineImpulseSource>();
    }


    public bool Shoot()
    {
        if (!_canShoot) return false;

        PlayShootSound();
        PlayCameraEffect();
        PlayShootEffect();
        _currentCount = _shootDelay;

        // TODO : Ray 발사 -> 반환받은 대상에게 데미지 부여
        //몬스터 구현시 같이 구현
        RaycastHit hit;
        IDamagable target = RayShoot(out hit);

        if (!hit.Equals(default))
        {
            PlayFireEffect(hit.point, Quaternion.LookRotation(hit.normal));
        }


        if (target == null) return true;

        target.TakeDamage(_shootDamage);

        return true;
    }

    private void HandleCanShoot()
    {
        if (_canShoot) return;

        _currentCount -= Time.deltaTime;
    }

    private IDamagable RayShoot(out RaycastHit hitTarget)
    {
        //+ScreenToPointToRay - 마우스 없애는기능에 더하면 좋음
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _attackRange, _targetLayer))
        {
            hitTarget = hit;
            return ReferenceRegistry.GetProvider(hit.collider.gameObject).
            GetAs<NormalMonster>();
            //TODO : 몬스터를 어덯게 구현하는가에 따라 다름
            //성능상 getcomponent는 안 좋을수도있음
            //return hit.transform.GetComponent<IDamagable>();
        }
        hitTarget = default;
        return null;
    }

    private void PlayFireEffect(Vector3 position, Quaternion rotation)
    {
        Instantiate(_fireParticle, position, rotation);
    }

    //소리
    private void PlayShootSound()
    {
        SFXController sfx = GameManager.Instance.Audio.GetSFX();
        sfx.Play(_shootSFX);
    }

    //cinemachine화면 흔들림
    private void PlayCameraEffect()
    {
        _impulse.GenerateImpulse();
    }

    private void PlayShootEffect()
    {
        //TODO : 총구 화염 효과 - particle
    }
}
