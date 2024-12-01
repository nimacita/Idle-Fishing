using UnityEngine;


[CreateAssetMenu(fileName = "GameStats")]
public class GameStats : ScriptableObject
{

    [Header("Casting Settings")]
    [Tooltip("���� ������� ��������")]
    public float flightAngle;
    [Tooltip("�������� ��������� �������� ��� ���� �����������")]
    public float forceSpeed = 1.5f;
    [Tooltip("����������� ���� �����������")]
    public float minForce = 5f;
    [Range(5f, 25f)]
    [Tooltip("������������ ���� �����������")]
    public float maxForce = 12f;

    [Header("Waiting Settings")]
    [Tooltip("����������� ����� ��������")]
    public float minWaitingTime;
    [Tooltip("������������ ����� ��������")]
    public float maxWaitingTime;

    [Header("Fishing Settings")]
    [Tooltip("���������� ������������� ��������� �� ����")]
    public float increaseAmount = 10f;
    [Tooltip("����������� �������� ������ �������")]
    public float decreaseSpeed = 45f;
}
