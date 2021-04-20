using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Service
{
    public static string pathToTranslationFolder = @"c:\Program Files (x86)\HummeL\Bacterias Simulator\Translations";
    public static List<FileInfo> translations = new List<FileInfo>();
    public static int translationID = PlayerPrefs.GetInt("translationID");

    public static int TranslationID
    {
        get => translationID;
        set
        {
            Debug.Log("Translation changing pt.2");
            translationID = value;
            PlayerPrefs.SetInt("translationID", value);
            UpdateDictionary();
            UpdateAllTranslates();
        }
    }

    public static Dictionary<string, string> translationDictionary = new Dictionary<string, string>();

    public static GameObject[] UIs = Resources.LoadAll<GameObject>("Prefabs/UI");

    public static string GetTranslatableText(string textToTranslate)
    {
        return translationDictionary[textToTranslate];
    }

    public static float GaussianRandom(float min, float max)
    {
        float deltaNumber = (Mathf.Abs(min) + Mathf.Abs(max)) / 2f; // 20
        float averageNumber = (min + max) / 2f; // 10
        float dispersion = 1;
        float scale = 1 / (deltaNumber / (4 * dispersion)); // 0.2

        float chance = 0;
        float randomNumber = 0;
        while (Random.Range(0f, 1f) > chance || chance == 0)
        {
            randomNumber = Random.Range(min, max); // [-10, 30]
            chance = (1 / (dispersion * Mathf.Sqrt(2 * Mathf.PI))) * Mathf.Exp(-(Mathf.Pow((randomNumber - averageNumber) * scale, 2) / (2 * dispersion * dispersion)));
        }

        return randomNumber;
    }

    public static void Initialize()
    {
        CheckCreateDirectory(pathToTranslationFolder);
        setTranslationsList();
        UpdateDictionary();
        UpdateAllTranslates();
    }

    public static void UpdateAllTranslates()
    {
        foreach (TranslatedText toTranslate in Resources.FindObjectsOfTypeAll<TranslatedText>())
        {
            //if (Application.isEditor)
            //{
            //    if (!PrefabUtility.IsPartOfAnyPrefab(toTranslate))
            //    {
            //        toTranslate.text = GetTranslatableText(toTranslate.originalText);
            //    }
            //}
            //else
            {
                if (toTranslate.originalText != "")
                {
                    toTranslate.text = GetTranslatableText(toTranslate.originalText);
                }
            }
        }
    }

    public static void CheckCreateDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            CheckCopyBuiltinTranslations();
        }
    }

    public static void CheckCopyBuiltinTranslations()
    {
        DirectoryInfo d = new DirectoryInfo(Path.Combine(Application.streamingAssetsPath, "Built-in translations"));
        if(Directory.GetFiles(pathToTranslationFolder).Length == 0)
        {
            foreach (FileInfo fileInfo in d.GetFiles())
            {
                if (fileInfo.Extension != ".meta")
                {
                    fileInfo.CopyTo(Path.Combine(pathToTranslationFolder, fileInfo.Name));
                }
            }
        }
    }

    public static void setTranslationsList()
    {
        foreach (string filePath in Directory.GetFiles(pathToTranslationFolder))
        {
            FileInfo fileInfo = new FileInfo(filePath);
            translations.Add(fileInfo);
        }
    }

    public static void UpdateDictionary()
    {
        translationDictionary.Clear();
        using (StreamReader sr = translations[TranslationID].OpenText())
        {
            string line = "";
            string key = "";
            string value = "";
            
            while ((line = sr.ReadLine()) != null)
            {
                bool readingKey = true;
                foreach(char ch in line)
                {
                    if (ch == ':')
                    {
                        readingKey = false;
                    }
                    else
                    {
                        if (readingKey)
                        {
                            key += ch;
                        }
                        else
                        {
                            value += ch;
                        }
                    }
                }

                if (line != "")
                {
                    //Debug.Log("line: " + line + "\nkey: " + key + "\nvalue: " + value);
                    translationDictionary.Add(key, value);
                }

                key = "";
                value = "";
            }
        }
    }
}
