using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GamePlayPanel : MonoBehaviour
{
    private VisualElement rootElement;      // panel 根节点
    private Label energyAmountLabel, drawAmountLabel, discardAmountLabel, turnLabel;
    private Button endTurnButton;           // 结束回合按钮
    [Header("广播事件")]
    public ObjectEventSO playerTurnEndEvent;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;

        // 获取子元素
        energyAmountLabel = rootElement.Q<Label>("EnergyAmount");
        drawAmountLabel = rootElement.Q<Label>("DrawAmount");
        discardAmountLabel = rootElement.Q<Label>("DiscardAmount");
        turnLabel = rootElement.Q<Label>("TurnLabel");
        endTurnButton = rootElement.Q<Button>("EndTurn");

        endTurnButton.clicked += OnEndTurnButtonClicked;

        // 初始化 UI
        drawAmountLabel.text = "0";
        discardAmountLabel.text = "0";
        energyAmountLabel.text = "0";
        turnLabel.text = "游戏开始";
    }

    // 结束回合按钮点击事件
    private void OnEndTurnButtonClicked()
    {
        playerTurnEndEvent.RaiseEvent(null, this);
    }

    // 更新能量数字UI，事件函数
    public void UpdateEnergyAmount(int amount)
    {
        energyAmountLabel.text = amount.ToString();
    }

    // 更新抽牌数字 UI，事件函数
    public void UpdateDrawDeckAmount(int amount)
    {
        drawAmountLabel.text = amount.ToString();
    }

    // 更新弃牌数字 UI，事件函数
    public void UpdateDiscardDeckAmount(int amount)
    {
        discardAmountLabel.text = amount.ToString();
    }

    // 敌方回合开始，事件函数
    public void OnEnemyTurnBegin()
    {
        endTurnButton.SetEnabled(false);
        turnLabel.text = "敌方回合";
        turnLabel.style.color = Color.red;
    }

    // 玩家回合开始，事件函数
    public void OnPlayerTurnBegin()
    {
        endTurnButton.SetEnabled(true);
        turnLabel.text = "玩家回合";
        turnLabel.style.color = Color.white;
    }

    public void OnGameWinEvent()
    {
        endTurnButton.SetEnabled(false);
    }
}
