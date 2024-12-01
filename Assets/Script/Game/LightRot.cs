using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRot : MonoBehaviour
{
    // Скорость вращения
    public float rotationSpeed = 100f;

    void Update()
    {
        // Вращаем объект вокруг оси Z
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
