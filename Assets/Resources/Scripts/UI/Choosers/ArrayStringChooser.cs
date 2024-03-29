﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine;
using static FoodSpawner;
using static Service;

public class ArrayStringChooser : ArrayElementChooser
{
    public ArrayChooser linkedChooser;
    public int id;

    public Dropdown dropdown;
    // Start is called before the first frame update
    void Awake()
    {
        dropdown = transform.GetChild(0).GetComponent<Dropdown>();

        SetText();
    }

    void OnDisable()
    {
        SetText();
    }


    public void SetText()
    {

        TranslatedText dropdownText = transform.GetChild(0).GetChild(0).GetComponent<TranslatedText>();

        dropdown.options.Clear();
        foreach (Type food in foodsList)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = food.ToString() });
        }

        dropdownText.originalText = linkedChooser.list[id].ToString();
        dropdownText.text = GetTranslatableText(dropdownText.originalText);
        dropdown.value = dropdown.options.FindIndex(option => option.text == linkedChooser.list[id].ToString());
    }

    public void ChangeList()
    {
        linkedChooser.list[id] = Type.GetType(dropdown.options[dropdown.value].text);
        SetText();
        linkedChooser.UpdateList();
    }

    public void AddElement()
    {
        if (transform.parent.childCount < foodsList.Count)
        {
            linkedChooser.list.Add(foodsList[0]);
            GameObject arrayTypeChooser = Instantiate(UIs[3], transform.parent);

            ArrayTypeChooser chooser = arrayTypeChooser.GetComponent<ArrayTypeChooser>();

            chooser.linkedChooser = linkedChooser;
            chooser.id = linkedChooser.list.Count - 1;
            chooser.SetText();
        }
    }
}
