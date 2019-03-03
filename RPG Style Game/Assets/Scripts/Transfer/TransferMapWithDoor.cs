using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferMapWithDoor : MonoBehaviour
{
public string transfer_map_name; // 이동할 맵의 이름.

    public Transform target;
    public BoxCollider2D target_bound;

    public Animator anim_1;
    public Animator anim_2;

    public int door_count;

    [Tooltip("UP, DOWN, LEFT, RIGHT")]
    public string direction; // 캐릭터가 바라보고 있는 방향.
    private Vector2 vector;  // GetFloat("DirX")

    [Tooltip("문이 있으면 : TRUE, 문이 없으면 : FALSE")]
    public bool door; // 문이 있나 없나.
    
    private CameraManager the_camera;
    private PlayerManager the_player;
    private FadeManager the_fade;
    private OrderManager the_order;
    
    void Start()
    {
        the_camera = FindObjectOfType<CameraManager>();
        the_player = FindObjectOfType<PlayerManager>();
        the_fade = FindObjectOfType<FadeManager>();
        the_order = FindObjectOfType<OrderManager>();
    }

    // 같은 씬에서 이동이 이루어질 때, Bound를 교체합니다.
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
                    vector.Set(the_player.animator.GetFloat("DirX"), the_player.animator.GetFloat("DirY"));
                    
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
        the_order.PreLoadCharacter();

        the_order.NotMove();
        the_fade.FadeOut();

        if (door)
        {            
            anim_1.SetBool("Open", true);

            if (door_count == 2)                
                anim_2.SetBool("Open", true);
        }

        yield return new WaitForSeconds(0.3F);

        the_order.SetTransparent("Player");

        if (door)
        {            
            anim_1.SetBool("Open", false);

            if (door_count == 2)                
                anim_2.SetBool("Open", false);
        }

        yield return new WaitForSeconds(0.7F);

        the_order.SetUnTransparent("Player");

        the_player.current_map_name = transfer_map_name;

        the_camera.SetBound(target_bound);
        the_camera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, the_camera.transform.position.z);

        the_player.transform.position = target.transform.position;

        the_fade.FadeIn();
        the_order.canMove();
    }
}
