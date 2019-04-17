using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour
{
    public int[] normalIndexRange;          // 출력할 대화의 범위입니다. 
    public TYPE dialogueType = TYPE.NORMAL; // 출력할 대화 패널의 종류입니다.
}
