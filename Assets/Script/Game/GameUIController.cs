using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{

    [Header("Game View")]
    public TMPro.TMP_Text fishCount;
    public Button pauseBtn;
    private int currFishCount;

    [Header("Pause View")]
    public GameObject pauseView;
    public Button pauseRestartBtn;
    public Button pauseNextBtn;
    public Button pauseMenuBtn;
    public Animation pauseAnim;
    public AnimationClip pauseOn;
    public AnimationClip pauseOff;

    [Header("Cought Fish View")]
    public GameObject coughtView;
    public TMPro.TMP_Text coughtFishName;
    public Image coughtFishImg;
    public TMPro.TMP_Text coughtCoinTxt;
    public Button coughtNextBtn;
    public Button coughtMenuBtn;
    public GameObject newTxt;
    public Animation caughtViewAnim;
    public AnimationClip caughtViewOn;
    public AnimationClip caughtViewOff;

    [Header("Missed Fish View")]
    public GameObject missedView;
    public TMPro.TMP_Text missedFishName;
    public Image missedFishImg;
    public Button missedNextBtn;
    public Button missedMenuBtn;
    public Animation missedViewAnim;
    public AnimationClip missedViewOn;
    public AnimationClip missedViewOff;

    [Header("Components")]
    public GameObject mainCamera;

    private FishController currFish;

    //bools
    private bool isCought;
    private bool isMissed;
    private bool isPause;

    public static Action<bool> isPauseSet;

    void Start()
    {
        currFishCount = 0;
        UpdateFishCount();

        ButtonSettings();
        StartSettings();
    }

    //настройка кнопок
    private void ButtonSettings()
    {
        coughtNextBtn.onClick.AddListener(CoughtNextBtnClick);
        missedNextBtn.onClick.AddListener(MissedNextBtnClick);
        coughtMenuBtn.onClick.AddListener(MenuBtnClick);
        missedMenuBtn.onClick.AddListener(MenuBtnClick);
        pauseBtn.onClick.AddListener(PauseViewOn);

        pauseMenuBtn.onClick.AddListener(MenuBtnClick);
        pauseNextBtn.onClick.AddListener(PauseViewOff);
        pauseRestartBtn.onClick.AddListener(Restart);
    }

    //начальные настройки
    public void StartSettings()
    {
        isCought = false;
        isMissed = false;
        isPause = false;

        coughtView.SetActive(false);
        missedView.SetActive(false);
        pauseView.SetActive(false);
    }

    //устанавливаем рыбу которую поймали
    public void SetFish(FishController fishController)
    {
        currFish = fishController;
    }

    //обновляем количества текста рыб
    private void UpdateFishCount()
    {
        fishCount.text = $"{currFishCount}";
    }

    private void PauseViewOn()
    {
        isPauseSet?.Invoke(true);

        pauseView.SetActive(true);
        pauseAnim.Play(pauseOn.name);
    }

    private void PauseViewOff()
    {
        StartCoroutine(PauseOffAnim());
    }

    private IEnumerator PauseOffAnim()
    {
        pauseAnim.Play(pauseOff.name);
        yield return new WaitForSeconds(pauseOff.length);
        isPauseSet?.Invoke(false);
        pauseView.SetActive(false);
    }

    //запуксаем экран пойманной рыбы
    public void CoughtFish()
    {
        coughtFishName.text = currFish.fishName;
        coughtFishImg.sprite = currFish.fishImage;
        coughtCoinTxt.text = $"{currFish.fishPrice}";

        GameSettings.instance.Coins += (int)(currFish.fishPrice);

        //если новая
        if (!GameSettings.instance.IsFishOpen(currFish.CurrFishType))
        {
            newTxt.SetActive(true);
        }
        else
        {
            newTxt.SetActive(false);
        }
        //записываем пойманную рыбу
        GameSettings.instance.SetCollectFish(currFish.CurrFishType);

        currFishCount++;
        UpdateFishCount();

        coughtView.SetActive(true);

        caughtViewAnim.Play(caughtViewOn.name);
    }

    //запускаем экран упущенной рыбы
    public void MissedFish()
    {
        missedFishName.text = currFish.fishName;
        missedFishImg.sprite = currFish.fishImage;

        missedView.SetActive(true);

        missedViewAnim.Play(missedViewOn.name);
    }

    //нажали на кнопку далее
    private void CoughtNextBtnClick()
    {
        StartCoroutine(CaughtNextBtnAnim());
        GameController.instance.StartGameSettings();
    }

    //анимация после нажатия на Next
    private IEnumerator CaughtNextBtnAnim()
    {
        caughtViewAnim.Play(caughtViewOff.name);
        yield return new WaitForSeconds(caughtViewOff.length);
        StartSettings();
    }

    //нажали на кнопку далее
    private void MissedNextBtnClick()
    {
        StartCoroutine(MissedNextBtnAnim());
        GameController.instance.StartGameSettings();
    }

    //анимация после нажатия на Next
    private IEnumerator MissedNextBtnAnim()
    {
        missedViewAnim.Play(missedViewOff.name);
        yield return new WaitForSeconds(missedViewOff.length);
        StartSettings();
    }

    //нажали на кнопку меню
    private void MenuBtnClick()
    {
        SoundController.instance.StopAmbientSound();
        StartCoroutine(openScene("MainMenu"));
    }

    //перезапускаем уровень
    private void Restart()
    {
        StartCoroutine(openScene(SceneManager.GetActiveScene().name));
    }


    //открываем сцену после задержки для перехода
    IEnumerator openScene(string sceneName)
    {
        float fadeTime = mainCamera.GetComponent<SceneFading>().StartFading(1);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
}
