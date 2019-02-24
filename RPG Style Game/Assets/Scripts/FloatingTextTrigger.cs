using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextTrigger : MonoBehaviour
{
    public GameObject prefabs_floating_text;
    public GameObject parent_canvas;

    private Vector3 vector_size_one =  new Vector3(1, 1, 1);

    public void TriggerFloatingText(Vector3 start_pos, float padding_size, string text, Color text_color, int text_size)
    {
        GameObject clone = Instantiate(prefabs_floating_text, start_pos, Quaternion.Euler(Vector3.zero));
        clone.gameObject.SetActive(true);

        // 클론 세부사항 설정.
        start_pos.y += padding_size;

        clone.GetComponent<FloatingText>().text.text = text;
        clone.GetComponent<FloatingText>().text.color = text_color;
        clone.GetComponent<FloatingText>().text.fontSize = text_size;

        clone.transform.SetParent(parent_canvas.transform);
        
        clone.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0); // UI 에서의 중심 위치.
        clone.gameObject.transform.localScale = vector_size_one;
    }
}
