using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FoodSpawner;

public class Mollusk : EdibleFood
{
    public override void Awake()
    {
        base.Awake();
        foodColor = new Color(0.84f, 0.74f, 0.61f);
    }
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
