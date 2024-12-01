using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public enum Depth
{
    //мелководье
    Shallow,
    //—редн€€ √лубина
    AverageDepth,
    //√лубоко
    Deep
}

public enum FishType
{
    AmberJack,
    Arapaima,
    ArcherFish,
    Atlantic,
    Barracuda,
    BlackRedeye,
    BlackSpottedGrunt,
    BrownTrout,
    BullShark,
    Carp,
    Cobia,
    RedTang
}

//класс с данными 
public class FishCollect
{
    public bool isAmberJack = false;
    public bool isArapaima = false;
    public bool isArcher = false;
    public bool isAtlanticCod = false;
    public bool isBarracuda = false;
    public bool isBlackRedeye = false;
    public bool isBlackSpotted = false;
    public bool isBrownTrout = false;
    public bool isBullShark = false;
    public bool isCarp = false;
    public bool isCobia = false;
    public bool isRedTang = false;

    public int allFishCount = 12;
}

public class GameSettings : MonoBehaviour
{

    static public string savedDataPath;
    public static GameSettings instance;
    public FishCollect fishCollect = new FishCollect();

    void Awake()
    {
        

        if (!instance)
            instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        StartSaveSettings();
    }


    //сохраненное значение громкости музыки
    public float MusicVolume
    {
        get
        {
            if (!PlayerPrefs.HasKey("MusicVolume"))
            {
                PlayerPrefs.SetFloat("MusicVolume", 1f);
            }
            return PlayerPrefs.GetFloat("MusicVolume");
        }

        set
        {
            if (value > 1f) value = 1f;
            if (value < 0f) value = 0f;
            PlayerPrefs.SetFloat("MusicVolume", value);
        }
    }

    //сохраненное значение громкости звуков
    public float SoundVolume
    {
        get
        {
            if (!PlayerPrefs.HasKey("SoundVolume"))
            {
                PlayerPrefs.SetFloat("SoundVolume", 1f);
            }
            return PlayerPrefs.GetFloat("SoundVolume");
        }

        set
        {
            if (value > 1f) value = 1f;
            if (value < 0f) value = 0f;
            PlayerPrefs.SetFloat("SoundVolume", value);
        }
    }

    //сохраненное значение монет
    public int Coins
    {
        get
        {
            if (!PlayerPrefs.HasKey("Coins"))
            {
                PlayerPrefs.SetInt("Coins", 0);
            }
            return PlayerPrefs.GetInt("Coins");
        }

        set
        {
            PlayerPrefs.SetInt("Coins", value);
        }
    }

    //сохраненное значение выбранного скина
    public int SkinId
    {
        get
        {
            if (!PlayerPrefs.HasKey("CurrentSkin"))
            {
                PlayerPrefs.SetInt("CurrentSkin", 0);
            }
            return PlayerPrefs.GetInt("CurrentSkin");
        }

        set
        {
            PlayerPrefs.SetInt("CurrentSkin", value);
        }
    }

    //значение открытого скина из коллекции
    public bool IsCollectSkinOpened
    {
        get
        {
            if (!PlayerPrefs.HasKey("IsCollectSkinOpened"))
            {
                PlayerPrefs.SetInt("IsCollectSkinOpened",0);
                return false;
            }
            else
            {
                if (PlayerPrefs.GetInt("IsCollectSkinOpened") == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        set
        {
            if (!value)
            {
                PlayerPrefs.SetInt("IsCollectSkinOpened", 0);
            }
            else
            {
                PlayerPrefs.SetInt("IsCollectSkinOpened", 1);
            }
        }
    }


    //открыта ли рыба по типу
    public bool IsFishOpen(FishType fishType)
    {
        bool isOpen = false;

        if (fishCollect == null) LoadFishCollectData();

        switch (fishType)
        {
            case FishType.AmberJack: 
                isOpen = fishCollect.isAmberJack;
                break;
            case FishType.Arapaima: 
                isOpen = fishCollect.isArapaima;
                break;
            case FishType.ArcherFish:
                isOpen = fishCollect.isArcher;
                break;
            case FishType.Atlantic:
                isOpen = fishCollect.isAtlanticCod;
                break;
            case FishType.Barracuda:
                isOpen = fishCollect.isBarracuda;
                break;
            case FishType.BlackRedeye:
                isOpen = fishCollect.isBlackRedeye;
                break;
            case FishType.BlackSpottedGrunt:
                isOpen = fishCollect.isBlackSpotted;
                break;
            case FishType.BrownTrout:
                isOpen = fishCollect.isBrownTrout;
                break;
            case FishType.BullShark:
                isOpen = fishCollect.isBullShark;
                break;
            case FishType.Carp:
                isOpen = fishCollect.isCarp;
                break;
            case FishType.Cobia:
                isOpen = fishCollect.isCobia;
                break;
            case FishType.RedTang:
                isOpen = fishCollect.isRedTang;
                break;
        }

        return isOpen;
    }

    //сохран€ем рыбу
    public void SetCollectFish(FishType fishType)
    {
        switch (fishType)
        {
            case FishType.AmberJack:
                if(!fishCollect.isAmberJack) fishCollect.isAmberJack = true;
                break;
            case FishType.Arapaima:
                if (!fishCollect.isArapaima) fishCollect.isArapaima = true;
                break;
            case FishType.ArcherFish:
                if (!fishCollect.isArcher) fishCollect.isArcher = true;
                break;
            case FishType.Atlantic:
                if (!fishCollect.isAtlanticCod) fishCollect.isAtlanticCod = true;
                break;
            case FishType.Barracuda:
                if (!fishCollect.isBarracuda) fishCollect.isBarracuda = true;
                break;
            case FishType.BlackRedeye:
                if (!fishCollect.isBlackRedeye) fishCollect.isBlackRedeye = true;
                break;
            case FishType.BlackSpottedGrunt:
                if (!fishCollect.isBlackSpotted) fishCollect.isBlackSpotted = true;
                break;
            case FishType.BrownTrout:
                if (!fishCollect.isBrownTrout) fishCollect.isBrownTrout = true;
                break;
            case FishType.BullShark:
                if (!fishCollect.isBullShark) fishCollect.isBullShark = true;
                break;
            case FishType.Carp:
                if (!fishCollect.isCarp) fishCollect.isCarp = true;
                break;
            case FishType.Cobia:
                if (!fishCollect.isCobia) fishCollect.isCobia = true;
                break;
            case FishType.RedTang:
                if (!fishCollect.isRedTang) fishCollect.isRedTang = true;
                break;
        }

        SaveFishCollectData();
        LoadFishCollectData();
    }

    //путь дл€ файла с сохранени€ми
    private void StartSaveSettings()
    {
        //записываем путь
        savedDataPath = Path.Combine(Application.dataPath, "FishCollect.json");

        //выгружаем данные
        LoadFishCollectData();

        if (fishCollect == null)
        {
            fishCollect = new FishCollect();
            SaveFishCollectData();
        }

    }

    //выгружаем даные из json файла
    private void LoadFishCollectData()
    {
        fishCollect = new FishCollect();
        fishCollect = SaveAndLoadData.Load<FishCollect>(savedDataPath);
    }

    //private FishCollect fc 

    //сохран€ем данные
    private void SaveFishCollectData()
    {
        //сохран€ем
        SaveAndLoadData.Save(fishCollect, savedDataPath);
    }
}
