using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static BacteriaChooser;

public class ItemPreview : MonoBehaviour
{
    Image previewImage;
    SpriteRenderer spr;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateSpriteRenderers()
    {
        previewImage = GetComponent<Image>();
        spr = choosedItem.GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if (previewImage && spr)
        {
            previewImage.color = spr.color;
            transform.rotation = choosedItem.transform.rotation;
        }
    }
}
