using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelPart : MonoBehaviour
{

    private Rigidbody2D mRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        mRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    /*void Update()
    {
        mRigidbody.AddTorque(30, ForceMode2D.Impulse);
    }*/
}
