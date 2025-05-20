using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpGuageUI : MonoBehaviour
{
    // 1. LookAt(���� ī�޶�) -> ī�޶� ��ġ�� ���� HP�������� ������
    // 2. ���� ī�޶��� �������� ȸ�� ��, ī�޶��� ���� ���͸� ����
    // 3. ī�޶��� �ݴ� ���� ���� ����

    [SerializeField] private Image _image;
    private Transform _cameraTransform;

    private void Awake() => Init();


    private void LateUpdate() => SetUIForwardVector(_cameraTransform.forward);

    private void Init()
    {
        _cameraTransform = Camera.main.transform;
    }

    //�����ġ / �ִ� ��ġ

    public void SetImageFillAmount(float value)
    {
        _image.fillAmount = value;
    }

    //ī�޶� ���� uiȸ��
    public void SetUIForwardVector(Vector3 target)
    {
        transform.forward = target;
    }

}
