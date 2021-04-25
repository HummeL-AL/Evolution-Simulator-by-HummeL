using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WorldSettings;
using static GlobalSettings;

public class EnergyMapGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public static float xOffset;
    public static float yOffset;
    public Texture2D publicNoiseTexture;
    public static Texture2D noiseTexture;
    public GameObject visibleLight;

    private void Awake()
    {
        noiseTexture = publicNoiseTexture;
    }
    void Start()
    {
        noiseTexture.Resize((int)hBorder * 2, (int)vBorder * 2);
        visibleLight.transform.localScale = new Vector3(hBorder * 0.9f, vBorder * 0.9f, 1f);
        GenerateMap();
    }

    public void UpdateOffset()
    {
        xOffset = Time.time;
        yOffset = Time.time;
        GenerateMap();
    }

    public static void GenerateMap()
    {
        int curX, curY;
        Color curColor;
        for (curY = 0; curY < noiseTexture.height; curY++)
        {
            for (curX = 0; curX < noiseTexture.width; curX++)
            {
                float sample = Mathf.PerlinNoise((curX + xOffset) / scale, (curY + yOffset) / scale);
                curColor = new Color(sample, sample, sample, sample);
                noiseTexture.SetPixel(curX, curY, curColor);
            }
        }
        noiseTexture.Apply();
    }
}
