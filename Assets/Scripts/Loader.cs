using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vibration.Init();
        Storage.Instance.Init();
        SkinsController.Instance.Init();
        LostController.Instance.Init();
        MenuController.Instance.Init();
        GameController.Instance.Init();

        MenuController.Instance.Open();
    }

}
