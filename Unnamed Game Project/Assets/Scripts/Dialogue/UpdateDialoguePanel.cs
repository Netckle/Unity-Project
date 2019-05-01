using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateDialoguePanel : MonoBehaviour // 대화하고 있는 대상의 머리 위로 대화창을 이동 시킨다.
{
    public float padding = 0.0f;
    public string targetName;

    private Vector3 targetPos;
    private Vector3 appliedTargetPos;

    void Update()
    {
        if (this.gameObject.activeSelf == true)
        {
            SetPanelPosition();
        }
    }

    public void SetTarget(string _targetName)
    {
        targetName = _targetName;
    }

    public void SetPanelPosition()
    {
        if (targetName == null)
            return;

        targetPos = GameObject.Find(targetName).transform.position;
        targetPos.y += padding;

        appliedTargetPos = Camera.main.WorldToScreenPoint(targetPos);
        targetPos = Vector3.zero;

        transform.position = appliedTargetPos;        
    }
}
