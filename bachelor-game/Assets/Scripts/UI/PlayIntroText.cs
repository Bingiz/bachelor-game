using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayIntroText : MonoBehaviour
{
    Animator anim;
    public Text nextText;

    public float timerStart;
    private float timer;
    private bool wait = false;
    private bool fadeOut = false;

    private void Start()
    {
        timer = timerStart;
        anim = GetComponent<Animator>();
    }

    public void LoadNext()
    {
        if (nextText != null)
        {
            //load next animation
            anim.SetTrigger("next");
            nextText.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
            
        }
        else
        {
            //loadNextScene
            if (SceneManager.GetActiveScene().name == "Intro")
            {
                Debug.Log("Load Game");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else if (SceneManager.GetActiveScene().name == "Outro")
            {
                Debug.Log("Load Main Menu");
                SceneManager.LoadScene("MainMenu");
            }
            
            
        }
    }

    public void Wait()
    {
        wait = true;
    }

    private void Update()
    {
        if (wait == true)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                //loadNextText
                if (fadeOut)
                {
                    LoadNext();
                }
                else
                {
                    anim.SetTrigger("next");
                    fadeOut = true;
                }
                wait = false;
                timer = timerStart;
            }
        }

        if (Input.anyKeyDown)
        {
            //loadNextScene
            if (SceneManager.GetActiveScene().name == "Intro")
            {
                Debug.Log("Load Game");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else if (SceneManager.GetActiveScene().name == "Outro")
            {
                Debug.Log("Load Main Menu");
                SceneManager.LoadScene("MainMenu");
            }
        }
    }


}
