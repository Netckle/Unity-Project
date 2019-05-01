using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
	public JsonManager jsonManager;

	public string type;

	public void First(GameObject obj)
	{
		Draggable d = obj.GetComponent<Draggable>();
		if(d != null) 
		{
			d.placeholderParent = this.transform;
		}
	}

	public void OnPointerEnter(PointerEventData eventData) 
    {
		if(eventData.pointerDrag == null)
			return;

		Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
		if(d != null) 
		{
			d.placeholderParent = this.transform;
		}
	}
	
	public void OnPointerExit(PointerEventData eventData) 
    {
		if(eventData.pointerDrag == null)
			return;

		Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
		if(d != null && d.placeholderParent==this.transform) 
        {
			d.placeholderParent = d.parentToReturnTo;
		}
	}
	
	public void OnDrop(PointerEventData eventData) 
	{
		Debug.Log (eventData.pointerDrag.name + " was dropped on " + gameObject.name);

		Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
		
		jsonManager.Add(eventData.pointerDrag.GetComponent<Card>().data);
		if(d != null) 
        {
			d.parentToReturnTo = this.transform;
		}
	}
}
