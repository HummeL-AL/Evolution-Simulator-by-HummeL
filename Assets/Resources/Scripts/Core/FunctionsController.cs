using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using static Service;

public class FunctionsController : MonoBehaviour
{
    private void Awake()
    {

    }
    // Start is called before the first frame update
    public static Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public static void GetVariables(GameObject objectToInspect, GameObject configPanel)
    {
        Grid grid = configPanel.GetComponent<Grid>();
        grid.enabled = false;
        Component[] objectComponents = objectToInspect.GetComponents(typeof(Component));
        foreach (var component in objectComponents)
        {
            foreach (var property in component.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.IsDefined(typeof(SerializeField), true))
                {
                    try
                    {
                        switch (property.PropertyType.Name)
                        {
                            case "Single":
                                {
                                    GameObject numberChooser = Instantiate(UIs[6], configPanel.transform);
                                    numberChooser.GetComponent<Chooser>().property = property;
                                    numberChooser.GetComponent<Chooser>().component = component;
                                    break;
                                }
                            case "Int32":
                                {
                                    GameObject numberChooser = Instantiate(UIs[6], configPanel.transform);
                                    numberChooser.GetComponent<Chooser>().property = property;
                                    numberChooser.GetComponent<Chooser>().component = component;
                                    break;
                                }
                            case "Int32[]":
                                {
                                    GameObject arrayChooser = Instantiate(UIs[0], configPanel.transform);
                                    arrayChooser.GetComponent<Chooser>().property = property;
                                    arrayChooser.GetComponent<Chooser>().component = component;

                                    arrayChooser.GetComponent<ArrayChooser>().SetChoosers();
                                    break;
                                }
                            case "IDictionary`2":
                                {
                                    GameObject arrayChooser = Instantiate(UIs[0], configPanel.transform);
                                    arrayChooser.GetComponent<Chooser>().property = property;
                                    arrayChooser.GetComponent<Chooser>().component = component;

                                    arrayChooser.GetComponent<ArrayChooser>().SetChoosers();
                                    break;
                                }
                            case "List`1":
                                {
                                    GameObject arrayChooser = Instantiate(UIs[0], configPanel.transform);
                                    arrayChooser.GetComponent<Chooser>().property = property;
                                    arrayChooser.GetComponent<Chooser>().component = component;

                                    arrayChooser.GetComponent<ArrayChooser>().SetChoosers();
                                    break;
                                }
                            case "Vector3":
                                {
                                    GameObject vectorChooser = Instantiate(UIs[8], configPanel.transform);
                                    vectorChooser.GetComponent<Chooser>().property = property;
                                    vectorChooser.GetComponent<Chooser>().component = component;
                                    break;
                                }
                            case "Boolean":
                                {
                                    GameObject booleanChooser = Instantiate(UIs[4], configPanel.transform);
                                    booleanChooser.GetComponent<BoolChooser>().property = property;
                                    booleanChooser.GetComponent<BoolChooser>().component = component;
                                    break;
                                }
                            case "Sprite":
                                {
                                    //GameObject spriteChooser = Instantiate(UIs[5], configPanel.transform);
                                    //spriteChooser.GetComponent<Chooser>().property = property;
                                    //spriteChooser.GetComponent<Chooser>().component = component;
                                    break;
                                }
                            case "Color":
                                {
                                    GameObject colorChooser = Instantiate(UIs[5], configPanel.transform);
                                    colorChooser.GetComponent<Chooser>().property = property;
                                    colorChooser.GetComponent<Chooser>().component = component;
                                    break;
                                }
                            default:
                                {
                                    Debug.Log("No one: " + property.PropertyType.Name);
                                    break;
                                }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
        }
        grid.enabled = true;
        grid.OnTransformChildrenChanged();
    }

    public static void GetVariablesToPanel(GameObject objectToInspect, GameObject configPanel, GameObject previewPanel)
    {
        Component[] objectComponents = objectToInspect.GetComponents(typeof(Component));
        foreach (var component in objectComponents)
        {
            foreach (var property in component.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.IsDefined(typeof(SerializeField), true))
                {
                    try
                    {
                        switch (property.PropertyType.Name)
                        {
                            case "Sprite":
                                {
                                    previewPanel.GetComponent<Image>().sprite = (Sprite)property.GetValue(component);
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
        }

        GetVariables(objectToInspect, configPanel);
    }
}
