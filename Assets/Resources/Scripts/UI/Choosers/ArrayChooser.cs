using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine;
using static Service;

public class ArrayChooser : Chooser
{
    public int[] array;
    public List<Type> list = new List<Type>();
    public Dictionary<int, int[]> dictionary;

    public ArrayChooser linkedChooser;
    public int id;

    public RectTransform chooserRect;
    public CustomGrid chooserGrid;
    void Awake()
    {
        chooserRect = GetComponent<RectTransform>();
        chooserGrid = GetComponent<CustomGrid>();
    }
    // Start is called before the first frame update
    void Start()
    {
        SetText();
        
        chooserGrid.enabled = true;
    }

    public void SetText()
    {
        if (property != null)
        {
            transform.GetChild(0).GetComponent<TranslatedText>().originalText = property.Name;
            transform.GetChild(0).GetComponent<TranslatedText>().text = GetTranslatableText(property.Name);
        }
        else
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }

    public virtual void SetChoosers()
    {
        if (property != null)
        {
            switch (property.PropertyType.Name)
            {
                case "Int32[]":
                    {
                        array = (int[])property.GetValue(component);
                        for (int number = 0; number < array.Length; number++)
                        {
                            GameObject arrayNumberChooser = Instantiate(UIs[2], transform.GetChild(1));
                            ArrayNumberChooser chooser = arrayNumberChooser.GetComponent<ArrayNumberChooser>();
                            chooser.linkedChooser = this;
                            chooser.id = number;
                            chooser.SetText();

                            if (!chooserRect)
                            {
                                chooserRect = transform.GetComponent<RectTransform>();
                            }
                        }
                        break;
                    }
                case "List`1":
                    {
                        var testList = property.GetValue(component) as IList;

                        foreach (var food in testList)
                        {
                            list.Add(food as Type);
                        }

                        for (int number = 0; number < list.Count; number++)
                        {
                            GameObject arrayTypeChooser = Instantiate(UIs[3], transform.GetChild(1));
                            ArrayTypeChooser chooser = arrayTypeChooser.GetComponent<ArrayTypeChooser>();
                            chooser.linkedChooser = this;
                            chooser.id = number;
                            chooser.SetText();

                            if (!chooserRect)
                            {
                                chooserRect = transform.GetComponent<RectTransform>();
                            }
                        }
                        break;
                    }
                case "IDictionary`2":
                    {
                        dictionary = (Dictionary<int, int[]>)property.GetValue(component);

                        for (int number = 0; number < dictionary.Count; number++)
                        {
                            GameObject arrayChooser = Instantiate(UIs[0], transform.GetChild(1));
                            ArrayChooser chooser = arrayChooser.GetComponent<ArrayChooser>();
                            chooser.id = number;
                            chooser.array = dictionary[number];
                            chooser.linkedChooser = this;
                            chooser.SetChoosers();
                            chooser.SetText();

                            if (!chooserRect)
                            {
                                chooserRect = transform.GetComponent<RectTransform>();
                            }
                        }
                        break;
                    }
                default:
                    {
                        Debug.Log("No one: " + property.PropertyType.Name);
                        break;
                    }
            }
        }
        else
        {
            for (int number = 0; number < array.Length; number++)
            {
                GameObject arrayNumberChooser = Instantiate(UIs[2], transform.GetChild(1));
                ArrayNumberChooser chooser = arrayNumberChooser.GetComponent<ArrayNumberChooser>();
                chooser.linkedChooser = this;
                chooser.id = number;
                chooser.SetText();

                if (!chooserRect)
                {
                    chooserRect = transform.GetComponent<RectTransform>();
                }
            }
        }
    }

    public void UpdateArray()
    {
        if (property != null)
        {
            property.SetValue(component, array);
        }
        else
        {
            ChangeDictionary();
        }
    }

    public void UpdateList()
    {
        if (property != null)
        {
            property.SetValue(component, list);
        }
    }

    public void ChangeDictionary()
    {
        if (property == null)
        {
            linkedChooser.dictionary[id] = array;
            linkedChooser.UpdateDictionary();
        }
    }

    public void UpdateDictionary()
    {
        if (property != null)
        {
            property.SetValue(component, dictionary);
        }
    }
}
