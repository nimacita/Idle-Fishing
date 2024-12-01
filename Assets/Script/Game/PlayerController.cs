using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [Header("Stats")]
    public GameStats gameStats;

    [Header("Components")]
    public GameObject fishRod;
    public GameObject cork;
    public CorkController corkController;
    public Animator playerAnimator;

    [Header("Skins")]
    public GameObject[] playerSkins;

    //bool
    private bool isPaused = false;

    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //RestartGameSettings();
        DefineSkin();
    }

    //Начальные настройки
    public void RestartGameSettings()
    {
        isPaused = false;
        playerAnimator.SetBool("Fishing", false);
    }

    //настройка скнинов
    private void DefineSkin()
    {
        for (int i = 0;i < playerSkins.Length;i++)
        {
            if (i == GameSettings.instance.SkinId)
            {
                playerSkins[i].SetActive(true);
            }
            else
            {
                playerSkins[i].SetActive(false);
            }
        }
    }


    //кинули удочку
    public void CastingRod()
    {
        playerAnimator.SetTrigger("Casting");
    }

    public void StartFishing()
    {
        playerAnimator.SetBool("Fishing", true);
    }

    public void EndFishig()
    {
        playerAnimator.SetTrigger("FishingEnd");
    }
}
