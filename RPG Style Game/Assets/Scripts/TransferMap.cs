using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{
    public string transferMapName; // 이동할 맵의 이름.

    public Transform target;
    public BoxCollider2D targetBound;
    
    private PlayerManager thePlayer;
    private CameraManager theCamera;
    private FadeManager theFade;
    private OrderManager theOrder;
    
    void Start()
    {
        theCamera = FindObjectOfType<CameraManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theFade   = FindObjectOfType<FadeManager>();
        theOrder = FindObjectOfType<OrderManager>();
    }

    // 같은 씬에서 이동이 이루어질 때 bound를 바꾸는 방법.
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            StartCoroutine(TransferCoroutine());
        }
    }

    IEnumerator TransferCoroutine()
    {
        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        theFade.FadeOut();

        yield return new WaitForSeconds(1f);

        thePlayer.currentMapName = transferMapName;
        theCamera.SetBound(targetBound);
        theCamera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, theCamera.transform.position.z);
        thePlayer.transform.position = target.transform.position;
        theFade.FadeIn();
        theOrder.Move();
    }
}
