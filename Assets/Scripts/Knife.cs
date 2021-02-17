using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public GameObject mParticleSystem;

    private void Start()
    {
        mParticleSystem = transform.Find("ParticleSystem").gameObject;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var rgd2d = gameObject.GetComponent<Rigidbody2D>();
        if (rgd2d != null)
        {
            if (rgd2d.bodyType == RigidbodyType2D.Static)
            {
                collision.gameObject.tag = "Respawn";
                StartCoroutine(LostGame_Coroutine(collision.gameObject));
            }
        }
    }

    IEnumerator LostGame_Coroutine(GameObject col)
    {
        col.GetComponent<Rigidbody2D>().AddTorque(0.5f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        LostController.Instance.SetGameInfo(GameController.Instance.mScore, GameController.Instance.mCurrentStage);
        GameController.Instance.CleanGameField();
        GameController.Instance.Close();
        
        LostController.Instance.Open();
    }
}
