using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    static public CameraManager instance;

    public GameObject           target;
    public float                moveSpeed;
    private Vector3             targetPosition;

    public BoxCollider2D        bound;
    private Vector3             minBound;
    private Vector3             maxBound;

    private float               halfWidth;
    private float               halfHeight;

    private Camera              theCamera; // halfHeight 값을 얻기 위해 필요함.

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
        theCamera   = GetComponent<Camera>();
        minBound    = bound.bounds.min;
        maxBound    = bound.bounds.max;
        halfHeight  = theCamera.orthographicSize;
        halfWidth   = halfHeight * Screen.width / Screen.height;
    }

    void Update()
    {
        if (target.gameObject != null)
        {
            targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);

            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed);

            float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
            float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

            this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);            
        }
    }
    
    public void SetBound(BoxCollider2D newBound)
    {
        bound = newBound;
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
    }
}
