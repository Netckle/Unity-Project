using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

// sealed을 사용하여 다른 클래스가 상속되지 못하도록 함.
public sealed class ItemIO
{
    public static void SaveDate()
    {
        // 인벤토리에서 슬롯을 관리해주는 리스트를 받아온다.
        List<GameObject> item = Inventory.Instance().AllSlot;
 
        XmlDocument XmlDoc  = new XmlDocument();                // XML문서 생성.
        XmlElement XmlEl    = XmlDoc.CreateElement("ItemDB");   // 요소 생성.
        XmlDoc.AppendChild(XmlEl);                              // 요소를 XML문서에 첨부.
 
        // 리스트의 총 크기(슬롯의 개수.)
        int Count = item.Count;
 
        for (int i = 0; i < Count; i++)
        {
            // 슬롯 리스트에서 슬롯을 하나씩 꺼내온다.
            Slot itemInfo = item[i].GetComponent<Slot>();
 
            // 슬롯이 비어있으면 저장할 필요가 없으므로 넘긴다.
            if (!itemInfo.isSlots())
                continue;
 
            // 필드(요소)를 생성.
            XmlElement ElementSetting = XmlDoc.CreateElement("Item");
 
            // 필드(요소)의 내용을 셋팅.
            ElementSetting.SetAttribute("SlotNumber", i.ToString());                                // n번째 슬롯에 아이템.
            ElementSetting.SetAttribute("Name"      , itemInfo.ItemReturn().Name);                  // 아이템의 이름.
            ElementSetting.SetAttribute("Count"     , itemInfo.slot.Count.ToString());              // 아이템의 개수. (ex: 현 슬롯에 겹쳐진 아이템 10개임.)
            ElementSetting.SetAttribute("MaxCount"  , itemInfo.ItemReturn().MaxCount.ToString());   // 아이템을 겹칠수 있는 한계.
            XmlEl.AppendChild(ElementSetting);                                                      // ItemDB요소에 위의 셋팅한 요소를 문서에 첨부.
        }
 
        // XML문서로 내보낸다. 인자로는 문서를 내보낼 경로.
        XmlDoc.Save(Application.streamingAssetsPath + "/Save/InventoryData.xml");
    }

    public static List<GameObject> Load(List<GameObject> SlotList)
    {
        if (!System.IO.File.Exists(Application.streamingAssetsPath + "/Save/InventoryData.xml"))
            return SlotList;
    
        XmlDocument XmlDoc  = new XmlDocument();                                         // 문서를 만듬.
        XmlDoc.Load(Application.streamingAssetsPath + "/Save/InventoryData.xml");       // 경로상의 XML파일을 로드
        XmlElement Xmlel    = XmlDoc["ItemDB"];                                            // 속성 ItemDB에 접속.
    
        foreach (XmlElement ItemElement in Xmlel.ChildNodes)
        {
            // 슬롯의 n번째 스크립트를 가져온다.
            Slot slot       = SlotList[System.Convert.ToInt32(ItemElement.GetAttribute("SlotNumber"))].GetComponent<Slot>();
    
            // 아이템 생성.
            Item item = new Item();
    
            // 아이템의 정보를 셋팅한다.
            string Name     = ItemElement.GetAttribute("Name");                              // 아이템 이름을 가져옴.
            int MaxCount    = System.Convert.ToInt32(ItemElement.GetAttribute("MaxCount")); // 겹칠수 있는 한계.
            item.Init(Name, MaxCount);                                                   // 위의 가져온 정보를 토대로 아이템의 정보를 초기화.
            
            int Count       = System.Convert.ToInt32(ItemElement.GetAttribute("Count"));      // 슬롯에 아이템을 n개 집어넣기 위해서 개수를 가져옴.
            for (int i = 0; i < Count; i++)
                slot.AddItem(item);
        }
    
        return SlotList;
    }
}

