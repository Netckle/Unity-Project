using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo = null;
	public Transform placeholderParent = null;

	GameObject placeholder = null;

   //public JsonTest jsonManager;
	
    private int cardSiblingIndex;
    public Transform parentToReturnToSave;

    private DropSpace dropzone;

    void Awake()
    {
        dropzone = GameObject.FindGameObjectWithTag("Panel").GetComponent<DropSpace>();
    }

	public void OnBeginDrag(PointerEventData eventData) 
	{		
        if (parentToReturnTo == null)
        {
            parentToReturnToSave = dropzone.First();
        }
        else
        {
            parentToReturnToSave = parentToReturnTo;
        }

        cardSiblingIndex = this.transform.GetSiblingIndex();
        

		placeholder = new GameObject();
		placeholder.transform.SetParent(this.transform.parent);
		LayoutElement le = placeholder.AddComponent<LayoutElement>();
		le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
		le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
		le.flexibleWidth = 0;
		le.flexibleHeight = 0;

		placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
		
		parentToReturnTo = this.transform.parent;
		placeholderParent = parentToReturnTo;
		this.transform.SetParent( this.transform.parent.parent );
		
		GetComponent<CanvasGroup>().blocksRaycasts = false;
	}
	
	public void OnDrag(PointerEventData eventData) 
	{		
		this.transform.position = eventData.position;

		if(placeholder.transform.parent != placeholderParent)
			placeholder.transform.SetParent(placeholderParent);

		int newSiblingIndex = placeholderParent.childCount;

		for(int i=0; i < placeholderParent.childCount; i++) 
		{
			if(this.transform.position.x < placeholderParent.GetChild(i).position.x) 
			{
				newSiblingIndex = i;

				if(placeholder.transform.GetSiblingIndex() < newSiblingIndex)
					newSiblingIndex--;

				break;
			}
		}

		placeholder.transform.SetSiblingIndex(newSiblingIndex);
	}
	
	public void OnEndDrag(PointerEventData eventData) 
	{      
		this.transform.SetParent(parentToReturnTo);
		this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

		GetComponent<CanvasGroup>().blocksRaycasts = true;
        Destroy(placeholder);

        DropSpace temp = parentToReturnTo.gameObject.GetComponent<DropSpace>();

        Debug.Log(parentToReturnToSave);
        Debug.Log(placeholderParent);

        if (parentToReturnToSave == placeholderParent)
        {
            
            if (transform.GetSiblingIndex() != cardSiblingIndex)
            {
                Debug.Log("같은 영역이지만 배열 순서가 다르다.");
                JsonTest.Instace().RemoveData(GetComponent<Card>().data, cardSiblingIndex);
                JsonTest.Instace().InsertData(GetComponent<Card>().data, transform.GetSiblingIndex());
            }
            else
            {
                Debug.Log("같은 위치라 아무것도 실행하지 않는다.");
            }
        }
        else if (parentToReturnToSave != placeholderParent)
        {
            if (temp.type == "UP")
            {
                Debug.Log("Data is Inserted...");
                Debug.Log("올릴 오브젝트의 번호 : " + this.gameObject.transform.GetSiblingIndex());
                JsonTest.Instace().InsertData(GetComponent<Card>().data, this.gameObject.transform.GetSiblingIndex());
            }

            else if (temp.type == "DOWN" && parentToReturnTo.GetComponent<DropSpace>().type == "DOWN")
            {
                Debug.Log("버릴 오브젝트의 번호 : " + this.gameObject.transform.GetSiblingIndex());
                JsonTest.Instace().RemoveData(GetComponent<Card>().data, cardSiblingIndex);
            }
        }
	}
}
