using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject cardSystemPanel;

    public void OpenCardPanel(bool flag)
    {
        cardSystemPanel.SetActive(flag);
    }
}
