using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectEventSO))]
 // 继承基类 用作object类型的事件的编辑器显示
public class ObjectEventSOEditor : BaseEventSOEditor<object>
{
}