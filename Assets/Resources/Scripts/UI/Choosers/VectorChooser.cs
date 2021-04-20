using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine;
using static Service;

public class VectorChooser : Chooser
{
    private bool activated = false;
    // Start is called before the first frame update
    void Start()
    {
        string temp = property.GetValue(component).ToString();
        string[] componentsOfVector = new string[4];

        int numberOfComponent = 0;
        for (int viewedChars = 0; viewedChars < temp.Length; viewedChars++)
        {
            if (temp[viewedChars] != ',' && temp[viewedChars] != '(' && temp[viewedChars] != ')')
            {
                if (temp[viewedChars] != ' ')
                {
                    if (temp[viewedChars] == '.')
                    {
                        componentsOfVector[numberOfComponent] += ',';
                    }
                    else
                    {
                        componentsOfVector[numberOfComponent] += temp[viewedChars];
                    }
                }
                else
                {
                    numberOfComponent++;
                }
            }
        }

        Vector3 vector = new Vector3(float.Parse(componentsOfVector[0]), float.Parse(componentsOfVector[1]), float.Parse(componentsOfVector[2]));

        SetText();

        transform.GetChild(1).GetComponent<InputField>().text = vector.x.ToString();
        transform.GetChild(2).GetComponent<InputField>().text = vector.y.ToString();
        transform.GetChild(3).GetComponent<InputField>().text = vector.z.ToString();
        activated = true;
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

    public void ChangeVector()
    {
        if (activated)
        {
            property.SetValue(component, new Vector3(float.Parse(transform.GetChild(1).GetComponent<InputField>().text), float.Parse(transform.GetChild(2).GetComponent<InputField>().text), float.Parse(transform.GetChild(3).GetComponent<InputField>().text)));
        }
    }
}