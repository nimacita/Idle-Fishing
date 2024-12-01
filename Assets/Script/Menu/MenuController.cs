using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Currency")]
    [SerializeField] private TMPro.TMP_Text coinTxt;

    [Header("Menu Settings")]
    [SerializeField] private Button playBtn;
    [SerializeField] private Button settingsBtn;
    [SerializeField] private Button shopBtn;
    [SerializeField] private Button collectionBtn;

    [Header("Animation Settings")]
    [SerializeField] private Animation menuAnim;
    [SerializeField] private AnimationClip menuOn;
    [SerializeField] private AnimationClip menuOff;
    [SerializeField] private AnimationClip menuOffToNext;

    [Header("Views Settings")]
    [SerializeField] private GameObject menuView;
    [SerializeField] private ShopController shopController;
    [SerializeField] private GameObject settingsView;
    [SerializeField] private SettingsController settingsController;
    [SerializeField] private GameObject collectionView;
    [SerializeField] private CollectionController collectionController;

    [Header("Components")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private PlayerSkin playerSkin;

    [Header("Player Animation")]
    public Animator playerAnim;
    public AnimationClip playerStandAnim;

    public static MenuController instance;

    private void Awake()
    {
        instance = this;
        StartViewSettings();
    }

    void Start()
    {
        playerAnim.SetBool("Stand", false);

        playerSkin.DefineSkin();

        ButtonSettings();
        UpdateCurrency();
    }

    //��������� ��������� �������
    private void StartViewSettings()
    {
        menuView.SetActive(true);
        settingsView.SetActive(false);
        collectionView.SetActive(false);
    }

    //��������� ������
    private void ButtonSettings()
    {
        playBtn.onClick.AddListener(PlayClick);
        settingsBtn.onClick.AddListener(SettingsClick);
        shopBtn.onClick.AddListener(ShopClick);
        collectionBtn.onClick.AddListener(CollectionClick);
    }

    //��������� �������� ������
    public void UpdateCurrency()
    {
        coinTxt.text = $"{GameSettings.instance.Coins}";
    }

    //����������� � ���� � ���������
    public void MenuOn()
    {
        playerSkin.DefineSkin();
        menuView.SetActive(true);
        StartCoroutine(MenuOnAnim());

        UpdateCurrency();
    }

    private IEnumerator MenuOnAnim()
    {
        menuAnim.Play(menuOn.name);
        yield return new WaitForSeconds(menuOn.length);
        ItteractBtn(true);
    }

    //��������� ����
    private IEnumerator MenuOff()
    {
        //������ ��������
        menuAnim.Play(menuOff.name);
        ItteractBtn(false);
        yield return new WaitForSeconds(menuOff.length);
        menuView.SetActive(false);
    }

    private IEnumerator MenuOffNext()
    {
        //������ ��������
        menuAnim.Play(menuOffToNext.name);
        ItteractBtn(false);
        yield return new WaitForSeconds(menuOffToNext.length);
        menuView.SetActive(false);
    }

    //������� �� ������ ������
    private void PlayClick()
    {
        StartCoroutine(PlayerStand());
    }

    private IEnumerator PlayerStand()
    {
        playerAnim.SetBool("Stand", true);
        menuAnim.Play(menuOff.name);
        yield return new WaitForSeconds(playerStandAnim.length /2f);
        StartCoroutine(openScene("GameScene"));
    }

    //������� �� ������ ��������
    private void SettingsClick()
    {
        StartCoroutine(SettingsViewOn());
    }

    private IEnumerator SettingsViewOn()
    {
        StartCoroutine(MenuOffNext());
        yield return new WaitForSeconds(menuOffToNext.length / 2f);
        settingsView.SetActive(true);
        settingsController.SettingsOn();
    }

    //������� �� ������ ������
    private void CollectionClick()
    {
        StartCoroutine(CollectionViewOn());
    }

    private IEnumerator CollectionViewOn()
    {
        StartCoroutine(MenuOffNext());
        yield return new WaitForSeconds(menuOffToNext.length / 2f);
        collectionView.SetActive(true);
        collectionController.CollectionOn();
    }

    //������� �� ������ ��������
    private void ShopClick()
    {
        StartCoroutine(ShopViewOn());
    }

    private IEnumerator ShopViewOn()
    {
        StartCoroutine(MenuOffNext());
        yield return new WaitForSeconds(menuOffToNext.length / 2f);
        shopController.ShopOn();
    }

    private void ItteractBtn(bool value)
    {
        playBtn.interactable = value;
        settingsBtn.interactable = value;
        shopBtn.interactable = value;
        collectionBtn.interactable = value;
    }

    //��������� ����� ����� �������� ��� ��������
    IEnumerator openScene(string sceneName)
    {
        float fadeTime = mainCamera.GetComponent<SceneFading>().StartFading(1);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
}
