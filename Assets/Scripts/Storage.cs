using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    #region Singleton
    private static Storage _instance;

    public static Storage Instance
    {
        get
        {
            try
            {
                if (_instance == null) _instance = GameObject.Find("Root").GetComponent<Storage>();
            }
            catch { }
            return _instance;
        }
    }
    #endregion

    public GameSettings mGameSettings;

    public int mAppleCount = 0;
    public int mMaxStage = 0;
    public int mMaxScore = 0;
    public string mSkins = "";

    public void Init()
    {
        
        mGameSettings = Resources.Load<GameSettings>("ScriptableObjects/GameSettings");
        PlayerPrefs.DeleteKey("skins");
        PlayerPrefs.DeleteKey("currentSkin");
        PlayerPrefs.SetInt("appleCount", 1000);
        LoadInfo();
    }

    public void LoadInfo()
    {
        if (PlayerPrefs.HasKey("appleCount"))
            mAppleCount = PlayerPrefs.GetInt("appleCount");
        else mAppleCount = 0;

        if (PlayerPrefs.HasKey("maxStageCount"))
            mMaxStage = PlayerPrefs.GetInt("maxStageCount");
        else mMaxStage = 0;

        if (PlayerPrefs.HasKey("maxScoreCount"))
            mMaxScore = PlayerPrefs.GetInt("maxScoreCount");
        else mMaxScore = 0;

        if (PlayerPrefs.HasKey("skins")) {
            mSkins = PlayerPrefs.GetString("skins");
        }else
        {
            mSkins = "0";
            PlayerPrefs.SetString("skins", "0");
            PlayerPrefs.SetInt("currentSkin", 0);
        }
    }

    public void SaveInfo(bool isAfterDeath, bool shouldSaveSkin)
    {
        if (isAfterDeath)
        {
            if (mMaxScore < GameController.Instance.mScore)
            {
                PlayerPrefs.SetInt("maxScoreCount", GameController.Instance.mScore);
                mMaxScore = GameController.Instance.mScore;
            }

            if (mMaxStage < GameController.Instance.mCurrentStage)
            {
                PlayerPrefs.SetInt("maxStageCount", GameController.Instance.mCurrentStage);
                mMaxStage = GameController.Instance.mCurrentStage;
            }
        }

        if (shouldSaveSkin)
        {
            if (PlayerPrefs.HasKey("skins"))
            {
                string str = PlayerPrefs.GetString("skins");
                List<string> strList = new List<string>(str.Split('|'));
                var currSkin = PlayerPrefs.GetInt("currentSkin");
                if (!strList.Contains(currSkin.ToString()))
                {
                    str += "|" + currSkin;
                    PlayerPrefs.SetString("skins", str);
                    mSkins = str;
                }
            }
        }

        PlayerPrefs.SetInt("appleCount", mAppleCount);
    }
}
