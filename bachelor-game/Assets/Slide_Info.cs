using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slide_Info : MonoBehaviour
{
    public Animator anim;
    public Text text;

    public float stayTime;
    float cooldown;
    bool visible = false;


    private void Update()
    {
        if(cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        else if (visible && cooldown <=0)
        {
            SlideOut();
            visible = false;
        }

    }

    public void ShowMessage(string inputString)
    {
        text.text = inputString;
        anim.SetTrigger("Slide_In");
        cooldown = stayTime;
        visible = true;
    }

    void SlideOut()
    {
        anim.SetTrigger("Slide_Out");
    }


}
