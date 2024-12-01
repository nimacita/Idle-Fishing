using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class FishController : MonoBehaviour
{

    [Header("Fish Stats")]
    [Tooltip("������� �� ������� ������� ����")]
    [SerializeField] private Depth currDepth;
    [Tooltip("��� ����")]
    [SerializeField] private FishType fishType;
    [Tooltip("����� ��� ����, ����� ��������")]
    [SerializeField] private float loseTime;
    [Tooltip("����� ��� ����, ����� �������")]
    [SerializeField] private float catchTime;
    [Tooltip("�������� �������� ������� ����")]
    [SerializeField] private float targetSpeed;
    [Tooltip("����� �� ��������� ������� ����")]
    [SerializeField] private float inTargetTime;
    [Tooltip("������� ����� ����")]
    [SerializeField] private float inTargetRange;
    [Tooltip("������� �� �����")]
    [SerializeField] private bool isStartSwiming = false;

    [Header("Move Stats")]
    [SerializeField] private Vector2 moveSpeed;
    private Vector3 startPosition;
    private float currSpeed;
    private int randDir;
    [SerializeField] private Vector2 rotateSpeed;
    private float currRotSpeed;

    [Header("Fish Settings")]
    public string fishName;
    public Sprite fishImage;
    public float fishPrice;

    void Start()
    {
        startPosition = transform.position;

        if (isStartSwiming)
        {
            SetStartStats();
        }
    }

    public void SetStartStats()
    {
        if (startPosition != Vector3.zero)
        {
            transform.position = startPosition;
        }
        else
        {
            startPosition = transform.position;
        }


        randDir = Random.Range(-1, 1);
        if (randDir == 0) randDir = 1;

        currSpeed = Random.Range(moveSpeed.x, moveSpeed.y);
        currRotSpeed = Random.Range(rotateSpeed.x, rotateSpeed.y);
    }

    void FixedUpdate()
    {
        FishMove();
    }

    private void FishMove()
    {
        transform.Rotate(new Vector3(0f, 1f, 0f) * currRotSpeed * Time.deltaTime * randDir);
        transform.Translate(Vector3.forward * currSpeed * Time.deltaTime);
    }

    //���������� ��������
    public float LoseTime
    {
        get { return loseTime; }
    }
    public float CatchTime
    {
        get { return catchTime; }
    }
    public float TargetSpeed
    {
        get { return targetSpeed; }
    }
    public float InTargetTime
    {
        get { return inTargetTime; }
    }
    public float InTargetRange
    {
        get { return inTargetRange; }
    }
    public Depth CurrDepth
    {
        get { return currDepth; }
    }
    public FishType CurrFishType
    {
        get
        {
            return fishType;
        }
    }
}
