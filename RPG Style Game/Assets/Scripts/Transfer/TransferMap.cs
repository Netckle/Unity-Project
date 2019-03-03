using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{
    public string transfer_map_name; // 이동할 맵의 이름.

    public Transform        target;
    public BoxCollider2D    target_bound;
    
    private CameraManager   the_camera;
    private PlayerManager   the_player;
    private FadeManager     the_fade;
    private OrderManager    the_order;
    
    void Start()
    {
        the_camera = FindObjectOfType<CameraManager>();
        the_player = FindObjectOfType<PlayerManager>();
        the_fade = FindObjectOfType<FadeManager>();
        the_order = FindObjectOfType<OrderManager>();
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
        the_order.PreLoadCharacter();
        the_order.NotMove();

        the_fade.FadeOut();

        yield return new WaitForSeconds(1.0F);

        the_player.current_map_name = transfer_map_name;

        the_camera.SetBound(target_bound);
        the_camera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, the_camera.transform.position.z);

        the_player.transform.position = target.transform.position;

        yield return new WaitForSeconds(0.3F);

        the_fade.FadeIn();
        the_order.canMove();
    }
}
