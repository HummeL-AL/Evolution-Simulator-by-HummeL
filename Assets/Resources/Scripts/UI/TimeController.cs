using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    private float choosedScale;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeTimeScale()
    {
        choosedScale = GetComponent<Slider>().value;
        Time.timeScale = choosedScale;
        //Time.fixedDeltaTime = choosedScale / 50; 
    }
}
