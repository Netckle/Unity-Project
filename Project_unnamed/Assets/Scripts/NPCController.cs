using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour
{
    private Vector3 buttonPos;
    
    public float verticalPadding = 0.0f;
    public int[] normalIndexRange;
    public TYPE dialogueType = TYPE.NORMAL;
    public GameObject interactButton = null;

    void Start()
    {
        interactButton.SetActive(false);
        interactButton.transform.position = Vector3.zero;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            interactButton.SetActive(true);

            buttonPos = transform.position;
            buttonPos.y += verticalPadding;

            interactButton.transform.position = buttonPos;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            interactButton.SetActive(false);
        }
    }
}
