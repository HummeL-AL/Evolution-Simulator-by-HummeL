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
    void Awake()
    {
        chooserRect = transform.GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        SetText();

        //Debug.Log("Property: " + property + " Value: " + property.GetValue(component) + " Type: " + property.PropertyType.Name);
        //Debug.Log(property.GetValue(component));


    }

    public void SetText()
    {
        if (property != null)
        {
            transform.GetChild(0).GetComponent<TranslatedText>().originalText = property.Name;
            transform.GetChild(0).GetComponent<TranslatedText>().text = GetTranslatableText(property.Name);
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

                            if (chooserRect)
                            {
                                chooserRect.sizeDelta += new Vector2(0f, arrayNumberChooser.gameObject.transform.GetComponent<RectTransform>().sizeDelta.y);
                            }
                            else
                            {
                                chooserRect = transform.GetComponent<RectTransform>();
                                chooserRect.sizeDelta += new Vector2(0f, arrayNumberChooser.gameObject.transform.GetComponent<RectTransform>().sizeDelta.y);
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

                            if (chooserRect)
                            {
                                chooserRect.sizeDelta += new Vector2(0f, arrayTypeChooser.gameObject.transform.GetComponent<RectTransform>().sizeDelta.y);
                            }
                            else
                            {
                                chooserRect = transform.GetComponent<RectTransform>();
                                chooserRect.sizeDelta += new Vector2(0f, arrayTypeChooser.gameObject.transform.GetComponent<RectTransform>().sizeDelta.y);
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

                            if (chooserRect)
                            {
                                chooserRect.sizeDelta += new Vector2(0f, arrayChooser.gameObject.transform.GetComponent<RectTransform>().sizeDelta.y);
                            }
                            else
                            {
                                chooserRect = transform.GetComponent<RectTransform>();
                                chooserRect.sizeDelta += new Vector2(0f, arrayChooser.gameObject.transform.GetComponent<RectTransform>().sizeDelta.y);
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

                if (chooserRect)
                {
                    chooserRect.sizeDelta += new Vector2(0f, arrayNumberChooser.gameObject.transform.GetComponent<RectTransform>().sizeDelta.y);
                }
                else
                {
                    chooserRect = transform.GetComponent<RectTransform>();
                    chooserRect.sizeDelta += new Vector2(0f, arrayNumberChooser.gameObject.transform.GetComponent<RectTransform>().sizeDelta.y);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
