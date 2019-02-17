using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferMap : MonoBehaviour
{
    public string targetMapName;

    public Transform        target;
    public BoxCollider2D    targetBound;

    private PlayerManager   thePlayer;
    private CameraManager   theCamera;
    private FadeManager     theFade;
    private OrderManager    theOrder;

    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
        theCamera = FindObjectOfType<CameraManager>();
        theFade   = FindObjectOfType<FadeManager>();
        theOrder  = FindObjectOfType<OrderManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            StartCoroutine(TransferCoroutine());
        }
    }

    IEnumerator TransferCoroutine()
    {   
        theOrder.PreLoadCharacterInform();
        theOrder.SetCanMove(false);
        theFade.FadeOut();
        
        yield return new WaitForSeconds(1.0F);

        thePlayer.currentMapName = targetMapName;

        theCamera.SetBound(targetBound);
        theCamera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, theCamera.transform.position.z);

        thePlayer.transform.position = target.transform.position;

        theFade.FadeIn();
        theOrder.SetCanMove(true);
    }
}
