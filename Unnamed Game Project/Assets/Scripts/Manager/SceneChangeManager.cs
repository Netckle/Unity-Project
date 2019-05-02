using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeManager : MonoBehaviour
{
    public FadeController fader;

    void Start()
    {
        // 처음은 투명한 상태로 한다.
        fader.gameObject.GetComponent<SpriteRenderer>().color = new Color(255,255,255, 0);
    }

    // 카드 HUD용 씬 변경 함수.
    public void Activate(string stageName)
    {
        GameManager.Instance().canvasM.ControllPanel(HUDTYPE.CARD, false);
        GameManager.Instance().canvasM.ControllPanel(HUDTYPE.INVENTORY, false);

        StartCoroutine(CoActivate(stageName, true));
    }

    public void Activate(string stageName, bool toStage)
    {
        StartCoroutine(CoActivate(stageName, toStage));
    }

    IEnumerator CoActivate(string stage, bool toStage)
    {              
        fader.FadeIn(0.2f, ()=>UnityEngine.SceneManagement.SceneManager.LoadScene(stage));
        
        yield return new WaitForSeconds(1.0f);

        fader.FadeOut(0.2f);

        Debug.Log(toStage);
        //yield return new WaitForSeconds(1.0f);
        if (toStage)
        {
            GameManager.Instance().objectM.portal.SetActive(false);
            GameManager.Instance().stageM.GenerateStage();
            
        }
    }
}
