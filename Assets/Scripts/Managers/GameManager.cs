using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("地图布局")]
    public MapLayoutSO mapLayout; // 游戏地图数据 传入方便修改

    public List<EnemyBase> aliveEnemyList = new List<EnemyBase>();

    [Header("广播事件")]
    public ObjectEventSO gameWinEvent;
    public ObjectEventSO gameOverEvent;

    /// <summary>
    /// 更新房间的事件监听函数，加载地图
    /// </summary>
    /// <param name="roomVector"></param>
    public void UpdateMapLayoutData(object value)
    {
        Vector2Int roomVector = (Vector2Int)value;
        if (mapLayout.mapRoomDataList.Count == 0)
            return;

        // 从布局数据中找到当前房间
        var currentRoom = mapLayout.mapRoomDataList.Find(r => r.column == roomVector.x && r.row == roomVector.y);

        // 设置当前房间以及同列房间的状态
        currentRoom.roomState = RoomState.Visited;
        // 寻找同列房间并上锁
        var sameColumnRooms = mapLayout.mapRoomDataList.FindAll(r => r.column == currentRoom.column);

        foreach (var room in sameColumnRooms)
        {
            if (room.row != roomVector.y) // 除了当前房间，其他房间都上锁
            {
                room.roomState = RoomState.Locked;
            }
        }

        // 设置连线房间为可达
        foreach (var link in currentRoom.linkTo)
        {
            var linkedRoom = mapLayout.mapRoomDataList.Find(r => r.column == link.x && r.row == link.y);
            linkedRoom.roomState = RoomState.Attainable;
        }

        aliveEnemyList.Clear();
    }

    /// <summary>
    /// 事件函数，加载房间后添加所有敌人
    /// </summary>
    /// <param name="obj"></param>
    public void OnRoomLoadedEvent(object obj)
    {
        var enemies = FindObjectsByType<EnemyBase>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var enemy in enemies)
        {
            aliveEnemyList.Add(enemy);
        }
    }

    /// <summary>
    /// 事件函数，角色死亡，发出对应通知
    /// </summary>
    /// <param name="character"></param>
    public void OnCharacterDeadEvent(object character)
    {
        if (character is Player)
        {
            // 发出失败的通知
            StartCoroutine(EventDelayAction(gameOverEvent));
        }
        else if (character is Boss)
        {
            // TODO: 游戏通关事件
            StartCoroutine(EventDelayAction(gameOverEvent));
        }
        else if (character is EnemyBase)
        {
            aliveEnemyList.Remove(character as EnemyBase);

            if (aliveEnemyList.Count == 0)
            {
                // 发出胜利的通知
                StartCoroutine(EventDelayAction(gameWinEvent));
            }

        }
    }

    IEnumerator EventDelayAction(ObjectEventSO eventSO)
    {
        yield return new WaitForSeconds(1.5f);
        eventSO.RaiseEvent(null, this);
    }

    public void OnNewGameEvent()
    {
        mapLayout.mapRoomDataList.Clear();
        mapLayout.linePositionList.Clear();
    }

}
