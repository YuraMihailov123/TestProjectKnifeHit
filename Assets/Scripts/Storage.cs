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

    public void Init()
    {
        mGameSettings = Resources.Load<GameSettings>("ScriptableObjects/GameSettings");
    }
}
