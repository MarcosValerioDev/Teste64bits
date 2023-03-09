using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] Trigger trigger;
    [SerializeField] Button buttonBuy;
    [SerializeField] Button buttonUnpause;
    [SerializeField] Button buttonRun;
    [SerializeField] Button buttonPunch;
    [SerializeField] UnityAction actionButtonPunch;
    [SerializeField] UnityAction<bool> actionButtonRun;
 
    public void SetPlayerActions(UnityAction<bool> runAction, UnityAction punchAction)
    {
        actionButtonRun += runAction;
        actionButtonPunch += punchAction;
    }

    public void ButtonActionRun(bool running)
    {
        actionButtonRun(running);
    }

    void SetButtonUI()
    {
        trigger.actionOnPointDown += ButtonActionRun;
        trigger.actionOnPointUp += ButtonActionRun;
        buttonPunch.onClick.AddListener(delegate { actionButtonPunch(); });
        buttonBuy.onClick.AddListener(delegate { GameManager.instance.BuyStack(); });
        buttonUnpause.onClick.AddListener(delegate { Continue(); });
      
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void Continue()
    {
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetButtonUI();
        Time.timeScale = 1f;
    }
}
