using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpGuageUI : MonoBehaviour
{
    // 1. LookAt(메인 카메라) -> 카메라 위치에 따라서 HP게이지가 기울어짐
    // 2. 현재 카메라의 방향으로 회전 죽, 카메라의 방향 벡터를 적용
    // 3. 카메라의 반대 방향 벡터 적용

    [SerializeField] private Image _image;
    private Transform _cameraTransform;

    private void Awake() => Init();


    private void LateUpdate() => SetUIForwardVector(_cameraTransform.forward);

    private void Init()
    {
        _cameraTransform = Camera.main.transform;
    }

    //현재수치 / 최대 수치

    public void SetImageFillAmount(float value)
    {
        _image.fillAmount = value;
    }

    //카메라에 따른 ui회전
    public void SetUIForwardVector(Vector3 target)
    {
        transform.forward = target;
    }

}
