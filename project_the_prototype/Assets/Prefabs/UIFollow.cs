using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIFollow : MonoBehaviour
{
    public TextMeshProUGUI elementalText;
	public Transform target;

    public float xPadding;

    private string content;

    float x, y;

	void LateUpdate () 
    {
        Vector3 pos = new Vector3(target.position.x - xPadding, target.position.y, target.position.z);
		Vector3 screenPos = Camera.main.WorldToScreenPoint (pos);

		float x = screenPos.x;

		elementalText.transform.position = new Vector3(x, screenPos.y, elementalText.transform.position.z);

        x = Mathf.Round(target.transform.position.x * 10f) * 0.1f;
        y = Mathf.Round(target.transform.position.y * 10f) * 0.1f;

        content = "player pos<br>[x]:" + x + " [y]:" + y;

        elementalText.text = content;
	}
}
