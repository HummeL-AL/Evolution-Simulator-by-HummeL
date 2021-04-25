using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FoodSpawner;

public class Meat : EdibleFood
{
    public override void Awake()
    {
        base.Awake();
        foodColor = new Color(1, 0, 0);
    }
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }
}
