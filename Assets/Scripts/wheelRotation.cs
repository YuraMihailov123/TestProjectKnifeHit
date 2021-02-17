using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheelRotation : MonoBehaviour
{

    private Quaternion rotation1 = Quaternion.Euler(0, 0, 0);
    private Quaternion rotation2 = Quaternion.Euler(0, 0, 360);
    private int mAngle = -360;
    private int mSpeed = 1;

    private void Start()
    {
        mSpeed = Storage.Instance.mGameSettings.mWheelSpeed[Random.Range(0, 3)];
    }

    // Update is called once per frame
    void Update()
    {
        mAngle += mSpeed;
        if (mAngle == 360)
        {
            mAngle = -360;
        }
        //transform.RotateAround(transform.position, new Vector3(0,0,1f), 100 * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, mAngle), 0.5f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.name);
        if (col.gameObject.tag == "Finish")
        {
            col.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            col.gameObject.GetComponent<Knife>().mParticleSystem.SetActive(true);
            //col.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            col.gameObject.transform.parent = transform;
            GameController.Instance.UpdateKnifeIconsStates();
        }
    }

}
