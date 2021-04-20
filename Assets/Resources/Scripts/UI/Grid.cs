using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public bool vertical = true;
    public float cellWidth;
    public float panelHeight;

    float placeHeight = 0;
    int childCount = 0;

    private void Awake()
    {
        panelHeight = transform.gameObject.GetComponent<RectTransform>().sizeDelta.y;
    }

    // Start is called before the first frame update
    void Start()
    {
        OnTransformChildrenChanged();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTransformChildrenChanged()
    {
        if (enabled)
        {
            float currentHeight = 0, previousHeight = 0;

            foreach (Transform child in transform)
            {
                Vector2 childDelta = child.GetComponent<RectTransform>().sizeDelta;
                if (childCount > 0)
                {
                    Transform previousChild = transform.GetChild(childCount - 1);
                    previousHeight = previousChild.GetComponent<RectTransform>().sizeDelta.y;
                }
                currentHeight = childDelta.y;

                placeHeight -= (currentHeight / 2f) + (previousHeight / 2f);
                child.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, placeHeight + panelHeight / 2f, 0f);

                childCount++;
            }
            placeHeight = 0;
            childCount = 0;
        }
    }
}
