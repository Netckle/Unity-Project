using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp : MonoBehaviour
{   
    public float horPaddingSpace = 0.0f;
    public float moveSpeed = 0.0f;
    private bool isRight = true;
    private bool isTalking = false;
    private int[] dialogueIndexRange = { -1, -1 };
    private Vector3 targetPos;
    private Vector3 movement;
    private Rigidbody2D rigid;
    private TYPE dialogueType = TYPE.NORMAL;   

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {      

        if ((isTalking) && (Input.GetKeyDown(KeyCode.O)))
        {
            FindObjectOfType<DialogueManager>().DisplayNextSentence(dialogueType);
        }

        if (FindObjectOfType<DialogueManager>().GetDialogueEnd())
        {
            isTalking = false;
        }
    }

    void FixedUpdate()
    {
        if (!isTalking) Move();
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

        transform.position += move_velocity * moveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "NPC")
        {
            NPCController temp = collider.gameObject.GetComponent<NPCController>();
            dialogueIndexRange[0] = temp.normalIndexRange[0];
            dialogueIndexRange[1] = temp.normalIndexRange[1];

            dialogueType = temp.dialogueType;

            Debug.Log(collider.gameObject.name + "와 충돌함");

            targetPos = collider.gameObject.transform.position;

            if (isRight)        
                targetPos.x -= horPaddingSpace;
            else if (!isRight)
                targetPos.x += horPaddingSpace;            
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "NPC" && (Input.GetKeyDown(KeyCode.I)))
        {
            isTalking = true;
            
            Debug.Log(collider.gameObject.name + "와 대화를 시작합니다.");

            StartCoroutine("MoveToPlayerForTalk");           

            FindObjectOfType<DialogueManager>().            
            StartDialogue(GameObject.Find("LoadCSV").GetComponent<LoadCSV>().GetDialogueData(),
            dialogueIndexRange, dialogueType);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "NPC")
        {
            dialogueIndexRange[0] = -1;
            dialogueIndexRange[1] = -1;
        }
    }

    IEnumerator MoveToPlayerForTalk()
    {
        while((transform.position.x >= targetPos.x))
        {
            if (isRight)
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;            

            yield return null;
        }
    }
}
