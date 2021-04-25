using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BacteriaChooser;

public class TabSystem : MonoBehaviour
{
    public int buttonID;
    public static GameObject[] tabs = new GameObject[3];

    void Awake()
    {
        tabs[0] = transform.parent.GetChild(1).gameObject;
        tabs[1] = transform.parent.GetChild(2).gameObject;
        tabs[2] = transform.parent.GetChild(3).gameObject;

        foreach (GameObject tab in tabs)
        {
            tab.SetActive(false);
        }
    }

    public void ChangeTab()
    {
        foreach(GameObject tab in tabs)
        {
            if (Array.IndexOf(tabs, tab) != buttonID)
            {
                if(buttonID == 0)
                {
                    inspectionPanelActive = true;
                }
                else
                {
                    inspectionPanelActive = false;
                }

                tab.SetActive(false);
            }
            else
            {
                tab.SetActive(true);
            }
        }
    }
}
