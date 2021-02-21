using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    #region Singleton
    private static MenuController _instance;

    public static MenuController Instance
    {
        get
        {
            try
            {
                if (_instance == null) _instance = GameObject.Find("Root").transform.Find("MenuController").GetComponent<MenuController>();
            }
            catch { }
            return _instance;
        }
    }
    #endregion

    public UIPanel mPanel;

    public UISprite mKnifeIcon;

    public void OnSkinsButtonClick()
    {
        Close();
        SkinsController.Instance.Open();
    }

    public void OnPlayButtonClick()
    {
        Close();
        GameController.Instance.Open();
    }

    public void Init()
    {
        mKnifeIcon = transform.Find("knifeIcon").GetComponent<UISprite>();
        mPanel = GetComponent<UIPanel>();
    }

    public void Open()
    {
        gameObject.SetActive(true);
        GameController.Instance.DetermineSkin();
        StartCoroutine("Open_Coroutine");
    }

    IEnumerator Open_Coroutine()
    {
        for (int i = 0; i < 10; i++)
        {
            mPanel.alpha += 0.1f;
            yield return new WaitForSeconds(0.001f);
        }
    }

    public void Close()
    {
        StartCoroutine("Close_Coroutine");
    }

    IEnumerator Close_Coroutine()
    {
        for (int i = 0; i < 10; i++)
        {
            mPanel.alpha -= 0.1f;
            yield return new WaitForSeconds(0.001f);
        }
        mPanel.alpha = 0;
        gameObject.SetActive(false);
    }
}
