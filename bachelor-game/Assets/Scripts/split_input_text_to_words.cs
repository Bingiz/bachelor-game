using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class split_input_text_to_words : MonoBehaviour
{
    public GameObject TextObject;

    public string input;

    public string output;

    public bool question;

    private char[] wordSplitPattern = {' ',',','.','?',':',';','!'};


    List<string> inputWords = new List<string>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("y"))
        {
            Debug.Log("enter");
            SendMessage();
        }
    }


    void SendMessage() 
    {
        input = TextObject.GetComponent<Text>().text;

        input = input.ToLower();

        string[] words = input.Split(wordSplitPattern);

        foreach(string x in words)
        {
            Debug.Log(x);
        }



    }

}
