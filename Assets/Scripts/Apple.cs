using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    private UISprite mSpriteComponent;
    private ParticleSystem mParticles;
    // Start is called before the first frame update
    void Start()
    {
        mSpriteComponent = GetComponent<UISprite>();
        mParticles = transform.Find("ParticleSystem").GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(GameController.Instance.mCurrentWheel.transform.localScale.x);
        if (GameController.Instance.mCurrentWheel.transform.localScale.x >= 1)
        {
            Debug.Log("apple triggered with " + collision.gameObject.tag);
            mSpriteComponent.enabled = false;
            mParticles.Play();
            Destroy(gameObject, 1.5f);
        }
        /*if (collision.gameObject.tag == "Guest")
        {
            mSpriteComponent.enabled = false;
            mParticles.Play();
            Destroy(gameObject, 1.5f);
        }*/
    }
}
