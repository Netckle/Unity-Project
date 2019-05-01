using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddlegroundScroller : MonoBehaviour
{
    private new Renderer renderer;
    public Player player;

    public float speed;
    public float offset;

    public void Start()
    {
        renderer = GetComponent<Renderer>();
        if (renderer == null)
            Debug.Log("없어");
    }

    public void Update()
    {
        offset = Time.time * speed;
        renderer.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
