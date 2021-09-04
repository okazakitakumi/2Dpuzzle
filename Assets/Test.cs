using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        touchmanager.Began += onTouchBegan;
        touchmanager.Ended += (info) =>
        {
            Debug.Log("ボタンが離されたとき" + info.screenPoint);
        };
    }

    void onTouchBegan(TouchInfo info)
    {
        Debug.Log("ボタンが押されたとき" + info.screenPoint);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
