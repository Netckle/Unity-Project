using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private DataBaseManager the_database;
    private OrderManager the_order;
    private AudioManager the_audio;
    private OkOrCancel the_ooc;

    public string key_sound;
    public string enter_sound;
    public string cancel_sound;
    public string open_sound;
    public string beep_sound;

    private InventorySlot[] slots; // 인벤토리 슬롯.

    private List<Item> inventory_item_list; // 플레이어가 소지한 아이템 리스트.
    private List<Item> inventory_tab_list;  // 선택한 탭에 따라 다르게 보여질 아이템 리스트.

    public Text description_text;    // 부연 설명.
    public string[] description_tab; // 탭 부연 설명.

    public Transform parent_slot; // 슬롯 오브젝트의 부모 객체의 위치.

    public GameObject[] selected_tab_images;
    public GameObject object_inventory;      // 인벤토리 활성화 비활성화.    
    public GameObject object_ooc;            // 선택지 활성화 비활성화.

    private FloatingTextTrigger floating_text_trigger;

    private int selected_item_num; // 선택한 아이템 칸 번호.
    private int selected_tab_num;  // 선택된 탭 칸 번호.

    private bool inventory_activated; // 인벤토리 창 활성화 시 TRUE.
    private bool tab_activated;       // 탭 활성화 시 TRUE;
    private bool item_activated;      // 아이템 활성화 시 TRUE;
    private bool stop_key_input;      // 키 입력 제한 : 아이템을 소비할 때 확인 메세지가 나올 텐데, 그 때 키입력을 방지함.
    private bool prevent_exception;   // 중복 실행 제한.

    private WaitForSeconds wait_time = new WaitForSeconds(0.01F);

    void Start()
    {
        instance = this;        
        the_audio = FindObjectOfType<AudioManager>();
        the_order = FindObjectOfType<OrderManager>();
        the_database = FindObjectOfType<DataBaseManager>();
        the_ooc = FindObjectOfType<OkOrCancel>();

        inventory_item_list = new List<Item>();
        inventory_tab_list = new List<Item>();
        slots = parent_slot.GetComponentsInChildren<InventorySlot>();
        floating_text_trigger = FindObjectOfType<FloatingTextTrigger>();
    }

    void Update()
    {
        if (!stop_key_input)
        {
            if (Input.GetKeyDown(KeyCode.I)) // 인벤토리 창 열기.
            {
                inventory_activated = !inventory_activated;

                if (inventory_activated)
                {
                    the_order.NotMove();
                    the_audio.Play(open_sound);

                    object_inventory.SetActive(true);
                    selected_tab_num = 0;

                    tab_activated = true;
                    item_activated = false;

                    ShowTab();
                }
                else
                {
                    the_audio.Play(cancel_sound);

                    StopAllCoroutines();

                    object_inventory.SetActive(false);

                    tab_activated = false;
                    item_activated = false;

                    the_order.canMove();
                }
            }

            if (inventory_activated)
            {
                if (tab_activated) // 탭 활성화시 키입력 처리.
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if (selected_tab_num < selected_tab_images.Length - 1)
                            selected_tab_num++;
                        else
                            selected_tab_num = 0;

                        the_audio.Play(key_sound);
                        SelectedTab();                        
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if (selected_tab_num > 0)
                            selected_tab_num--;
                        else
                            selected_tab_num = selected_tab_images.Length - 1;

                        the_audio.Play(key_sound);
                        SelectedTab();                        
                    }
                    else if (Input.GetKeyDown(KeyCode.Z))
                    {
                        the_audio.Play(enter_sound);

                        Color color = selected_tab_images[selected_tab_num].GetComponent<Image>().color;
                        color.a = 0.25f;

                        selected_tab_images[selected_tab_num].GetComponent<Image>().color = color;

                        item_activated = true;
                        tab_activated = false;

                        prevent_exception = true;

                        ShowItem();
                    }
                }                
                else if (item_activated) // 아이템 활성화 시 키입력 처리.
                {
                    if (inventory_tab_list.Count > 0)
                    {
                        if (Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            if (selected_item_num < inventory_tab_list.Count - 2)
                                selected_item_num += 2;
                            else
                                selected_item_num %= 2;

                            the_audio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            if (selected_item_num > 1)
                                selected_item_num -= 2;
                            else
                                selected_item_num = inventory_tab_list.Count - 1 - selected_item_num;

                            the_audio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            if (selected_item_num < inventory_tab_list.Count - 1)
                                selected_item_num++;
                            else
                                selected_item_num = 0;

                            the_audio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            if (selected_item_num > 0)
                                selected_item_num--;
                            else
                                selected_item_num = inventory_tab_list.Count - 1;

                            the_audio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.Z) && !prevent_exception)
                        {
                            if (selected_tab_num == 0) // 소모품
                            {
                                the_audio.Play(enter_sound);
                                stop_key_input = true;
                                // 물약을 마실 거냐? 같은 선택지 호출.

                                StartCoroutine(OOCCoroutine());
                            }
                            else if (selected_tab_num == 1)
                            {
                                // 장비 장착
                            }
                            else // 비프음 출력
                            {
                                the_audio.Play(key_sound);
                            }
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        the_audio.Play(cancel_sound);
                        StopAllCoroutines();
                        item_activated = false;
                        tab_activated = true;
                        ShowTab();
                    }
                }

                if (Input.GetKeyUp(KeyCode.Z)) // 중복 실행 방지.
                    prevent_exception = false;
            }     
        }
    }

    public void GetAnItem(int _item_id, int _count = 1)
    {
        for (int i = 0; i < the_database.itemList.Count; i++) // 데이터베이스 아이템 리스트에서 검색.
        {
            if (_item_id == the_database.itemList[i].item_id) // 만약 데이터베이스에 아이템 발견한다면...
            {
                string text_temp = the_database.itemList[i].item_name + " " + _count + "개 획득";
                floating_text_trigger.TriggerFloatingText(PlayerManager.instance.transform.position, 0, text_temp, Color.white, 25);

                for (int j = 0; j < inventory_item_list.Count; j++) // 소지품에 같은 아이템이 있는지 검색.
                {
                    if (the_database.itemList[j].item_id == _item_id) // 만약 소지품에 같은 아이템이 있다면, 개수만 증감시켜 줌.
                    {
                        if (inventory_item_list[j].item_type == Item.ItemType.USE)
                        {
                            inventory_item_list[j].item_count += _count;                            
                        }
                        else
                        {
                            inventory_item_list.Add(the_database.itemList[i]);
                        }
                        return;
                    }
                }
                inventory_item_list.Add(the_database.itemList[i]); // 소지품에 해당 아이템 추가.
                inventory_item_list[inventory_item_list.Count - 1].item_count = _count;
                return;
            }
        }
        Debug.LogError("데이터베이스에 해당 ID값을 가진 아이템이 존재하지 않습니다."); // 데이터베이스에 Item ID 가 없음.
    }

    public void RemoveSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
            slots[i].gameObject.SetActive(false);
        }
    } 

    public void ShowTab()
    {
        RemoveSlot();
        SelectedTab();
    }

    // 선택된 탭이 반짝이는 효과를 보여줍니다.
    IEnumerator SelectedTabEffectCoroutine()
    {
        while (tab_activated)
        {
            Color color = selected_tab_images[selected_tab_num].GetComponent<Image>().color;
            while(color.a < 0.5f)
            {
                color.a += 0.03f;
                selected_tab_images[selected_tab_num].GetComponent<Image>().color = color;
                yield return wait_time;
            }
            while(color.a > 0f)
            {
                color.a -= 0.03f;
                selected_tab_images[selected_tab_num].GetComponent<Image>().color = color;
                yield return wait_time;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    // 아이템 활성화 (inventoryTabList에 조건에 맞는 아이템들만 넣어주고, 인벤토리 슬롯에 출력)
    public void ShowItem()
    {
        inventory_tab_list.Clear();
        RemoveSlot();
        selected_item_num = 0;

        // 탭에 따른 아이템 분류. 그것을 인벤토리 탭 리스트에 추가.
        switch(selected_tab_num)
        {
            case 0:
                for (int i = 0; i < inventory_item_list.Count; i++)
                {
                    if (Item.ItemType.USE == inventory_item_list[i].item_type)
                        inventory_tab_list.Add(inventory_item_list[i]);
                }
                break;
            case 1:
                for (int i = 0; i < inventory_item_list.Count; i++)
                {
                    if (Item.ItemType.EQUIP == inventory_item_list[i].item_type)
                        inventory_tab_list.Add(inventory_item_list[i]);
                }
                break;
            case 2:
                for (int i = 0; i < inventory_item_list.Count; i++)
                {
                    if (Item.ItemType.QUEST == inventory_item_list[i].item_type)
                        inventory_tab_list.Add(inventory_item_list[i]);
                }
                break;
            case 3:
                for (int i = 0; i < inventory_item_list.Count; i++)
                {
                    if (Item.ItemType.ETC == inventory_item_list[i].item_type)
                        inventory_tab_list.Add(inventory_item_list[i]);
                }
                break;
        }

        // 인벤토리 탭 리스트의 내용물, 인벤토리 슬롯에 추가.
        for (int i = 0; i < inventory_tab_list.Count; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].AddItem(inventory_tab_list[i]);
        }

        SelectedItem();
    }

    // 선택된 탭을 제외하고 다른 모든 탭의 컬러 알파값 0으로 조정.
    public void SelectedTab()
    {
        Color color = selected_tab_images[selected_tab_num].GetComponent<Image>().color;
        color.a = 0f;

        for (int i = 0; i < selected_tab_images.Length; i++)
        {
            selected_tab_images[i].GetComponent<Image>().color = color;
        }
        description_text.text = description_tab[selected_tab_num];
        StartCoroutine(SelectedTabEffectCoroutine());
    }

    // 선택된 아이템을 제외하고 다른 모든 탭의 컬러 알파값을 0 으로 조정.
    public void SelectedItem()
    {
        StopAllCoroutines();

        if (inventory_tab_list.Count > 0)
        {
            Color color = slots[0].selected_item.GetComponent<Image>().color;
            color.a = 0f;

            for (int i = 0; i < inventory_tab_list.Count; i++)
                slots[i].selected_item.GetComponent<Image>().color = color;

            description_text.text = inventory_tab_list[selected_item_num].item_description;
            StartCoroutine(SelectedItemEffectCoroutine());
        }
        else
            description_text.text = "해당 타입의 아이템을 소유하고 있지 않습니다.";
    }

    // 선택된 아이템이 반짝이는 효과를 보여줍니다.
    IEnumerator SelectedItemEffectCoroutine()
    {
        while (item_activated)
        {
            Color color = slots[0].GetComponent<Image>().color;
            while(color.a < 0.5f)
            {
                color.a += 0.03f;
                slots[selected_item_num].GetComponent<Image>().color = color;
                yield return wait_time;
            }
            while(color.a > 0f)
            {
                color.a -= 0.03f;
                slots[selected_item_num].GetComponent<Image>().color = color;
                yield return wait_time;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }    

    // "확인", "취소" 선택창을 띄워줍니다.
    IEnumerator OOCCoroutine() 
    {
        object_ooc.SetActive(true);
        the_ooc.ShowTwoChoice("사용", "취소");

        yield return new WaitUntil(() => !the_ooc.activated);

        if (the_ooc.GetResult())
        {
            for (int i = 0; i < inventory_item_list.Count; i++)
            {
                if (inventory_item_list[i].item_id == inventory_tab_list[selected_item_num].item_id)
                {
                    the_database.UseItem(inventory_item_list[i].item_id);

                    if (inventory_item_list[i].item_count > 1)
                        inventory_item_list[i].item_count--;
                    else
                        inventory_item_list.RemoveAt(i);

                    //theAudio 아이템 먹는 소리 출력.                    

                    ShowItem();
                    break;
                }                
            }
        }

        stop_key_input = false;
        object_ooc.SetActive(false);
    }
}
