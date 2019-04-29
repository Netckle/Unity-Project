using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // Public
    public GameObject       blackImgPrefab;
    public GameObject       cardSystem;

    // Private
    private GameObject      blackImg;
    private SpriteRenderer  render;

    private WaitForSeconds  waitTime = new WaitForSeconds(0.05f);  

    public void OpenCardPanel(bool isOpen)
    {
        cardSystem.SetActive(isOpen);
    }

    public void SceneChange(string sceneName)
    {
        cardSystem.SetActive(false);
        StartCoroutine(Fading(sceneName));        
    }

    public IEnumerator Fading(string sceneName)
    {
        blackImg = Instantiate(blackImgPrefab, Vector3.zero, Quaternion.identity);
        render = blackImg.GetComponent<SpriteRenderer>();

        for (float i = 0f; i < 1; i += 0.1f)
        {
            render.color = new Color(255, 255, 255, i);
            yield return waitTime;
        }
        Destroy(blackImg);
        SceneManager.LoadScene(sceneName); 

        yield return new WaitForSeconds(0.1f); 
        GameManager.Instance().UpdateManager();      
    }
}
