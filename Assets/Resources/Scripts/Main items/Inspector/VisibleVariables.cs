using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleVariables : MonoBehaviour
{
    [SerializeField]
    public Vector3 Position
    {
        get => gameObject.transform.position;
        set
        {
            gameObject.transform.position = value;
        }
    }

    [SerializeField]
    public Sprite Image
    {
        get => gameObject.GetComponent<SpriteRenderer>().sprite;
        set
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = value;
        }
    }

    [SerializeField]
    public Color Coloration
    {
        get => gameObject.GetComponent<SpriteRenderer>().color;
        set
        {
            gameObject.GetComponent<SpriteRenderer>().color = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
