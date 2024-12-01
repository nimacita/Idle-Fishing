using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{

    [Header("Shop View")]
    [SerializeField] private GameObject shopView;
    [SerializeField] private Button backBtn;
    [SerializeField] private TMPro.TMP_Text coinTxt;

    [Header("Animations")]
    [SerializeField] private Animation shopAnim;
    [SerializeField] private AnimationClip shopOnAnim;
    [SerializeField] private AnimationClip shopOffAnim;

    [Header("PopUp")]
    [SerializeField] private GameObject popUpPanel;
    [SerializeField] private GameObject notEnoughtMoney;
    [SerializeField] private Button nemBackBtn;
    [SerializeField] private GameObject skinNotAvailable;
    [SerializeField] private Button snaBackBtn;
    [SerializeField] private Animation popUpAnim;
    [SerializeField] private AnimationClip popUpOn;
    [SerializeField] private AnimationClip popUpOff;

    public static ShopController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        shopView.SetActive(false);
        popUpPanel.SetActive(false);

        UpdateCoins();

        backBtn.onClick.AddListener(ShopOff);
        nemBackBtn.onClick.AddListener(ClosePopUp);
        snaBackBtn.onClick.AddListener(ClosePopUp);
    }


    //включаем экран магазина
    public void ShopOn()
    {
        shopView.SetActive(true);
        UpdateCoins();
        StartCoroutine(ShopOnAnim());
    }

    private IEnumerator ShopOnAnim()
    {
        shopAnim.Play(shopOnAnim.name);
        yield return new WaitForSeconds(shopOnAnim.length);
    }

    //выключаем экран магазина 
    private void ShopOff()
    {
        MenuController.instance.MenuOn();
        StartCoroutine(ShopOffAnim());
    }

    public void UpdateCoins()
    {
        coinTxt.text = $"{GameSettings.instance.Coins}";
    }

    //выключаем экран магазина анимация
    private IEnumerator ShopOffAnim()
    {
        shopAnim.Play(shopOffAnim.name);
        yield return new WaitForSeconds(shopOffAnim.length);
        shopView.SetActive(false);
    }

    //активируем поп ап про нехватку денег
    public void NotEnoughtMoneyPopUp()
    {
        notEnoughtMoney.SetActive(true);
        skinNotAvailable.SetActive(false);

        popUpPanel.SetActive(true);
        popUpAnim.Play(popUpOn.name);
    }

    //активируем поп ап про закрытый скин
    public void SkinNotAvailablePopUp()
    {
        notEnoughtMoney.SetActive(false);
        skinNotAvailable.SetActive(true);

        popUpPanel.SetActive(true);
        popUpAnim.Play(popUpOn.name);
    }

    private void ClosePopUp()
    {
        StartCoroutine(ClosePopUpAnim());
    }

    private IEnumerator ClosePopUpAnim()
    {
        popUpAnim.Play(popUpOff.name);
        yield return new WaitForSeconds(popUpOff.length);

        notEnoughtMoney.SetActive(false);
        skinNotAvailable.SetActive(false);
        popUpPanel.SetActive(false);
    }
}
