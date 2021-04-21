using System.IO;
using System.Text;
using System.Linq;
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

    public static bool FilesEqual(string firstFilePath, string secondFilePath)
    {
        byte[] bytes1 = Encoding.Convert(Encoding.GetEncoding(1252), Encoding.ASCII, Encoding.GetEncoding(1252).GetBytes(File.ReadAllText(firstFilePath)));
        byte[] bytes2 = Encoding.Convert(Encoding.GetEncoding(1252), Encoding.ASCII, Encoding.GetEncoding(1252).GetBytes(File.ReadAllText(secondFilePath)));

        if (Encoding.ASCII.GetChars(bytes1).SequenceEqual(Encoding.ASCII.GetChars(bytes2)))
        {
            return true; 
        }
        else
        {
            return false;
        }
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
        }
        CheckCopyBuiltinTranslations();
    }

    public static void CheckCopyBuiltinTranslations()
    {
        string builtinTranslationsPath = Path.Combine(Application.streamingAssetsPath, "Built-in translations");
        DirectoryInfo b = new DirectoryInfo(pathToTranslationFolder);
        DirectoryInfo d = new DirectoryInfo(builtinTranslationsPath);

        foreach (FileInfo primaryFileInfo in d.GetFiles())
        {
            if(primaryFileInfo.Extension != ".meta")
            {
                bool exists = false;
                bool equal = false;
                FileInfo existingFile = null;

                foreach(FileInfo secondaryFileInfo in b.GetFiles())
                {
                    if(primaryFileInfo.Name == secondaryFileInfo.Name)
                    {
                        exists = true;
                        existingFile = secondaryFileInfo;
                        if (FilesEqual(Path.Combine(primaryFileInfo.DirectoryName, primaryFileInfo.Name), Path.Combine(secondaryFileInfo.DirectoryName, secondaryFileInfo.Name)))
                        {
                            equal = true;
                        }
                        break;
                    }
                }

                if (!exists)
                {
                    primaryFileInfo.CopyTo(Path.Combine(pathToTranslationFolder, primaryFileInfo.Name));
                    
                }
                else
                {
                    if(!equal)
                    {
                        File.Delete(Path.Combine(existingFile.DirectoryName, existingFile.Name));
                        primaryFileInfo.CopyTo(Path.Combine(pathToTranslationFolder, primaryFileInfo.Name));
                        
                    }
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
