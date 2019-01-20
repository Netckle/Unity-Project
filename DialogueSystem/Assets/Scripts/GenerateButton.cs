using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateButton : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject panel;
    private int count = 0;

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            MakeButton();
        }
    }
    public void MakeButton()
    {
        for (int i = 0; i < 2; i++)
        {
        GameObject button = (GameObject) Instantiate(buttonPrefab);

        button.transform.position = panel.transform.position;        

        button.GetComponent<RectTransform>().SetParent(panel.transform);        
        
        count++;
        }
    }
}
