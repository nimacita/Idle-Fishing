using UnityEngine;


[CreateAssetMenu(fileName = "GameStats")]
public class GameStats : ScriptableObject
{

    [Header("Casting Settings")]
    [Tooltip("Угол запуска поплавка")]
    public float flightAngle;
    [Tooltip("Скорость изменения слайдера для силы закидывания")]
    public float forceSpeed = 1.5f;
    [Tooltip("Минимальная сила закижывания")]
    public float minForce = 5f;
    [Range(5f, 25f)]
    [Tooltip("Максимальная сила закидывания")]
    public float maxForce = 12f;

    [Header("Waiting Settings")]
    [Tooltip("Минимальное время ожидания")]
    public float minWaitingTime;
    [Tooltip("Максимальное время ожидания")]
    public float maxWaitingTime;

    [Header("Fishing Settings")]
    [Tooltip("Количество Прибавляемого значчения за клик")]
    public float increaseAmount = 10f;
    [Tooltip("Убавлеяемое значение каждую секунду")]
    public float decreaseSpeed = 45f;
}
