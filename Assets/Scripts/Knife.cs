using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var rgd2d = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rgd2d != null)
        {
            if (rgd2d.bodyType == RigidbodyType2D.Dynamic)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddTorque(0.5f, ForceMode2D.Impulse);
                //death logic -> show restart screen
            }
        }
    }
}
