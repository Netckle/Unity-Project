using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OkOrCancel : MonoBehaviour
{
    private AudioManager the_audio;
    public string key_sound;
    public string enter_sound;
    public string cancel_sound;

    public GameObject up_Panel;
    public GameObject down_Panel;

    public Text up_Text;
    public Text down_Text;

    public bool activated;
    private bool key_input;
    private bool result = true;

    void Start()
    {
        the_audio = FindObjectOfType<AudioManager>();
    }

    public void Selected()
    {
        the_audio.Play(key_sound);
        result = !result;

        if (result)
        {
            up_Panel.gameObject.SetActive(false);
            down_Panel.gameObject.SetActive(true);
        }
        else
        {
            up_Panel.gameObject.SetActive(true);
            down_Panel.gameObject.SetActive(false);
        }
    }

    public void ShowTwoChoice(string _upText, string _downText)
    {
        activated = true;
        result = true;
        up_Text.text = _upText;
        down_Text.text = _downText;

        up_Panel.gameObject.SetActive(false);
        down_Panel.gameObject.SetActive(true);

        StartCoroutine(ShowTwoChoiceCoroutine());
    }

    public bool GetResult()
    {
        return result;
    }

    IEnumerator ShowTwoChoiceCoroutine()
    {
        yield return new WaitForSeconds(0.01f);
        key_input = true;
    }

    void Update()
    {
        if (key_input)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {                
                Selected();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Selected();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                the_audio.Play(enter_sound);
                key_input = false;
                activated = false;
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                the_audio.Play(cancel_sound);
                key_input = false;
                activated = false;
                result = false;
            }
        }
    }
}
