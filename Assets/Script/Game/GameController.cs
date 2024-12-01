using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    enum GameState
    {
        casting,
        waiting,
        fishing
    }

    [Header("Stats")]
    public GameStats gameStats;

    [Header("Main Settings")]
    [SerializeField] private GameState currState;

    [Header("Casting Stats")]
    public CorkController corkController;
    private float flightAngle;
    private float forceSpeed;
    private float minForce;
    private float maxForce;
    private float currForce;

    [Header("Casting View")]
    public Button castingBtn;
    public GameObject castingForceSliderView;
    public Slider forceSlider;
    public RectTransform sliderRectTransform;
    public Image sliderFill;
    public Gradient forceSliderGrad;
    public Animation castingViewAnim;
    public AnimationClip castingOnAnim;
    public AnimationClip castingOffAnim;
    private float minRotation = -1f;
    private float maxRotation = 1f;
    private float minScale = 0.9f;
    private float maxScale = 1.1f;

    [Header("Wait View")]
    public GameObject fishWaitingView;
    public Slider waitingSlider;
    public Animation waitnigViewAnim;
    public AnimationClip waitingOnAnim;
    public AnimationClip waitingOffAnim;
    private float minWaitingTime, maxWaitingTime;
    private float currWaitingTime;

    [Header("Fishing View")]
    public GameObject fishingView;
    public Slider fishingSlider;
    public Image fishingSliderFill;
    public Color catchingColor;
    public Color losingColor;
    public Button fishingBtn;
    public Slider targetFishingSlider;
    public RectTransform targetImage;
    public Slider currCatchingSlider;
    public Slider currMissingSlider;
    public Animation fishingViewAnim;
    public AnimationClip fishingOnAnim;
    public AnimationClip fishingOffAnim;

    [Header("Fish objects")]
    public GameObject[] fishPrefabs;
    private GameObject currFish;
    private Depth currDepth;
    [SerializeField]
    //����������� ����
    private List<GameObject> shallowFishes;
    [SerializeField]
    //������������ ����
    private List<GameObject> awerageDeepFishes;
    [SerializeField]
    //������������� ����
    private List<GameObject> deepFishes;

    //Fishing Stats
    private float decreaseSpeed;
    private float increaseAmount;

    //Target Stats
    private float targetSpeed;
    private float currTargetValue;
    private float inTargetRange;
    private float inTargetTime;
    private float currTargetTime;
    //����������� ����� ���� ���������
    private float loseTime;
    private float currLoseTime;
    //����������� ����� ���� �������
    private float catchTime;
    private float currCatchTime;
    private bool isCatching;

    [Header("Components")]
    private PlayerController playerController;
    public GameUIController gameUIController;
    public AmbientFishes ambientFishes;
    public Animation cameraAnim;
    public AnimationClip cameraStartFishing;
    public AnimationClip cameraEndFishing;

    //bools
    private bool isPause;
    private bool isWaiting;
    private bool isFishing;
    private bool isCasting;
    private bool isGame;
    private bool isCatch;


    public static GameController instance;
    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        CorkController.onDepthDefinded += CastingEnd;
        GameUIController.isPauseSet += SetPause;
    }

    private void OnDisable()
    {
        CorkController.onDepthDefinded -= CastingEnd;
        GameUIController.isPauseSet -= SetPause;
    }

    void Start()
    {
        playerController = PlayerController.instance;


        SoundController.instance.PlayAmbientSound();

        AllFishDeepSorting();

        ButtonSettings();
        StartGameSettings();
    }

    //��������� ���������� ����
    public void StartGameSettings()
    {
        isPause = false;
        isWaiting = false;
        isFishing = false;
        isCasting = false;
        isCatching = false;
        isGame = true;
        isCatch = false;

        currFish = null;

        StartCasting();
        //SelectRandomFish();

        //gameUIController.StartSettings();
        UpdateGameStateView();

        ambientFishes.SpawnRandomFishes();
        playerController.RestartGameSettings();
    }

    //��������� ������
    private void ButtonSettings()
    {
        castingBtn.onClick.AddListener(CastingRod);

        fishingBtn.onClick.AddListener(OnIncreaseFishingBtnClick);
    }

    //��������� ����� � ����������� �� ���������
    private void UpdateGameStateView()
    {
        if (isGame)
        {
            switch (currState)
            {
                case GameState.casting:
                    {
                        CastingViewAnimEnable(true);
                        fishWaitingView.SetActive(false);
                        fishingView.SetActive(false);
                        break;
                    }
                case GameState.waiting:
                    {
                        castingForceSliderView.SetActive(false);
                        WaitingViewAnimEnable(true);
                        fishingView.SetActive(false);
                        break;
                    }
                case GameState.fishing:
                    {
                        castingForceSliderView.SetActive(false);
                        fishWaitingView.SetActive(false);
                        fishingView.SetActive(true);
                        break;
                    }
            }
        }
        else
        {
            castingForceSliderView.SetActive(false);
            fishWaitingView.SetActive(false);
            fishingView.SetActive(false);
        }
    }

    //�������� ������� ���
    private void CastingViewAnimEnable(bool value)
    {
        if (value)
        {
            castingForceSliderView.SetActive(true);
            castingViewAnim.Play(castingOnAnim.name);
        }
        else
        {
            castingViewAnim.Play(castingOffAnim.name);
        }
    }

    //�������� �������� ���
    private void WaitingViewAnimEnable(bool value)
    {
        if (value)
        {
            fishWaitingView.SetActive(true);
            waitnigViewAnim.Play(waitingOnAnim.name);
        }
        else
        {
            waitnigViewAnim.Play(waitingOffAnim.name);
        }
    }

    void FixedUpdate()
    {
        Casting();
        Waiting();
        Fishing();
        TargetFishing();
    }

    //�������� �����������
    private void StartCasting()
    {
        //�������� �� ����
        flightAngle = gameStats.flightAngle;

        forceSpeed = gameStats.forceSpeed;
        minForce = gameStats.minForce;
        maxForce = gameStats.maxForce;

        //��������� ��������� ��������
        corkController.StartSettings();

        //������������� ��� � ���� �������� ��������
        forceSlider.minValue = minForce;
        forceSlider.maxValue = maxForce;
        forceSlider.value = minForce;

        isCasting = true;
        currState = GameState.casting;
    }

    //�������� ���� ������� �����
    private void Casting()
    {
        if (isCasting && currState == GameState.casting && !isPause)
        {
            float t = Mathf.PingPong(Time.time * forceSpeed, 1);
            forceSlider.value = Mathf.Lerp(minForce, maxForce, t);

            float normalizedValue = (forceSlider.value - minForce) / (maxForce - minForce);

            // �������� ���� �� ��������� � ��������� ��� � ���������� Image
            Color newColor = forceSliderGrad.Evaluate(normalizedValue);
            sliderFill.color = newColor;

            // �������� ������� � ������� ��������
            //float newRotation = Mathf.Lerp(minRotation, maxRotation, normalizedValue);
            float newScale = Mathf.Lerp(minScale, maxScale, normalizedValue);

            // ��������� ������� � ������� � RectTransform ��������
            //sliderRectTransform.localRotation = Quaternion.Euler(0, 0, newRotation);
            sliderRectTransform.localScale = new Vector3(newScale, newScale, 1);
        }
    }

    //������ ������
    public void CastingRod()
    {
        if (isCasting && currState == GameState.casting && !isFishing)
        {
            currForce = forceSlider.value;


            //��������� �������� ������
            playerController.CastingRod();

            CastingViewAnimEnable(false);

            isCasting = false;
        }
    }

    //����������� ��������
    public void ThrowCork()
    {
        corkController.Threw(currForce, flightAngle);
        SoundController.instance.PlayCastingSound();
    }

    //�������� ��������
    private void StartWaiting()
    {
        minWaitingTime = gameStats.minWaitingTime;
        maxWaitingTime = gameStats.maxWaitingTime;

        //��������� ����� ��������
        currWaitingTime = Random.Range(minWaitingTime, maxWaitingTime);

        waitingSlider.minValue = 0f;
        waitingSlider.value = 0f;
        waitingSlider.maxValue = currWaitingTime;

        //�������� ��������
        currState = GameState.waiting;
        isWaiting = true;
    }

    //����
    private void Waiting()
    {
        if (currState == GameState.waiting && isWaiting && !isPause)
        {
            if (waitingSlider.value <= waitingSlider.maxValue - 0.03f)
            {
                waitingSlider.value += Time.deltaTime;
            }
            else
            {
                //��������� ��������
                waitingSlider.value = waitingSlider.maxValue;
                isWaiting = false;

                WaitingViewAnimEnable(false);

                SelectRandomFish();
            }
        }
    }

    //������������ ��� �� ������ �������
    private void AllFishDeepSorting()
    {
        shallowFishes = new List<GameObject>();
        awerageDeepFishes = new List<GameObject>();
        deepFishes = new List<GameObject>();

        foreach (var fish in fishPrefabs)
        {
            FishController fc = fish.GetComponent<FishController>();
            switch (fc.CurrDepth)
            {
                case Depth.Shallow:
                    {
                        shallowFishes.Add(fish);
                        break;
                    }
                case Depth.AverageDepth:
                    {
                        awerageDeepFishes.Add(fish);
                        break;
                    }
                case Depth.Deep:
                    {
                        deepFishes.Add(fish);
                        break;
                    }
            }
        }
    }

    //�������� ��������� ���� � ����������� �� ������� � �������� �������
    private void SelectRandomFish()
    {
        GameObject fish;
        int randFish;

        switch (currDepth)
        {
            case Depth.Shallow:
                {
                    randFish = Random.Range(0, shallowFishes.Count);
                    fish = shallowFishes[randFish];
                    break;
                }
            case Depth.AverageDepth:
                {
                    randFish = Random.Range(0, awerageDeepFishes.Count);
                    fish = awerageDeepFishes[randFish];
                    break;
                }
            case Depth.Deep:
                {
                    randFish = Random.Range(0, deepFishes.Count);
                    fish = deepFishes[randFish];
                    break;
                }
            default:
                {
                    Debug.LogError("Undefinded Depth");
                    return;
                }
        }
        //StartFishing(fish);
        StartCoroutine(StartFishingAfterAnim(fish));
    }

    //��������� �������� ��� ������ ����� ������
    private IEnumerator StartFishingAfterAnim(GameObject fish)
    {
        cameraAnim.Play(cameraStartFishing.name);
        yield return new WaitForSeconds(cameraStartFishing.length);
        StartFishing(fish);
    }

    //�������� ������������
    private void StartFishing(GameObject fish)
    {
        currFish = fish;
        FishController fishController = fish.GetComponent<FishController>();

        //���������� ��������� ����������� �� ����
        loseTime = fishController.LoseTime;
        catchTime = fishController.CatchTime;
        targetSpeed = fishController.TargetSpeed;
        //����� ������� � ����
        inTargetTime = fishController.InTargetTime;
        //������� � ������� ������� �����
        inTargetRange = fishController.InTargetRange;

        //��������� ������ ����
        targetImage.transform.localScale = new Vector3(inTargetRange / 10f, targetImage.transform.localScale.y, targetImage.transform.localScale.z);

        //����������� � ������� ��������
        increaseAmount = gameStats.increaseAmount;
        decreaseSpeed = gameStats.decreaseSpeed;

        fishingSlider.value = 0f;
        currCatchingSlider.value = 0f;
        currMissingSlider.value = 0f;

        //Target Stats
        targetFishingSlider.value = 0f;
        currTargetValue = 0f;
        currTargetTime = 0f;

        currLoseTime = currCatchTime = 0f;
        isCatching = false;

        //��������� �������� ������
        playerController.StartFishing();

        SoundController.instance.PlayFishingSound();

        currState = GameState.fishing;
        //UpdateGameStateView();
        //isFishing = true;
        StartCoroutine(StartFishingAnim());
        NewTarget();
    }

    private IEnumerator StartFishingAnim()
    {
        fishingViewAnim.Play(fishingOnAnim.name);
        UpdateGameStateView();
        yield return new WaitForSeconds(fishingOnAnim.length);
        isFishing = true;
    }

    //��������� ��������� �������
    private void Fishing()
    {
        if (currState == GameState.fishing && isFishing && !isPause)
        {
            // ��������� �������� �������� �� ��������
            if (fishingSlider.value > fishingSlider.minValue)
            {
                fishingSlider.value -= decreaseSpeed * Time.deltaTime;
            }

            //������ ���� ��������
            if (IsCatching())
            {
                //�����
                fishingSliderFill.color = catchingColor;
                currCatchTime += Time.deltaTime;
            }
            else
            {
                //�� �����
                fishingSliderFill.color = losingColor;
                currLoseTime += Time.deltaTime;
            }

            //��������� �������� ������ � ������
            UpdateCurrSliders();

            //������� ��� ���������
            if (currCatchTime >= catchTime)
            {
                //�������
                EndFishing(true);
            }
            if (currLoseTime >= loseTime)
            {
                //��������
                EndFishing(false);
            }
        }
    }

    //��������� �������� �������� �������� ������ � �������� ������ ����
    private void UpdateCurrSliders()
    {
        float currCatchingValue = currCatchTime / catchTime;
        currCatchingSlider.value = currCatchingValue;

        float currMissingValue = currLoseTime / loseTime;
        currMissingSlider.value = currMissingValue;
    }

    //��������� �������
    private void EndFishing(bool isCatch)
    {
        isFishing = false;
        isGame = false;
        UpdateGameStateView();

        SoundController.instance.StopFishingSound();

        gameUIController.SetFish(currFish.GetComponent<FishController>());

        this.isCatch = isCatch;

        //��������� �������� ��������� �������
        playerController.EndFishig();
        cameraAnim.Play(cameraEndFishing.name);
        /*if (isCatch)
        {
            //Debug.Log("Catch");
            gameUIController.CoughtFish();
        }
        else
        {
            //Debug.Log("Lose");
            gameUIController.MissedFish();
        }*/
    }

    //��������� �������, �������� ������
    public void IsFishingEnd()
    {
        if (!isGame)
        {
            if (isCatch)
            {
                //Debug.Log("Catch");
                gameUIController.CoughtFish();
                SoundController.instance.PlaySuccessSound();
            }
            else
            {
                //Debug.Log("Lose");
                gameUIController.MissedFish();
                SoundController.instance.PlayMissedSound();
            }
        }
    }

    //����� �� ��� ������
    private bool IsCatching()
    {
        if (fishingSlider.value < targetFishingSlider.value + inTargetRange 
            && fishingSlider.value > targetFishingSlider.value - inTargetRange)
        {
            //� �������
            isCatching = true;
        }
        else
        {
            //�� � �������
            isCatching = false;
        }
        return isCatching;
    }

    //������ �������� ������ ��������
    private void TargetFishing()
    {
        if (isFishing && !isPause)
        {
            currTargetTime += Time.deltaTime;

            if (currTargetTime < inTargetTime)
            {
                //targetFishingSlider.value = Mathf.Lerp(initialTargetValue, currTargetValue, currTargetTime / inTargetTime);
                targetFishingSlider.value = Mathf.MoveTowards(targetFishingSlider.value, currTargetValue, targetSpeed * Time.deltaTime);
            }
            else
            {
                //����������� ����� - ������ ��������
                NewTarget();
            }
        }
    }

    //����� ����
    private void NewTarget()
    {
        currTargetValue = Random.Range(targetFishingSlider.minValue, targetFishingSlider.maxValue);
        currTargetTime = 0f;
    }

    //���������� �������� �� ������
    private void OnIncreaseFishingBtnClick()
    {
        if (!isPause)
        {
            // ����������� �������� �������� �� �������� ����������
            fishingSlider.value = Mathf.Min(fishingSlider.value + increaseAmount, fishingSlider.maxValue);
        }
    }

    //��������� �����������
    private void CastingEnd(Depth depth)
    {
        if (currState == GameState.casting)
        {
            currDepth = depth;
            //Debug.Log(currDepth);
            StartWaiting();
            UpdateGameStateView();
        }
    }

    //������������� �����
    private void SetPause(bool value)
    {
        isPause = value;
    }
}
