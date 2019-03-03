using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    static public CameraManager instance;

    public GameObject target;           // 카메라가 따라갈 대상.
    public float move_speed;            // 카메라의 속도.
    private Vector3 target_position;    // 대상의 현재 위치.

    // 박스 콜라이더 영역의 최소 최대 좌표값을 지닙니다.

    public BoxCollider2D bound;
    private Vector3 min_bound;
    private Vector3 max_bound;

    // 카메라의 반너비, 반높이 값을 지닐 변수.

    private float half_width;
    private float half_height;    

    // 카메라의 반높이값을 구할 속성을 이용하기 위한 변수.

    private Camera the_camera;   

    #region Singleton
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }
    #endregion Singleton

    void Start()
    {
        the_camera   = GetComponent<Camera>();
        min_bound    = bound.bounds.min;
        max_bound    = bound.bounds.max;
        half_height  = the_camera.orthographicSize;
        half_width   = half_height * Screen.width / Screen.height;
    }

    void Update()
    {
        if (target.gameObject != null)
        {
            target_position.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);

            this.transform.position = Vector3.Lerp(this.transform.position, target_position, move_speed * Time.deltaTime);

            float clampedX = Mathf.Clamp(this.transform.position.x, min_bound.x + half_width, max_bound.x - half_width);
            float clampedY = Mathf.Clamp(this.transform.position.y, min_bound.y + half_height, max_bound.y - half_height);

            this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);            
        }
    }

    public void SetBound(BoxCollider2D newBound)
    {
        bound = newBound;
        min_bound = bound.bounds.min;
        max_bound = bound.bounds.max;
    }
}
