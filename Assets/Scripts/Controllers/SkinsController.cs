using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsController : MonoBehaviour
{
    #region Singleton
    private static SkinsController _instance;

    public static SkinsController Instance
    {
        get
        {
            try
            {
                if (_instance == null) _instance = GameObject.Find("Root").transform.Find("SkinsController").GetComponent<SkinsController>();
            }
            catch { }
            return _instance;
        }
    }
    #endregion

    public UIPanel mPanel;

    

    private UIGrid mUIGrid;
    private UICenterOnChild mChildCenter;

    public GameObject[] mSkinsPrefab;
    private GameObject mScrollView;

    public void Init()
    {
        mSkinsPrefab = Resources.LoadAll<GameObject>("Prefabs/skins");

        mScrollView = transform.Find("ScrollView").gameObject;
        mUIGrid = mScrollView.transform.Find("UIGrid").GetComponent<UIGrid>();
        mChildCenter = mScrollView.transform.Find("UIGrid").GetComponent<UICenterOnChild>();

        mPanel = GetComponent<UIPanel>();

        AddSkinsToController();
    }

    public void OnChooseButtonPressed()
    {
        var currentObject = mChildCenter.centeredObject;
        var skin = int.Parse(currentObject.gameObject.name.Split('(')[0]);
        Debug.Log(skin);
        PlayerPrefs.SetInt("currentSkin", skin);
        Close();
        MenuController.Instance.Open();
    }

    private void AddSkinsToController()
    {
        for(int i=0;i< mSkinsPrefab.Length; i++)
        {
            mUIGrid.gameObject.transform.AddChild(mSkinsPrefab[i]);
            mUIGrid.enabled = true;
        }
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
