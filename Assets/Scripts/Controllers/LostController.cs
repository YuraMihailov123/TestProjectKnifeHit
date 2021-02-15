using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostController : MonoBehaviour
{
    #region Singleton
    private static LostController _instance;

    public static LostController Instance
    {
        get
        {
            try
            {
                if (_instance == null) _instance = GameObject.Find("Root").transform.Find("LostController").GetComponent<LostController>();
            }
            catch { }
            return _instance;
        }
    }
    #endregion

    public UIPanel mPanel;

    public void Init()
    {
        mPanel = GetComponent<UIPanel>();
    }

    public void OnRestartButtonClick()
    {
        Close();
        GameController.Instance.CreateNewKnifeToHit(false);
        GameController.Instance.Open();
    }

    public void Open()
    {
        gameObject.SetActive(true);
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
