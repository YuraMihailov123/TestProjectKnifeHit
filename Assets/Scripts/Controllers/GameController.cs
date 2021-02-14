using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    #region Singleton
    private static GameController _instance;

    public static GameController Instance
    {
        get
        {
            try
            {
                if (_instance == null) _instance = GameObject.Find("Root").transform.Find("GameController").GetComponent<GameController>();
            }
            catch { }
            return _instance;
        }
    }
    #endregion

    private UIPanel mPanel;

    private UILabel mScoreLabel;
    private UILabel mStageLabel;

    private GameObject mKnifeSet;
    private GameObject mStagesProgress;
    private GameObject mKnifeToHitPrefab;
    private GameObject mKnifeToHit;

    private List<UISprite> mKnifesIconSprites;

    private Vector3 mKnifeToHitStartPosition;

    public int mCurrentStage = 1;
    public int mKnifesToHitLeft = 0;
    private int mKnifeLowerLimitForY = -650;
    private int score = 0;

    private bool mKnifeNeedMove = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init()
    {
        score = 0;

        mPanel = GetComponent<UIPanel>();
        mPanel.alpha = 0;

        mKnifeToHitPrefab = Resources.Load<GameObject>("Prefabs/knifeHit");

        mKnifeToHit = transform.Find("knifeHit").gameObject; // -650
        mScoreLabel = transform.Find("GameControllerUI").transform.Find("scoreLabel").GetComponent<UILabel>();
        mStageLabel = transform.Find("GameControllerUI").transform.Find("stageLabel").GetComponent<UILabel>();
        mKnifeSet = transform.Find("GameControllerUI").transform.Find("knifeSet").gameObject;

        mKnifesIconSprites = new List<UISprite>();
        for (int i = 0; i < mKnifeSet.transform.childCount; i++) {
            mKnifesIconSprites.Add(mKnifeSet.transform.GetChild(i).GetComponent<UISprite>());
            mKnifesIconSprites[i].alpha = 0;
        }

        mStagesProgress = transform.Find("GameControllerUI").transform.Find("stagesProgress").gameObject;
    }

    private void StartGame()
    {
        mKnifeNeedMove = true;
        PrepareGameControllerUI();
    }

    private void PrepareGameControllerUI()
    {
        BuildKnifeIconsToHit();
    }

    private void BuildKnifeIconsToHit()
    {
        mStageLabel.text = "STAGE " + mCurrentStage;
        mScoreLabel.text = score.ToString();

        mKnifesToHitLeft = Storage.Instance.mGameSettings.mStagesKnifeCount[mCurrentStage - 1];
        mStagesProgress.transform.Find(mCurrentStage.ToString()+"s").GetComponent<UISprite>().color = new Color(255, 196, 0);
        for (int i = 0; i < Storage.Instance.mGameSettings.mStagesKnifeCount[mCurrentStage-1]; i++)
        {
            mKnifesIconSprites[i].alpha = 1;
        }
    }

    public void ResetKnifeIconsStates()
    {
        for (int i = 0; i < mKnifesIconSprites.Count; i++)
        {
            mKnifesIconSprites[i].color = new Color(1, 1, 1,0);
        }
    }

    public void CreateNewKnifeToHit()
    {
        mScoreLabel.text = (++score).ToString();
        mKnifeToHit = transform.AddChild(mKnifeToHitPrefab);
        mKnifeToHit.transform.localPosition = mKnifeToHitStartPosition;
        UpdateKnifeIconsStates();
    }

    private void UpdateKnifeIconsStates()
    {
        if (mKnifesToHitLeft <= 1)
        {
            mCurrentStage++;
            ResetKnifeIconsStates();
            BuildKnifeIconsToHit();
            return;
        }
        mKnifesIconSprites[mKnifesToHitLeft - 1].color = new Color(0.5f, 0.5f, 0.5f, 1);
        mKnifesToHitLeft--;
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
        StartGame();
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

    public void TapToHit()
    {
        Debug.Log("Hit!");
        mKnifeToHit.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        mKnifeToHit.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1) * 300);
    }

    private void Update()
    {
        if (mKnifeNeedMove)
        {
            Vector3 newPos = new Vector3(mKnifeToHit.transform.localPosition.x, mKnifeLowerLimitForY, mKnifeToHit.transform.localPosition.z);
            mKnifeToHit.transform.localPosition = Vector3.Lerp(mKnifeToHit.transform.localPosition, newPos, Time.deltaTime * 10);
            if (Mathf.Abs(mKnifeToHit.transform.localPosition.y - mKnifeLowerLimitForY) < 1)
            {
                mKnifeNeedMove = false;
                mKnifeToHitStartPosition = mKnifeToHit.transform.localPosition;
            }
        }
        
    }
}
