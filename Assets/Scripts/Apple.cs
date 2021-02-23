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
            
            if (!mIsPlayedPartciles)
            {
                mSpriteComponent.enabled = false;
                mParticles.Play();
                mIsPlayedPartciles = true;
                Storage.Instance.mAppleCount++;
                Storage.Instance.SaveInfo(false, false);
                GameController.Instance.UpdateControllerUI();
                Destroy(gameObject, 1.5f);
            }
            
        }
    }
}
