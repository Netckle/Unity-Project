using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    static public DialogueManager instance;   

    public Text text;
    public SpriteRenderer renderer_sprite;
    public SpriteRenderer renderer_dialogue_window;

    private List<string> list_sentences;
    private List<Sprite> list_sprites;
    private List<Sprite> list_dialogue_windows;

    private int count; // 대화 진행 상황 카운트.

    public Animator anim_sprite;
    public Animator anim_dialogue_window;

    public string type_sound;
    public string enter_sound;

    private AudioManager the_audio;
    private OrderManager the_order;

    public bool is_talking = false;
    private bool key_activated = false;
    private bool only_text = false; // 대화가 아닌 단순 텍스트 표현 여부.

    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion Singleton

    void Start () 
    {
        count = 0;
        text.text = "";
        list_sentences = new List<string>();
        list_sprites = new List<Sprite>();
        list_dialogue_windows = new List<Sprite>();

        the_audio = FindObjectOfType<AudioManager>();
        the_order = FindObjectOfType<OrderManager>();
    }

    public void ShowText(string[] _sentences)
    {
        is_talking = true;
        only_text = true;

        for(int i = 0; i < _sentences.Length; i++)
        {
            list_sentences.Add(_sentences[i]);
        }

        StartCoroutine(StartTextCoroutine());
    }

    IEnumerator StartTextCoroutine()
    {        
        key_activated = true;

        for(int i = 0; i < list_sentences[count].Length; i++)
        {
            text.text += list_sentences[count][i]; // 1글자씩 출력.
            if(i % 7 == 1)
            {
                the_audio.Play(type_sound);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
	
    public void ShowDialogue(Dialogue dialogue)
    {
        is_talking = true;
        only_text = false;

        the_order.NotMove(); // 대화 중에 움직이는 것 방지.

        for(int i = 0; i < dialogue.sentences.Length; i++)
        {
            list_sentences.Add(dialogue.sentences[i]);
            list_sprites.Add(dialogue.sprites[i]);
            list_dialogue_windows.Add(dialogue.dialogueWindows[i]);
        }

        anim_sprite.SetBool("Appear", true);
        anim_dialogue_window.SetBool("Appear", true);

        StartCoroutine(StartDialogueCoroutine());
    }

    public void ExitDialogue()
    {
        text.text = "";
        count = 0;
        list_sentences.Clear();
        list_sprites.Clear();
        list_dialogue_windows.Clear();
        anim_sprite.SetBool("Appear", false);
        anim_dialogue_window.SetBool("Appear", false);
        is_talking = false;

        the_order.canMove();
    }

    IEnumerator StartDialogueCoroutine()
    {
        if(count > 0)
        {
            if (list_dialogue_windows[count] != list_dialogue_windows[count - 1])
            {
                anim_sprite.SetBool("Change", true);
                anim_dialogue_window.SetBool("Appear", false);

                yield return new WaitForSeconds(0.2f);

                renderer_dialogue_window.GetComponent<SpriteRenderer>().sprite = list_dialogue_windows[count];
                renderer_sprite.GetComponent<SpriteRenderer>().sprite = list_sprites[count];

                anim_dialogue_window.SetBool("Appear", true);
                anim_sprite.SetBool("Change", false);
            }
            else
            {
                if (list_sprites[count] != list_sprites[count - 1])
                {
                    anim_sprite.SetBool("Change", true);

                    yield return new WaitForSeconds(0.1f);

                    renderer_sprite.GetComponent<SpriteRenderer>().sprite = list_sprites[count];
                    anim_sprite.SetBool("Change", false);
                }
                else
                {
                    yield return new WaitForSeconds(0.05f);
                }
            }
            
        }
        else
        {
            yield return new WaitForSeconds(0.05f);

            renderer_dialogue_window.GetComponent<SpriteRenderer>().sprite = list_dialogue_windows[count];
            renderer_sprite.GetComponent<SpriteRenderer>().sprite = list_sprites[count];
        }
        key_activated = true;
        for(int i = 0; i < list_sentences[count].Length; i++)
        {
            text.text += list_sentences[count][i]; // 1글자씩 출력.
            if(i % 7 == 1)
            {
                the_audio.Play(type_sound);
            }
            yield return new WaitForSeconds(0.01f);
        }

    }

	void Update () 
    {
        if (is_talking && key_activated)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                key_activated = false;
                count++;
                text.text = "";
                the_audio.Play(enter_sound);

                if (count == list_sentences.Count)
                {
                    StopAllCoroutines();
                    ExitDialogue();                    
                }
                else
                {
                    StopAllCoroutines();
                    if (only_text)
                        StartCoroutine(StartTextCoroutine());
                    else
                        StartCoroutine(StartDialogueCoroutine());
                }
            }
        }
	}
}