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

    public void Init()
    {
        
        mGameSettings = Resources.Load<GameSettings>("ScriptableObjects/GameSettings");

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
    }

    public void SaveInfo(bool isAfterDeath)
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

        PlayerPrefs.SetInt("appleCount", mAppleCount);
    }
}
