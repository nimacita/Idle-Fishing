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
    //мелководные рыбы
    private List<GameObject> shallowFishes;
    [SerializeField]
    //Средневодные рыбы
    private List<GameObject> awerageDeepFishes;
    [SerializeField]
    //Глубоководные рыбы
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
    //необходимое время чтоб проиграть
    private float loseTime;
    private float currLoseTime;
    //необходимое время чтоб поймать
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

    //начальные настроойки игры
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

    //настройки кнопок
    private void ButtonSettings()
    {
        castingBtn.onClick.AddListener(CastingRod);

        fishingBtn.onClick.AddListener(OnIncreaseFishingBtnClick);
    }

    //обновляем экран в зависимости от состояния
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

    //анимация кастинг вью
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

    //анимация ожидания вью
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

    //начинаем закидывание
    private void StartCasting()
    {
        //значения из стат
        flightAngle = gameStats.flightAngle;

        forceSpeed = gameStats.forceSpeed;
        minForce = gameStats.minForce;
        maxForce = gameStats.maxForce;

        //начальные настройки поплавка
        corkController.StartSettings();

        //устанавливаем мин и макс значения слайдера
        forceSlider.minValue = minForce;
        forceSlider.maxValue = maxForce;
        forceSlider.value = minForce;

        isCasting = true;
        currState = GameState.casting;
    }

    //выбираем силу кидания вверх
    private void Casting()
    {
        if (isCasting && currState == GameState.casting && !isPause)
        {
            float t = Mathf.PingPong(Time.time * forceSpeed, 1);
            forceSlider.value = Mathf.Lerp(minForce, maxForce, t);

            float normalizedValue = (forceSlider.value - minForce) / (maxForce - minForce);

            // Получаем цвет из градиента и применяем его к компоненту Image
            Color newColor = forceSliderGrad.Evaluate(normalizedValue);
            sliderFill.color = newColor;

            // Изменяем поворот и масштаб слайдера
            //float newRotation = Mathf.Lerp(minRotation, maxRotation, normalizedValue);
            float newScale = Mathf.Lerp(minScale, maxScale, normalizedValue);

            // Применяем поворот и масштаб к RectTransform слайдера
            //sliderRectTransform.localRotation = Quaternion.Euler(0, 0, newRotation);
            sliderRectTransform.localScale = new Vector3(newScale, newScale, 1);
        }
    }

    //кинули удочку
    public void CastingRod()
    {
        if (isCasting && currState == GameState.casting && !isFishing)
        {
            currForce = forceSlider.value;


            //запуксаем анимацию удочки
            playerController.CastingRod();

            CastingViewAnimEnable(false);

            isCasting = false;
        }
    }

    //забарсываем поплавок
    public void ThrowCork()
    {
        corkController.Threw(currForce, flightAngle);
        SoundController.instance.PlayCastingSound();
    }

    //начинаем ожидание
    private void StartWaiting()
    {
        minWaitingTime = gameStats.minWaitingTime;
        maxWaitingTime = gameStats.maxWaitingTime;

        //случайное время ожидания
        currWaitingTime = Random.Range(minWaitingTime, maxWaitingTime);

        waitingSlider.minValue = 0f;
        waitingSlider.value = 0f;
        waitingSlider.maxValue = currWaitingTime;

        //начинаем ожидание
        currState = GameState.waiting;
        isWaiting = true;
    }

    //ждем
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
                //закончили ожидание
                waitingSlider.value = waitingSlider.maxValue;
                isWaiting = false;

                WaitingViewAnimEnable(false);

                SelectRandomFish();
            }
        }
    }

    //распределяем рыб по листам глубины
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

    //выбираем случайную рыбу в зависимости от глубины и начинаем рыбалку
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

    //запускаем анимацию для камеры перед ловлей
    private IEnumerator StartFishingAfterAnim(GameObject fish)
    {
        cameraAnim.Play(cameraStartFishing.name);
        yield return new WaitForSeconds(cameraStartFishing.length);
        StartFishing(fish);
    }

    //начинаем вылавливание
    private void StartFishing(GameObject fish)
    {
        currFish = fish;
        FishController fishController = fish.GetComponent<FishController>();

        //Изменяемые значенияв зависимости от рыбы
        loseTime = fishController.LoseTime;
        catchTime = fishController.CatchTime;
        targetSpeed = fishController.TargetSpeed;
        //время стояния у цели
        inTargetTime = fishController.InTargetTime;
        //область в которой считаем ловлю
        inTargetRange = fishController.InTargetRange;

        //визуально меняем цель
        targetImage.transform.localScale = new Vector3(inTargetRange / 10f, targetImage.transform.localScale.y, targetImage.transform.localScale.z);

        //прибавления и падение слайдера
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

        //запуксаем анимацию игрока
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

    //управляем слайдером рыбалки
    private void Fishing()
    {
        if (currState == GameState.fishing && isFishing && !isPause)
        {
            // Уменьшаем значение слайдера со временем
            if (fishingSlider.value > fishingSlider.minValue)
            {
                fishingSlider.value -= decreaseSpeed * Time.deltaTime;
            }

            //меняем цвет слайдера
            if (IsCatching())
            {
                //ловим
                fishingSliderFill.color = catchingColor;
                currCatchTime += Time.deltaTime;
            }
            else
            {
                //не ловим
                fishingSliderFill.color = losingColor;
                currLoseTime += Time.deltaTime;
            }

            //обновляем слайдеры поимке и потери
            UpdateCurrSliders();

            //поймали или проиграли
            if (currCatchTime >= catchTime)
            {
                //поймали
                EndFishing(true);
            }
            if (currLoseTime >= loseTime)
            {
                //упустили
                EndFishing(false);
            }
        }
    }

    //обновляем слайдеры текущего процента поимки и процента потери рыбы
    private void UpdateCurrSliders()
    {
        float currCatchingValue = currCatchTime / catchTime;
        currCatchingSlider.value = currCatchingValue;

        float currMissingValue = currLoseTime / loseTime;
        currMissingSlider.value = currMissingValue;
    }

    //закончили рыбалку
    private void EndFishing(bool isCatch)
    {
        isFishing = false;
        isGame = false;
        UpdateGameStateView();

        SoundController.instance.StopFishingSound();

        gameUIController.SetFish(currFish.GetComponent<FishController>());

        this.isCatch = isCatch;

        //запускаем анимацию окончания рыбалки
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

    //закончили рыбалку, включаем экраны
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

    //ловим ли или теряем
    private bool IsCatching()
    {
        if (fishingSlider.value < targetFishingSlider.value + inTargetRange 
            && fishingSlider.value > targetFishingSlider.value - inTargetRange)
        {
            //в области
            isCatching = true;
        }
        else
        {
            //не в области
            isCatching = false;
        }
        return isCatching;
    }

    //меняем значения таргет слайдера
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
                //закончилось время - меняем значение
                NewTarget();
            }
        }
    }

    //новая цель
    private void NewTarget()
    {
        currTargetValue = Random.Range(targetFishingSlider.minValue, targetFishingSlider.maxValue);
        currTargetTime = 0f;
    }

    //прибавляем значение на кнопку
    private void OnIncreaseFishingBtnClick()
    {
        if (!isPause)
        {
            // Увеличиваем значение слайдера на заданное количество
            fishingSlider.value = Mathf.Min(fishingSlider.value + increaseAmount, fishingSlider.maxValue);
        }
    }

    //закончили забрасывать
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

    //устанавливаем паузу
    private void SetPause(bool value)
    {
        isPause = value;
    }
}
