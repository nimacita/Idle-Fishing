using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRot : MonoBehaviour
{
    // �������� ��������
    public float rotationSpeed = 100f;

    void Update()
    {
        // ������� ������ ������ ��� Z
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}