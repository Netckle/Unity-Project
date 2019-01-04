using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp : MonoBehaviour
{   
    public float padding_space = 0.0f;
    public float move_speed = 0.0f;
    private bool collide_with_npc = false;
    private bool is_right = true;
    private bool is_talking = false;
    private Vector3 target_pos;
    private Rigidbody2D rigid;
    private Vector3 movement;

    private int start_dialogue_index = -1;
    private int end_dialogue_index = -1;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if ((collide_with_npc) && (Input.GetKeyDown(KeyCode.I)))
        {
            is_talking = true;
            Debug.Log("코루틴 시작 : MoveToPlayerForTalk");
            StartCoroutine("MoveToPlayerForTalk");

            FindObjectOfType<DialogueManager>().
            StartDialogue(GameObject.Find("LoadCSV").GetComponent<LoadCSV>().GetDialogueData(),
            start_dialogue_index,
            end_dialogue_index);
        }

        if ((is_talking) && (Input.GetKeyDown(KeyCode.O)))
        {
            FindObjectOfType<DialogueManager>().DisplayNextSentence();
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 move_velocity = Vector3.zero;

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            move_velocity = Vector3.left;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            move_velocity = Vector3.right;
        }

        transform.position += move_velocity * move_speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "NPC")
        {
            NPCController temp = collider.gameObject.GetComponent<NPCController>();
            start_dialogue_index = temp.start_dialogue_index;
            end_dialogue_index = temp.end_dialogue_index;

            Debug.Log(collider.gameObject.name);
            collide_with_npc = true;

            target_pos = collider.gameObject.transform.position;
            Debug.Log(target_pos);

            if (is_right)        
                target_pos.x -= padding_space;
            else if (!is_right)
                target_pos.x += padding_space;            
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "NPC")
        {
            start_dialogue_index = -1;
            end_dialogue_index = -1;
            
            collide_with_npc = false;
        }
    }

    IEnumerator MoveToPlayerForTalk()
    {
        while((transform.position.x >= target_pos.x))
        {
            if (is_right)
                transform.position += Vector3.left * move_speed * Time.deltaTime;
            

            yield return null;
        }
    }
}
