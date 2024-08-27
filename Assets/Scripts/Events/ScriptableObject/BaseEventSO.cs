using UnityEngine;
using UnityEngine.Events;

public class BaseEventSO<T> : ScriptableObject
{
    [TextArea]
    public string description; // 方便添加事件描述

    public UnityAction<T> OnEventRaised;// public 方便监听脚本 添加触发事件
    public string lastSender;

    // 事件触发 点击进行触犯事件供给 交互点击 进行触发 事件中的方法
    public void RaiseEvent(T value, object sender)
    {
        OnEventRaised?.Invoke(value);
        lastSender = sender.ToString(); // 记录最后一个发送者
    }
}