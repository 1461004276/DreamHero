using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapLayoutSO", menuName = "Map/MapLayoutSO")]
public class MapLayoutSO : ScriptableObject {
    public List<MapRoomData> mapRoomDataList = new List<MapRoomData>();
    public List<LinePosition> linePositionList = new List<LinePosition>();
}

[System.Serializable]
public class MapRoomData // 存储地图房间数据
{
    public float posX, posY;
    public int column, row;
    public RoomDataSO roomData;
    public RoomState roomState;
    public List<Vector2Int> linkTo;
}

[System.Serializable]
public class LinePosition // 存储地图房间连接线数据
{
    public SerializeVector3 startPos, endPos;
}