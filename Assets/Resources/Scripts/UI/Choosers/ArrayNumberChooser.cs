using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine;

public class ArrayNumberChooser : ArrayElementChooser
{
    public ArrayChooser linkedChooser;
    public int id;

    public InputField chooserInput;
    // Start is called before the first frame update
    void Awake()
    {
        chooserInput = transform.GetChild(0).GetComponent<InputField>();
    }

    public void SetText()
    {
        if (chooserInput)
        {
            chooserInput.text = linkedChooser.array[id].ToString();
        }
        else
        {
            chooserInput = transform.GetChild(0).GetComponent<InputField>();
            chooserInput.text = linkedChooser.array[id].ToString();
        }
    }

    public void ChangeArray()
    {
        linkedChooser.array[id] = int.Parse(chooserInput.text);
        linkedChooser.UpdateArray();
    }
}
