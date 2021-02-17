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
    private GameObject mwheelPrefab;
    private GameObject mCurrentWheel;

    private List<UISprite> mKnifesIconSprites;

    private Vector3 mKnifeToHitStartPosition;

    public int mCurrentStage = 1;
    public int mKnifesToHitLeft = 0;
    private int mKnifeLowerLimitForY = -650;
    public int mScore = 0;

    public bool isPlaying = false;
    private bool mKnifeNeedMove = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init()
    {
        mScore = 0;

        mPanel = GetComponent<UIPanel>();
        mPanel.alpha = 0;

        mKnifeToHitPrefab = Resources.Load<GameObject>("Prefabs/knifeHit");
        mwheelPrefab = Resources.Load<GameObject>("Prefabs/wheelSpriteNew");

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
        mScoreLabel.text = mScore.ToString();

        mKnifesToHitLeft = Storage.Instance.mGameSettings.mStagesKnifeCount[mCurrentStage - 1];
        mStagesProgress.transform.Find(mCurrentStage.ToString()+"s").GetComponent<UISprite>().color = new Color(255, 196, 0);
        for (int i = 0; i < Storage.Instance.mGameSettings.mStagesKnifeCount[mCurrentStage-1]; i++)
        {
            mKnifesIconSprites[i].alpha = 1;
        }

        mCurrentWheel = gameObject.transform.AddChild(mwheelPrefab);
        mCurrentWheel.transform.localPosition = new Vector3(0, 350, 0);
        StartCoroutine("SpawnWheel_Coroutine");
    }

    public void ResetKnifeIconsStates()
    {
        for (int i = 0; i < mKnifesIconSprites.Count; i++)
        {
            mKnifesIconSprites[i].color = new Color(1, 1, 1,0);
        }
    }

    public void CleanGameField()
    {
        mScore = 0;
        mCurrentStage = 1;
        Destroy(mCurrentWheel);
        Destroy(mKnifeToHit);
        ResetKnifeIconsStates();
        StartCoroutine("ResetStages_Couroutine");
    }

    IEnumerator ResetStages_Couroutine()
    {
        mStagesProgress.transform.Find("5s").GetComponent<UISprite>().color = new Color(1,1,1);
        //yield return new WaitForSeconds(0.1f);
        mStagesProgress.transform.Find("4s").GetComponent<UISprite>().color = new Color(1, 1, 1);
        //yield return new WaitForSeconds(0.1f);
        mStagesProgress.transform.Find("3s").GetComponent<UISprite>().color = new Color(1, 1, 1);
        //yield return new WaitForSeconds(0.1f);
        mStagesProgress.transform.Find("2s").GetComponent<UISprite>().color = new Color(1, 1, 1);
        //yield return new WaitForSeconds(0.1f);
        mStagesProgress.transform.Find("1s").GetComponent<UISprite>().color = new Color(1, 1, 1);
        yield return new WaitForSeconds(0.001f);
    }

    public void CreateNewKnifeToHit(bool shouldIncreaseScore = true)
    {
        if (shouldIncreaseScore)
            mScoreLabel.text = (++mScore).ToString();
        mKnifeToHit = transform.AddChild(mKnifeToHitPrefab);
        mKnifeToHit.transform.localPosition = mKnifeToHitStartPosition;
        //UpdateKnifeIconsStates();
    }

    IEnumerator BreakWheel_Couroutine()
    {
        wheelRotation wheel = mCurrentWheel.GetComponent<wheelRotation>();
        mCurrentWheel.GetComponent<CircleCollider2D>().enabled = false;
        wheel.enabled = false;
        for (int i = 0; i < mCurrentWheel.transform.childCount; i++)
        {
            var currChild = mCurrentWheel.transform.GetChild(i);
            currChild.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            currChild.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-2.5f,2.5f), Random.Range(1f, 2f)) * 40);
            currChild.GetComponent<Rigidbody2D>().AddTorque(10, ForceMode2D.Impulse);
        }
        
        yield return new WaitForSeconds(1f);
        Destroy(mCurrentWheel);
        CreateNewKnifeToHit();
        BuildKnifeIconsToHit();

    }

    public void UpdateKnifeIconsStates()
    {
        if (mKnifesToHitLeft <= 1)
        {
            if(mCurrentStage == 5)
            {
                mCurrentStage = 0;
                StartCoroutine("ResetStages_Couroutine");
            }
            mCurrentStage++;
            ResetKnifeIconsStates();
            StartCoroutine("BreakWheel_Couroutine");
            //BuildKnifeIconsToHit();
            return;
        }
        CreateNewKnifeToHit();
        mKnifesIconSprites[mKnifesToHitLeft - 1].color = new Color(0.5f, 0.5f, 0.5f, 1);
        mKnifesToHitLeft--;
    }

    IEnumerator SpawnWheel_Coroutine()
    {
        float scale = 0;
        for (int i = 0; i < 10; i++)
        {
            scale += 0.1f;
            mCurrentWheel.transform.localScale = new Vector3(scale,scale,scale);
            yield return new WaitForSeconds(0.001f);
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
        isPlaying = true;
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
        if (!mKnifeNeedMove && mPanel.alpha == 1 && isPlaying)
        {
            mKnifeToHit.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            mKnifeToHit.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1) * 400);
        }
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
