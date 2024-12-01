using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [Header("Main Settings")]
    [SerializeField] private Button backBtn;

    [Header("Sound Settings")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    [Header("Animation Settings")]
    [SerializeField] private Animation settingsAnim;
    [SerializeField] private AnimationClip settingsOn;
    [SerializeField] private AnimationClip settingsOff;

    [Header("Values")]
    private float musicVolume;
    private float soundVolume;

    [Header("View Settings")]
    [SerializeField] private GameObject settingsView;

    private GameSettings gameSettings;
    public static SettingsController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameSettings = GameSettings.instance;

        Load();

        backBtn.onClick.AddListener(Save);
        musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
        soundSlider.onValueChanged.AddListener(ChangeSoundVolume);
    }

    //загружаем сохраненные значения
    private void Load()
    {
        musicVolume = gameSettings.MusicVolume;
        soundVolume = gameSettings.SoundVolume;

        UpdateSliders();
    }

    //сохраняем значения
    private void Save()
    {
        gameSettings.MusicVolume = musicVolume;
        gameSettings.SoundVolume = soundVolume;

        SettingsOff();
    }

    //заходим
    public void SettingsOn()
    {

        //анимация и появление
        settingsAnim.Play(settingsOn.name);
    }

    //выходим в меню
    private void SettingsOff()
    {
        MenuController.instance.MenuOn();
        StartCoroutine(SettingsOffAnim());
    }

    //анимация ухода в меню
    private IEnumerator SettingsOffAnim()
    {
        settingsAnim.Play(settingsOff.name);
        yield return new WaitForSeconds(settingsOff.length);
        settingsView.SetActive(false);
    }

    //обновляем отображение слайдеров
    private void UpdateSliders()
    {
        musicSlider.value = musicVolume;
        soundSlider.value = soundVolume;
    }

    //меняем громкость музыки
    private void ChangeMusicVolume(float value)
    {
        musicVolume = value;
        SoundController.instance.ChangeMusicSound(musicVolume);
    }

    //меняем громкость звуков
    private void ChangeSoundVolume(float value)
    {
        soundVolume = value;
    }
}
