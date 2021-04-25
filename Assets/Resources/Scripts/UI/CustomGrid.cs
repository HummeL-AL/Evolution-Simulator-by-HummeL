using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrid : MonoBehaviour
{
    public bool vertical = true;
    public float cellWidth;
    public float upperOffset, downOffset;
    public float spacing;
    public float panelHeight = 0;

    float placeHeight = 0;
    int childCount = 0;

    void Start()
    {

    }

    public void PlaceElements()
    {
        float tempPanelHeight = 0;
        float currentHeight = 0, previousHeight = 0;

        foreach (Transform child in transform)
        {
            if (child.GetComponent<CustomGrid>())
            {
                child.GetComponent<CustomGrid>().PlaceElements();
            }
            Debug.Log("Panel height: " + tempPanelHeight + " Child height: " + child.GetComponent<RectTransform>().sizeDelta.y);
            tempPanelHeight += child.GetComponent<RectTransform>().sizeDelta.y + upperOffset + downOffset;
            Debug.Log("Second panel height: " + tempPanelHeight);
        }

        panelHeight = tempPanelHeight;
        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, panelHeight);

        foreach (Transform child in transform)
        {
            Vector2 childDelta = child.GetComponent<RectTransform>().sizeDelta;
            if (childCount > 0)
            {
                Transform previousChild = transform.GetChild(childCount - 1);
                previousHeight = previousChild.GetComponent<RectTransform>().sizeDelta.y + spacing * 2f;
            }
            currentHeight = childDelta.y;

            placeHeight -= (currentHeight / 2f) + (previousHeight / 2f);
            child.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, placeHeight + panelHeight / 2f - upperOffset, 0f);

            childCount++;
        }
        placeHeight = 0;
        childCount = 0;
    }
}
