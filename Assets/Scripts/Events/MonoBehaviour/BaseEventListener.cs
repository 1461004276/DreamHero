using System;
using UnityEngine;
using UnityEngine.Events;

public class BaseEventListener<T> : MonoBehaviour
{
    public BaseEventSO<T> eventSO; // 监听一个事件
    public UnityEvent<T> response;// 事件响应 在面板中添加绑定脚本中的方法

    // 启用时 将事件响应方法注册到事件中
    private void OnEnable() {
        if(eventSO != null)
        {
            eventSO.OnEventRaised += OnEventRaised;
        }
    }
    // 注销事件中的方法
    private void OnDisable() {
        if(eventSO != null)
        {
            eventSO.OnEventRaised -= OnEventRaised;
        }
    }
    // 事件响应方法
    private void OnEventRaised(T value)
    {
        response.Invoke(value);
    }
}
