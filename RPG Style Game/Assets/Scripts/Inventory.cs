using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private OrderManager theOrder;
    private AudioManager theAudio;
    public string key_sound;
    public string enter_sound;
    public string cancel_sound;
    public string open_sound;
    public string beep_sound;

    private InventorySlot[] slots; // 인벤토리 슬롯들

    private List<Item> inventoryItemList; // 플레이어가 소지한 아이템 리스트.
    private List<Item> inventoryTabList; // 선택한 탭에 따라 다르게 보여질 아이템 리스트.

    public Text Description_Text; // 부연 설명.
    public string[] tabDescription; // 탭 부연 설명.

    public Transform tf; // slot 부모 객체.

    public GameObject go; // 인벤토리 활성화 비활성화.
    public GameObject[] selectedTabImage;

    private int selectedItem; // 선택한 아이템.
    private int selectedTab; // 선택된 탭.

    private bool activated; // 인벤토리 활성화 시 true;
    private bool tabActivated; // 탭 활성화시 true;
    private bool itemActivated; // 아이템 활성화시 true;
    private bool stopKeyInput; // 키입력 제한 [소비할 때 질의가 나올 텐데, 그 때 키입력 방지]
    private bool preventExec; // 중복실행 제한.

    private WaitForSeconds waitTime = new WaitForSeconds(0.01F);

    void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();
        theAudio = FindObjectOfType<AudioManager>();
        inventoryItemList = new List<Item>();
        inventoryTabList = new List<Item>();
        slots = tf.GetComponentsInChildren<InventorySlot>();
    }

    public void ShowTab()
    {
        RemoveSlot();
        SelectedTab();
    }

    public void RemoveSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
            slots[i].gameObject.SetActive(false);
        }
    }

    public void SelectedTab()
    {
        Color color = selectedTabImage[selectedTab].GetComponent<Image>().color;
        color.a = 0f;

        for (int i = 0; i < selectedTabImage.Length; i++)
        {
            selectedTabImage[i].GetComponent<Image>().color = color;
        }
        Description_Text.text = tabDescription[selectedTab];
        StartCoroutine(SelectedTabEffectCoroutine());
    }

    IEnumerator SelectedTabEffectCoroutine()
    {
        StopAllCoroutines();
        while (tabActivated)
        {
            Color color = selectedTabImage[selectedTab].GetComponent<Image>().color;
            while(color.a < 0.5f)
            {
                color.a += 0.03f;
                selectedTabImage[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }
            while(color.a > 0f)
            {
                color.a -= 0.03f;
                selectedTabImage[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }

            yield return new WaitForSeconds(0.3f);


        }
    }

    void Update()
    {
        if (!stopKeyInput)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                activated = !activated;

                if (activated)
                {
                    theOrder.NotMove();
                    theAudio.Play(open_sound);
                    go.SetActive(true);
                    selectedTab = 0;
                    tabActivated = true;
                    itemActivated = false;
                    ShowTab();
                }
                else
                {
                    theAudio.Play(cancel_sound);
                    StopAllCoroutines();
                    go.SetActive(false);
                    tabActivated = false;
                    itemActivated = false;
                    theOrder.canMove();
                }
            }
        }
    }
}
