using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPurchase : MonoBehaviour
{
    public int mPrice;
    public int mIdSkin;

    public bool mIsBought;

    private UILabel mPriceLabel;

    private GameObject mAppleIcon;

    private void Start()
    {
        mAppleIcon = transform.Find("appleIcon").gameObject;
        mPriceLabel = transform.Find("priceLabel").GetComponent<UILabel>();
        mPriceLabel.text = mPrice.ToString();

        List<string> skins = new List<string>(Storage.Instance.mSkins.Split('|'));

        if (skins.Contains(mIdSkin.ToString()))
        {
            mIsBought = true;
            SetAsBought();
            if (PlayerPrefs.GetInt("currentSkin") == mIdSkin)
            {
                SetAsChoosen();
            }
        }
        
    }

    private void Purchase()
    {
        if (mPrice <= Storage.Instance.mAppleCount && !mIsBought)
        {
            Storage.Instance.mAppleCount -= mPrice;
            mIsBought = true;
            SetAsBought();
            SkinsController.Instance.UpdateControllerUI();
        }else if (mIsBought)
        {
            SetAsChoosen();
        }
        SkinsController.Instance.ChooseSkin(mIdSkin);
    }

    private void SetAsChoosen()
    {
        mAppleIcon.SetActive(false);
        mPriceLabel.text = "Choosen";
        mPriceLabel.alignment = NGUIText.Alignment.Center;
        mPriceLabel.transform.localPosition = Vector3.zero;
        mPriceLabel.width = 280;
    }

    private void SetAsBought()
    {
        mAppleIcon.SetActive(false);
        mPriceLabel.text = "Choose";
        mPriceLabel.alignment = NGUIText.Alignment.Center;
        mPriceLabel.transform.localPosition = Vector3.zero;
        mPriceLabel.width = 280;
    }

    public void OnClick()
    {
        Purchase();
    }
}
