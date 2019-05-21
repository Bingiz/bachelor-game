using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;


public class StemDictionary : MonoBehaviour
{
    //StreamReader stemVocabulary = new StreamReader(Application.dataPath + "../Librarys/stemVocabulary.txt");

    public TextAsset stemVocabularyText;
    public Dictionary<string,string> stemDictionary = new Dictionary<string,string>();

    string splitDictionaryKey = "^[a-z,A-Z,/']+";
    string splitDictionaryValue = "[a-z,A-Z,/']+$";

    // This code writes the Values from the stemVocabulary.txt into the stemDictionary (Word is key and stemmed version is value)
    void Start()
    {
        string stemVocabulary = stemVocabularyText.text;

        string[] line = stemVocabulary.Split('\n');


        for (int i = 0; i < line.Length; i++)
        {
            stemDictionary.Add(Regex.Match(line[i], splitDictionaryKey).ToString(), Regex.Match(line[i], splitDictionaryValue).ToString());
        }

    }
}
