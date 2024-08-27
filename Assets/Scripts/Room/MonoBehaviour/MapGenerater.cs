using System.Collections.Generic;
using UnityEngine;

public class MapGenerater : MonoBehaviour
{
    [Header("地图配置表")]
    public MapConfigSO mapConfig;
    [Header("地图布局")]
    public MapLayoutSO mapLayout;
    [Header("预制体")]
    public Room roomPrefab;//房间预制体
    public LineRenderer linePrefab;//连线预制体

    private float screenHeight;
    private float screenWidth;
    private float columnWidth;              // 每列间距
    private Vector3 generatePoint;          // 地图房间生成位置
    public float border;                    // 边框预留空间

    private List<Room> rooms = new List<Room>();
    private List<LineRenderer> lines = new List<LineRenderer>();
    public List<RoomDataSO> roomDataList = new List<RoomDataSO>(); // 存储房间数据
    private Dictionary<RoomType, RoomDataSO> roomDataDict = new Dictionary<RoomType, RoomDataSO>(); // 房间枚举类型关联房间数据

    private void Awake()
    {
        // 获取屏幕宽度高度以及每列间距
        screenHeight = Camera.main.orthographicSize * 2;// 摄像机所监视区域的高度 为摄像机的 正交大小的二倍
        screenWidth = screenHeight * Camera.main.aspect;// 摄像机所监视区域的宽度 为高度乘以 摄像机视角的宽高比
        columnWidth = screenWidth / mapConfig.roomBlueprints.Count;

        foreach (var roomData in roomDataList)
        {
            roomDataDict.Add(roomData.roomType, roomData);
        }

    }

    private void OnEnable()
    {
        if (mapLayout.mapRoomDataList.Count > 0)
        {
            LoadMap();
        }
        else
        {
            CreateMap();
        }
    }

    /// <summary>
    /// 创建地图
    /// </summary>
    public void CreateMap()
    {
        // 前一列房间
        List<Room> previousColumnRooms = new List<Room>();

        // 生成每一列房间 一列一列生成 mapconfig中存储一列最小最大行数以及可以生成的房间类型
        for (int column = 0; column < mapConfig.roomBlueprints.Count; column++)
        {
            var blueprint = mapConfig.roomBlueprints[column];// 获得当前列的配置

            var amount = Random.Range(blueprint.min, blueprint.max); //获得一个在最大和最小之间的随机数

            var startHeight = screenHeight / 2 - screenHeight / (amount + 1);

            generatePoint = new Vector3(-screenWidth / 2 + border + columnWidth * column, startHeight, 0);// 每次右移一个列的宽度

            var newPosition = generatePoint;

            var roomOffsetY = screenHeight / (amount + 1);

            // 当前列房间
            List<Room> currentColumnRooms = new List<Room>();

            // 循环当前队列的所有房间数量生成房间
            for (int i = 0; i < amount; i++)
            {
                // 房间位置随机左右偏移
                if (column != 0)// 如果不是第一列
                {
                    newPosition.x = generatePoint.x + Random.Range(-border / 3, border / 3);
                }
                else
                if (column == mapConfig.roomBlueprints.Count - 1) // 如果是最后一列
                {
                    newPosition.x = screenWidth / 2 - border * 2; // 生成最后一列的房间 固定右边的左侧一些距离
                }

                newPosition.y = startHeight - roomOffsetY * i; // 该列下一个房间的位置
                // 生成房间
                var room = Instantiate(roomPrefab, newPosition, Quaternion.identity, transform);// 实例化预制体
                RoomType newType = GetRandomRoomType(mapConfig.roomBlueprints[column].roomType);// 获取随机房间类型

                if (column == 0)
                {
                    room.roomState = RoomState.Attainable;
                }
                else
                {
                    room.roomState = RoomState.Locked;
                }

                // 设置房间数据
                room.SetupRoom(column, i, GetRoomData(newType));
                rooms.Add(room);
                currentColumnRooms.Add(room);
            }
            // 如果不是第一列
            if (previousColumnRooms.Count > 0)
            {
                // 创建两个列表的房间连接
                CreateConnections(previousColumnRooms, currentColumnRooms);
            }
            previousColumnRooms = currentColumnRooms;
        }

        SaveMap();
    }

