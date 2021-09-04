using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimeManager : MonoBehaviour
{
    public GameObject timeobj = null;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Text timetext = timeobj.GetComponent<Text>();
        timetext.text = "残り時間（秒）" + (60.0f - Time.time);
        
    }
}
