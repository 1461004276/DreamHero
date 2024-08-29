using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public RoomDataSO roomData;
    public RoomState roomState;

    public int row;//行
    public int column;//列
    public List<Vector2Int> linkTo = new List<Vector2Int>(); // 连接的房间

    [Header("广播")]
    public ObjectEventSO loadRoomEvent; // 每次点击房间时触发 加载场景

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        // Debug.Log("点击房间: " + roomData.roomType);
        if (roomState == RoomState.Attainable) // 只有确定可以进入状态时才触发
            loadRoomEvent.RaiseEvent(this, this);
    }

    /// <summary>
    /// 设置房间, 在创建时调用
    /// </summary>
    /// <param name="row">房间的列</param>
    /// <param name="col">房间的行</param>
    /// <param name="roomData">房间数据</param>
    public void SetupRoom(int column, int row, RoomDataSO roomData)
    {
        this.row = row;
        this.column = column;
        this.roomData = roomData;

        spriteRenderer.sprite = roomData.roomIcon;
        // 设置房间状态
        spriteRenderer.color = roomState switch
        {
            RoomState.Locked => Color.grey,
            RoomState.Visited => new Color(0.6f, 0.8f, 0.6f, 1f),
            RoomState.Attainable => Color.white,
            _ => throw new System.NotImplementedException(),
        };
    }
}
