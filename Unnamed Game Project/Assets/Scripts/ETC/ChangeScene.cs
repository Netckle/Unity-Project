using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // Singleton
    static Player instance = null; 

    public static Player Instace()
    {
        return instance;
    } 

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    //-----

    public GameObject blackImagePrefabs;
    private GameObject blackImage;
    private SpriteRenderer render;

    private WaitForSeconds waitTime = new WaitForSeconds(0.05f);

    public GameObject CardSystem;

    public void CardSystemSwitch(bool control)
    {
        CardSystem.SetActive(control);
    }

    public void SceneChange(string sceneName)
    {
        StartCoroutine(Fading(sceneName));
    }

    public IEnumerator Fading(string sceneName)
    {
        blackImage = Instantiate(blackImagePrefabs, Vector3.zero, Quaternion.identity);
        render = blackImage.GetComponent<SpriteRenderer>();

        for (float i = 0f; i < 1; i += 0.1f)
        {
            render.color = new Color(255, 255, 255, i);

            yield return waitTime;
        }

        Destroy(blackImage);

        SceneManager.LoadScene(sceneName);
    }
}
