using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    private float choosedScale;

    public void ChangeTimeScale()
    {
        choosedScale = GetComponent<Slider>().value;
        Time.timeScale = choosedScale;
        //Time.fixedDeltaTime = choosedScale / 50; 
    }
}
