using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(BaseEventSO<>))]
public class BaseEventSOEditor<T> : Editor
{
    private BaseEventSO<T> BaseEventSO;

    private void OnEnable()
    {
        if (BaseEventSO == null)
        {
            BaseEventSO = target as BaseEventSO<T>;// TODO：搞懂target这些特殊方法属性
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("订阅数量: " + GetListeners().Count);

        foreach (var listener in GetListeners())
        {
            EditorGUILayout.LabelField(listener.ToString());    // 显示监听者的名称
        }
    }

    private List<MonoBehaviour> GetListeners()
    {
        List<MonoBehaviour> listeners = new List<MonoBehaviour>();

        if(BaseEventSO == null || BaseEventSO.OnEventRaised == null)
            return listeners; // 防止未运行时一直报空

        var subscribers = BaseEventSO.OnEventRaised.GetInvocationList(); // 获取所有订阅者
        foreach (var subscriber in subscribers)
        {
            var obj = subscriber.Target as MonoBehaviour;
            if(!listeners.Contains(obj))
            {
                listeners.Add(obj);
            }
        }

        return listeners;
    }
}