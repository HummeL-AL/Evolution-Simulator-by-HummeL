using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using static Service;

public class ColorChooser : Chooser
{
    private Color coloring;
    private bool activated = false;
    private Slider rSlider, gSlider, bSlider, aSlider;

    void Start()
    {
        SetText();
        coloring = (Color)property.GetValue(component);

        rSlider = transform.GetChild(2).GetComponent<Slider>();
        rSlider.value = coloring.r;
        gSlider = transform.GetChild(3).GetComponent<Slider>();
        gSlider.value = coloring.g;
        bSlider = transform.GetChild(4).GetComponent<Slider>();
        bSlider.value = coloring.b;
        aSlider = transform.GetChild(5).GetComponent<Slider>();
        aSlider.value = coloring.a;

        activated = true;
        ChangeColor();
    }

    public void SetText()
    {
        transform.GetChild(0).GetComponent<TranslatedText>().originalText = property.Name;
        transform.GetChild(0).GetComponent<TranslatedText>().text = GetTranslatableText(property.Name);
    }

    public void ChangeColor()
    {
        if (activated)
        {
            ColorBlock cb = rSlider.colors;
            cb.normalColor = new Color(rSlider.value, 0f, 0f);
            rSlider.colors = cb;

            cb = gSlider.colors;
            cb.normalColor = new Color(0f, gSlider.value, 0f);
            gSlider.colors = cb;

            cb = bSlider.colors;
            cb.normalColor = new Color(0f, 0f, bSlider.value);
            bSlider.colors = cb;

            cb = aSlider.colors;
            cb.normalColor = new Color(0f, 0f, 0f, aSlider.value);
            aSlider.colors = cb;

            coloring = new Color(rSlider.value, gSlider.value, bSlider.value, aSlider.value);

            transform.GetChild(1).GetComponent<Image>().color = coloring;
            property.SetValue(component, coloring);
        }
    }
}
