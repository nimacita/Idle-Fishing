using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioSource BgMusic;

    [Header("In Game Sounds")]
    [SerializeField] private AudioSource splashSound;
    [SerializeField] private AudioSource[] castingRodSounds;
    [SerializeField] private AudioSource fishingSound;
    [SerializeField] private AudioSource successSound;
    [SerializeField] private AudioSource missedSound;
    [SerializeField] private AudioSource ambientSounds;

    [Header("Addcoins")]
    [SerializeField] private AudioSource addCoins;
    [SerializeField] private AudioSource equipSound;
    [SerializeField] private AudioSource collectBonusSound;

    [Header("Button Sounds")]
    [SerializeField] private AudioSource btnClickSound;


    public static SoundController instance;

    void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this.gameObject);


        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        PlayBgMusic();
    }

    //играме музыку
    private void PlayBgMusic()
    {
        BgMusic.volume = GameSettings.instance.MusicVolume;
        BgMusic.Play();
    }

    //настройка звука мущыки
    public void ChangeMusicSound(float volume)
    {
        BgMusic.volume = volume;
    }

    //играем выбранный звук
    private void PlayCurrSound(AudioSource sound)
    {
        sound.volume = GameSettings.instance.SoundVolume;
        sound.Play();
    }

    public void PlayAddCoins() { PlayCurrSound(addCoins); }
    public void PlayEquipSound() { PlayCurrSound(equipSound); }
    public void PlayCollectBonusSound() { PlayCurrSound(collectBonusSound); }
    public void PlayBtnClickSound() { PlayCurrSound(btnClickSound); }
    public void PlayFishingSound() { PlayCurrSound(fishingSound); }
    public void StopFishingSound() { fishingSound.Stop(); }
    public void PlaySuccessSound() { PlayCurrSound(successSound); }
    public void PlayMissedSound() { PlayCurrSound(missedSound); }
    public void PlaySplashSound() { PlayCurrSound(splashSound); }
    public void PlayAmbientSound() 
    {
        if (!ambientSounds.isPlaying)
        {
            PlayCurrSound(ambientSounds);
        }
    }
    public void StopAmbientSound() { ambientSounds.Stop(); }

    public void PlayCastingSound()
    {
        int randSound = Random.Range(0, castingRodSounds.Length);
        PlayCurrSound(castingRodSounds[randSound]);
    }


}
