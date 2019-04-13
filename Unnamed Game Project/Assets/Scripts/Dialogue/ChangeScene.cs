using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string sceneName = null;
    public Image blackImage = null;
    public Animator anim = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine("Fading");
        }
    }

    public void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator Fading()
    {
        anim.SetBool("Fade", true);

        yield return new WaitUntil(()=>blackImage.color.a == 1);

        Debug.Log("Fade 코루틴 조건문 통과함.");

        SceneChange(sceneName);
    }
}
