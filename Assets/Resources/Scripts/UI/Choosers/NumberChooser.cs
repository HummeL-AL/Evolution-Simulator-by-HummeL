using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine;
using static Service;

public class NumberChooser : Chooser
{   // Start is called before the first frame update
    
    void Start()
    {
        SetText();
        transform.GetChild(1).GetComponent<InputField>().text = property.GetValue(component, null).ToString();
    }

    public void SetText()
    {
        transform.GetChild(0).GetComponent<TranslatedText>().originalText = property.Name;
        transform.GetChild(0).GetComponent<TranslatedText>().text = GetTranslatableText(property.Name);
    }

    public void ChangeNumber()
    {
        if (property.PropertyType.Name == "Int32" || property.PropertyType.Name == "Int32[]")
        {
            property.SetValue(component, int.Parse(transform.GetChild(1).GetComponent<InputField>().text));
        }
        else
        {
            property.SetValue(component, float.Parse(transform.GetChild(1).GetComponent<InputField>().text));
        }
    }
}
