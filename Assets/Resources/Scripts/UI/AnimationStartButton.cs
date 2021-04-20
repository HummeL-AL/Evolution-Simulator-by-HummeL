using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BacteriaChooser;

public class AnimationStartButton : MonoBehaviour
{
    public GameObject objectToAnimate;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = objectToAnimate.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAnimation()
    {
        inspectionPanelActive = !anim.GetBool("Was Played");
        anim.SetBool("Was Played", !anim.GetBool("Was Played"));
    }
}
