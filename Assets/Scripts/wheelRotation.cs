using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheelRotation : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, new Vector3(0,0,1f), 100 * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.name);
        col.gameObject.transform.parent = transform;
        Vector3 relative = col.gameObject.transform.InverseTransformPoint(transform.position);
        col.gameObject.transform.localPosition = relative;
        Debug.Log(col.gameObject.transform.position);
        Debug.Log(relative);
    }

}
