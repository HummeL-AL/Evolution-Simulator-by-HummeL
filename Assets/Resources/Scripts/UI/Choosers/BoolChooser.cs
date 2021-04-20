using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine;
using static Service;

public class BoolChooser : Chooser
{
    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        SetText();
        transform.GetChild(1).GetComponent<Toggle>().isOn = (bool)property.GetValue(component, null);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetText()
    {
        transform.GetChild(0).GetComponent<TranslatedText>().originalText = property.Name;
        transform.GetChild(0).GetComponent<TranslatedText>().text = GetTranslatableText(property.Name);
    }

    public void ChangeBool()
    {
        property.SetValue(component, transform.GetChild(1).GetComponent<Toggle>().isOn);
    }
}