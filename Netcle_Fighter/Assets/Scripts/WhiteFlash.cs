using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteFlash : MonoBehaviour {

    SpriteRenderer renderer;

    public Material material;

    void Start()
    {
        renderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SkillSpace")
        {
            Debug.Log("스킬과 충돌함");
            StartCoroutine(ShowHitFlash());
        }
    }

    IEnumerator ShowHitFlash()
    {
        // change the current shader
        renderer.material.shader = Shader.Find("PaintWhite");

        // show a white flash for a little moment
        yield return new WaitForSeconds(0.1f);

        // put again the shader it had before
        renderer.material = material;
    }
}
