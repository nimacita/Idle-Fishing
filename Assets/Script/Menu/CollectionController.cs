using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionController : MonoBehaviour
{

    [Header("Main Settings")]
    [SerializeField] private Button backBtn;
    [SerializeField] private Button collectBtn;
    [SerializeField] private GameObject collectedBtn;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TMPro.TMP_Text progressTxt;

    [Header("Items")]
    public CollectItem[] collectItems;  

    [Header("Animation Settings")]
    [SerializeField] private Animation collectionAnim;
    [SerializeField] private AnimationClip collectionOn;
    [SerializeField] private AnimationClip collectionOff;

    [Header("View Settings")]
    [SerializeField] private GameObject collectionView;

    private bool canCollect = false;

    private GameSettings gameSettings;
    public static CollectionController instance;

    void Start()
    {
        gameSettings = GameSettings.instance;

        ButtonSettings();
        UpdateCollectBtn();
    }

    private void ButtonSettings()
    {
        backBtn.onClick.AddListener(CollectionOff);
        collectBtn.onClick.AddListener(CollectBtnClick);
    }

    private void UddateProgress()
    {
        int currFishes = GetCurrUnlockFishes();
        int maxFishes = gameSettings.fishCollect.allFishCount;

        progressSlider.maxValue = maxFishes;
        progressSlider.value = currFishes;

        progressTxt.text = $"{currFishes}/{maxFishes}";
    }

    private int GetCurrUnlockFishes()
    {
        if (gameSettings.fishCollect == null)
        {
            return 0;
        }
        int currFishes = 0;

        if(gameSettings.fishCollect.isAmberJack) currFishes++;
        if(gameSettings.fishCollect.isArapaima) currFishes++;
        if(gameSettings.fishCollect.isArcher) currFishes++;
        if(gameSettings.fishCollect.isAtlanticCod) currFishes++;
        if(gameSettings.fishCollect.isBarracuda) currFishes++;
        if(gameSettings.fishCollect.isBlackRedeye) currFishes++;
        if(gameSettings.fishCollect.isBlackSpotted) currFishes++;
        if(gameSettings.fishCollect.isBrownTrout) currFishes++;
        if(gameSettings.fishCollect.isBullShark) currFishes++;
        if(gameSettings.fishCollect.isCarp) currFishes++;
        if(gameSettings.fishCollect.isCobia) currFishes++;
        if(gameSettings.fishCollect.isRedTang) currFishes++;

        if (currFishes > gameSettings.fishCollect.allFishCount)
        {
            currFishes = gameSettings.fishCollect.allFishCount;
        }

        return currFishes;
    }

    private void UpdateCollectBtn()
    {
        if (!gameSettings.IsCollectSkinOpened)
        {
            collectBtn.gameObject.SetActive(true);
            collectedBtn.gameObject.SetActive(false);

            if (GetCurrUnlockFishes() >= gameSettings.fishCollect.allFishCount)
            {
                canCollect = true;
            }
            else
            {
                canCollect = false;
                //collectBtn.gameObject.SetActive(false);
            }
            collectBtn.interactable = canCollect;
        }
        else
        {
            collectBtn.gameObject.SetActive(false);
            collectedBtn.gameObject.SetActive(true);
        }
    }

    private void UpdateAllItems()
    {
        foreach (var item in collectItems)
        {
            item.UpdateItemView();
        }
    }

    //заходим
    public void CollectionOn()
    {
        UddateProgress();
        UpdateCollectBtn();
        UpdateAllItems();

        //анимация и появление
        collectionAnim.Play(collectionOn.name);
    }

    //выходим в меню
    private void CollectionOff()
    {
        MenuController.instance.MenuOn();
        StartCoroutine(CollectionOffAnim());
    }

    //анимация ухода в меню
    private IEnumerator CollectionOffAnim()
    {
        collectionAnim.Play(collectionOff.name);
        yield return new WaitForSeconds(collectionOff.length);
        collectionView.SetActive(false);
    }

    //нажали на кнопку собрать
    private void CollectBtnClick()
    {
        if (!gameSettings.IsCollectSkinOpened && canCollect)
        {
            gameSettings.IsCollectSkinOpened = true;
            UpdateCollectBtn();
            SoundController.instance.PlayCollectBonusSound();
        }
    }
}