    /// <summary>
    /// 创建房间之间的连线
    /// </summary>
    /// <param name="column1">第一列房间</param>
    /// <param name="column2">第二列房间</param>
    private void CreateConnections(List<Room> column1, List<Room> column2)
    {
        // 第二列已经有连接的房间
        // hashset 用于去重 方便看那些房间已经链接上去了
        HashSet<Room> connectedColumn2Rooms = new HashSet<Room>();
        // 遍历第一列房间 去连线
        foreach (var room in column1)
        {
            var targetRoom = ConnectToRandomRoom(room, column2, false);
            connectedColumn2Rooms.Add(targetRoom);
        }

        // 检查确保 column2 中所有房间都有连线
        foreach (var room in column2)
        {
            // 如果房间没有连线 
            if (!connectedColumn2Rooms.Contains(room))
            {
                // 连接它
                ConnectToRandomRoom(room, column1, true);
            }
        }
    }

    /// <summary>
    /// 连接到随机房间
    /// </summary>
    /// <param name="room">起点房间</param>
    /// <param name="column2">目标列房间</param>
    /// <returns>连接到的房间</returns>
    private Room ConnectToRandomRoom(Room room, List<Room> column2, bool check)
    {
        Room targetRoom;

        targetRoom = column2[Random.Range(0, column2.Count)];

        if (check)
        {
            targetRoom.linkTo.Add(new Vector2Int(room.column, room.row));
        }
        else
        {
            room.linkTo.Add(new Vector2Int(targetRoom.column, targetRoom.row));
        }

        // 创建房间之间的连线
        var line = Instantiate(linePrefab, transform);
        line.SetPosition(0, room.transform.position);
        line.SetPosition(1, targetRoom.transform.position);
        lines.Add(line);

        return targetRoom;
    }

    /// <summary>
    /// 重新生成地图
    /// </summary>
    [ContextMenu("ReGenerate Map")] // 监视器中右键脚本的菜单
    public void ReGenerateMap()
    {
        foreach (var room in rooms)
        {
            Destroy(room.gameObject);
        }

        foreach (var line in lines)
        {
            Destroy(line.gameObject);
        }

        rooms.Clear();
        lines.Clear();

        CreateMap();
    }

    /// <summary>
    /// 依据房间类型获取房间数据
    /// </summary>
    /// <param name="roomType"></param>
    /// <returns></returns>
    private RoomDataSO GetRoomData(RoomType roomType)
    {
        return roomDataDict[roomType];
    }

    /// <summary>
    /// 随机生成房间类型
    /// </summary>
    /// <param name="flags"></param>
    /// <returns></returns>
    private RoomType GetRandomRoomType(RoomType flags)
    {
        string[] options = flags.ToString().Split(',');

        string randomOption = options[Random.Range(0, options.Length)];

        RoomType roomType = (RoomType)System.Enum.Parse(typeof(RoomType), randomOption); // 将字符串转为枚举

        return roomType;
    }

    /// <summary>
    /// 保存地图
    /// </summary>
    private void SaveMap()
    {
        mapLayout.mapRoomDataList = new List<MapRoomData>();

        // 添加已经生成的房间
        for (int i = 0; i < rooms.Count; i++)
        {
            var mapRoom = new MapRoomData()
            {
                posX = rooms[i].transform.position.x,
                posY = rooms[i].transform.position.y,
                column = rooms[i].column,
                row = rooms[i].row,
                roomData = rooms[i].roomData,
                roomState = rooms[i].roomState,
                linkTo = rooms[i].linkTo,
            };

            mapLayout.mapRoomDataList.Add(mapRoom);
        }

        mapLayout.linePositionList = new List<LinePosition>();
        // 添加连线
        for (int i = 0; i < lines.Count; i++)
        {
            var line = new LinePosition()
            {
                startPos = new SerializeVector3(lines[i].GetPosition(0)),
                endPos = new SerializeVector3(lines[i].GetPosition(1))
            };

            mapLayout.linePositionList.Add(line);
        }
    }

    /// <summary>
    /// 加载地图
    /// </summary>
    private void LoadMap()
    {
        // 读取房间数据生成房间
        for (int i = 0; i < mapLayout.mapRoomDataList.Count; i++)
        {
            MapRoomData roomData = mapLayout.mapRoomDataList[i];
            var newPos = new Vector3(roomData.posX, roomData.posY, 0);
            var newRoom = Instantiate(roomPrefab, newPos, Quaternion.identity, transform);
            newRoom.roomState = roomData.roomState;
            newRoom.SetupRoom(roomData.column, roomData.row, roomData.roomData);
            newRoom.linkTo = roomData.linkTo;

            rooms.Add(newRoom);
        }

        // 读取连线数据生成连线
        for (int i = 0; i < mapLayout.linePositionList.Count; i++)
        {
            var line = Instantiate(linePrefab, transform);
            line.SetPosition(0, mapLayout.linePositionList[i].startPos.GetVector3());
            line.SetPosition(1, mapLayout.linePositionList[i].endPos.GetVector3());

            lines.Add(line);
        }
    }
}
