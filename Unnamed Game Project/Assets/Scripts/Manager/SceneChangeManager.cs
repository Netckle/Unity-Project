using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeManager : MonoBehaviour
{
    public FadeController fader;

    void Start()
    {
        fader.gameObject.GetComponent<SpriteRenderer>().color = new Color(255,255,255, 0);
    }

    public void Activate(string stageName)
    {
        StartCoroutine(CoActivate(stageName, false));
    }

    public void Activate(string stageName, bool onStage = false)
    {
        StartCoroutine(CoActivate(stageName, onStage));
    }

    IEnumerator CoActivate(string stageName, bool onStage)
    {      
        if (onStage)
        {
            fader.gameObject.transform.position = GameManager.Instance().stageM.generatedStages[GameManager.Instance().stageM.currentStageIndex].transform.position;
        }
        fader.FadeIn(0.2f, ()=>UnityEngine.SceneManagement.SceneManager.LoadScene(stageName));
        
        yield return new WaitForSeconds(1.0f);

        fader.FadeOut(0.2f);

        GameManager.Instance().stageM.GenerateStage();
    }
}
