using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP : MonoBehaviour {

    public List<GameObject> portalList;
    public int listCount = 0;

    public GameObject temp; // 더미 오브젝트

    void Start()
    {
        temp = new GameObject();
        temp.transform.position = new Vector3(0, 0, 0);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Portal" && 
            Input.GetKeyDown(KeyCode.F))
        {
            portalList.Add(collision.gameObject);
            listCount++;

            if (listCount >= 2)
            {
                SwapPortalOBJ(); // 포탈스왑

                portalList.Clear();
                listCount = 0;
            }
        }
    }

    void SwapPortalOBJ()
    {        
        temp.transform.position = portalList[0].transform.position;
        portalList[0].transform.position = portalList[1].transform.position;
        portalList[1].transform.position = temp.transform.position;
    }
    
}
