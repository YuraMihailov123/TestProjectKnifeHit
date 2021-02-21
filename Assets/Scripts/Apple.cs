using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    private UISprite mSpriteComponent;

    private ParticleSystem mParticles;

    private bool mIsPlayedPartciles;
    // Start is called before the first frame update
    void Start()
    {
        mIsPlayedPartciles = false;
        mSpriteComponent = GetComponent<UISprite>();
        mParticles = transform.Find("ParticleSystem").GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameController.Instance.mCurrentWheel.transform.localScale.x >= 1 && !GameController.Instance.mIsWheelBreaking)
        {
            Debug.Log("apple triggered with " + collision.gameObject.tag);
            mSpriteComponent.enabled = false;
            if (!mIsPlayedPartciles)
            {
                mParticles.Play();
                mIsPlayedPartciles = false;
            }
            Storage.Instance.mAppleCount++;
            Storage.Instance.SaveInfo(false);
            GameController.Instance.UpdateControllerUI();
            Destroy(gameObject, 1.5f);
        }
    }
}
