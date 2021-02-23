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

    private UILabel mAppleLabel;

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

        mAppleLabel = transform.Find("appleLabel").GetComponent<UILabel>();

        mPanel = GetComponent<UIPanel>();

        AddSkinsToController();
    }

    public void UpdateControllerUI()
    {
        mAppleLabel.text = Storage.Instance.mAppleCount.ToString();
    }

    public void OnChooseButtonPressed()
    {
        Close();
        MenuController.Instance.Open();
    }

    public void ChooseSkin(int idSkin)
    {
        var skin = idSkin;
        var currentSkin = PlayerPrefs.GetInt("currentSkin");
        if (skin != currentSkin)
            UpdateSkins();
        PlayerPrefs.SetInt("currentSkin", skin);
        Storage.Instance.SaveInfo(false, true);
        //Close();
        //MenuController.Instance.Open();
    }

    public void UpdateSkins()
    {
        var currentSkin = PlayerPrefs.GetInt("currentSkin");
        var currentSkinInScene = mUIGrid.transform.Find(currentSkin + "(Clone)").gameObject;
        currentSkinInScene.transform.Find("purchaseButton").transform.Find("priceLabel").GetComponent<UILabel>().text = "Choose";
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
        UpdateControllerUI();
        
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
