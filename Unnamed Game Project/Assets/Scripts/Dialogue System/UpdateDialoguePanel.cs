using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateDialoguePanel : MonoBehaviour
{
    public float padding = 0.0f;
    private string targetName = null;
    private Vector3 tempPos;
    private Vector3 targetPos;

    void Update()
    {
        if (targetName != null)
        {
            tempPos = GameObject.Find("Player").transform.position;
            tempPos.y += padding;

            targetPos = Camera.main.WorldToScreenPoint(tempPos);

            tempPos = Vector3.zero;

            transform.position = targetPos;
        }
    }

    public void SetTarget(string name)
    {
        targetName = name;
    }
}
