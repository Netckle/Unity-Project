using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{
    public string transferMapName; // 이동할 맵의 이름.

    public Transform target;
    public BoxCollider2D targetBound;

    public Animator anim_1;
    public Animator anim_2;

    public int door_count;

    [Tooltip("UP, DOWN, LEFT, RIGHT")]
    public string direction; // 캐릭터가 바라보고 있는 방향.
    private Vector2 vector; // GetFloat("DirX)

    [Tooltip("문이 있으면 : true, 문이 없으면 : false")]
    public bool door; // 문이 있나 없나.
    
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
        if (!door)
        {
            if (collision.gameObject.name == "Player")
            {
                StartCoroutine(TransferCoroutine());
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (door)
        {
            if (collision.gameObject.name == "Player")
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    vector.Set(thePlayer.animator.GetFloat("DirX"), thePlayer.animator.GetFloat("DirY"));
                    switch (direction)
                    {
                        case "UP":
                            if (vector.y == 1.0F)
                                StartCoroutine(TransferCoroutine());
                            break;
                        case "DOWN":
                            if (vector.y == -1.0F)
                                StartCoroutine(TransferCoroutine());
                            break;
                        case "RIGHT":
                            if (vector.x == 1.0F)
                                StartCoroutine(TransferCoroutine());
                            break;
                        case "LEFT":
                            if (vector.x == -1.0F)
                                StartCoroutine(TransferCoroutine());
                            break;
                        default:
                            StartCoroutine(TransferCoroutine());
                            break;
                    }
                    StartCoroutine(TransferCoroutine());
                }                
            }
        }
    }

    IEnumerator TransferCoroutine()
    {
        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        theFade.FadeOut();

        if (door)
        {            
            anim_1.SetBool("Open", true);

            if (door_count == 2)                
                anim_2.SetBool("Open", true);
        }

        yield return new WaitForSeconds(0.3F);

        theOrder.SetTransparent("Player");

        if (door)
        {            
            anim_1.SetBool("Open", false);

            if (door_count == 2)                
                anim_2.SetBool("Open", false);
        }

        yield return new WaitForSeconds(0.7F);

        theOrder.SetUnTransparent("Player");

        thePlayer.currentMapName = transferMapName;
        theCamera.SetBound(targetBound);
        theCamera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, theCamera.transform.position.z);
        thePlayer.transform.position = target.transform.position;
        theFade.FadeIn();
        theOrder.Move();
    }
}
