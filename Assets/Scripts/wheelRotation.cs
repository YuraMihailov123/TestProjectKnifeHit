using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheelRotation : MonoBehaviour
{

    private Quaternion rotation1 = Quaternion.Euler(0, 0, 0);
    private Quaternion rotation2 = Quaternion.Euler(0, 0, 360);

    private int mAngle = -360;
    private int mSpeed = 1;
    private int mDistanceFromCenterToSpawnKnife = 0;

    private bool mIsObjectsSpawned = false;


    private void Start()
    {
        mSpeed = Storage.Instance.mGameSettings.mWheelSpeed[Random.Range(0, 3)];
        mDistanceFromCenterToSpawnKnife = (int)GetComponent<CircleCollider2D>().radius + 50;
        SpawnObjectsOnWheel();
    }

    private void SpawnObjectsOnWheel()
    {
        //---------knifes spawn
        int countKnifes = Random.Range(1, 3);
        int alpha = 0;
        List<int> prevAlphas = new List<int>();
        float xOffset = 0;
        float yOffset = 0;
        GameController.Instance.mKnifeToHitPrefab.tag = "Homie";
        for (int i = 0; i < countKnifes; i++)
        {
            alpha = Random.Range(-180,180);
            for(int j = 0; j < prevAlphas.Count; j++)
            {
                if (Mathf.Abs(alpha - prevAlphas[j]) < 20)
                {
                    alpha = Random.Range(-180, 180);
                    j = 0;
                }
            }
            prevAlphas.Add(alpha);
            xOffset = Mathf.Cos(alpha * Mathf.Deg2Rad) * mDistanceFromCenterToSpawnKnife;
            yOffset = Mathf.Sin(alpha * Mathf.Deg2Rad) * mDistanceFromCenterToSpawnKnife;
            var currentKnife = transform.AddChild(GameController.Instance.mKnifeToHitPrefab);
            currentKnife.GetComponent<BoxCollider2D>().isTrigger = true;
            currentKnife.transform.localPosition = new Vector3(xOffset, yOffset, 0);
            currentKnife.transform.localRotation = Quaternion.Euler(0, 0, 90+alpha);
        }
        //---------knifes spawn

        //---------apple spawn
        alpha = Random.Range(-180, 180);
        for (int j = 0; j < prevAlphas.Count; j++)
        {
            if (Mathf.Abs(alpha - prevAlphas[j]) < 40)
            {
                alpha = Random.Range(-180, 180);
                j = 0;
            }
        }
        xOffset = Mathf.Cos(alpha * Mathf.Deg2Rad) * mDistanceFromCenterToSpawnKnife;
        yOffset = Mathf.Sin(alpha * Mathf.Deg2Rad) * mDistanceFromCenterToSpawnKnife;
        var currentApple = transform.AddChild(GameController.Instance.mApplePrefab);
        currentApple.transform.localPosition = new Vector3(xOffset, yOffset, 0);
        currentApple.transform.localRotation = Quaternion.Euler(0, 0, 90 + alpha);
        //---------apple spawn
        mIsObjectsSpawned = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.isPlaying && mIsObjectsSpawned)
        {
            mAngle += mSpeed;
            if (mAngle == 360)
            {
                mAngle = -360;
            }
            //transform.RotateAround(transform.position, new Vector3(0,0,1f), 100 * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, mAngle), 0.5f);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("wheel triggered with " + col.gameObject.tag);
        if (col.gameObject.tag == "Guest")
        {
            col.gameObject.tag = "Homie";
            col.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            col.gameObject.GetComponent<Knife>().mParticleSystem.SetActive(true);
            col.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            col.gameObject.transform.parent = transform;
            GameController.Instance.UpdateKnifeIconsStates();
        }
    }

}
