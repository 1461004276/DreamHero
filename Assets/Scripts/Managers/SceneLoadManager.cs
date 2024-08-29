using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public FadePanel fadePanel;
    private AssetReference currentScene;        // 当前场景
    public AssetReference map;                  // 地图
    public AssetReference menu;                  // 主菜单
    public AssetReference intro;                  // 开场动画

    private Vector2Int currentRoomVector; // 当前房间
    private Room currentRoom;

    [Header("广播事件")]
    public ObjectEventSO afterRoomLoadedEvent;
    public ObjectEventSO updateRoomEvent;

    private void Awake()
    {
        currentRoomVector = Vector2Int.one * -1;
        // LoadMenu();
        LoadIntro();
    }

    /// <summary>
    /// 在房间加载事件中监听
    /// </summary>
    /// <param name="data"></param>
    public async void OnLoadRoomEvent(object data)
    {
        if (data is Room)
        {
            currentRoom = (Room)data;
            RoomDataSO currentData = currentRoom.roomData;
            currentRoomVector = new Vector2Int(currentRoom.column, currentRoom.row);
            // 设置当前场景
            currentScene = currentData.sceneToLoad;

        }
        // 卸载场景
        await UnloadSceneTask();
        // 加载房间
        await LoadSceneTask(); // 用await启动异步操作

        afterRoomLoadedEvent.RaiseEvent(currentRoom, this); // 发出广播事件 通知GameManager更新房间数据
    }

    /// <summary>
    /// 异步操作加载场景
    /// </summary>
    /// <returns></returns>
    private async Awaitable LoadSceneTask() // awaitable 是 2023新的特性
    {
        var s = currentScene.LoadSceneAsync(LoadSceneMode.Additive);
        await s.Task;

        if (s.Status == AsyncOperationStatus.Succeeded) // 如果异步加载成功
        {
            fadePanel.FadeOut(0.4f);
            SceneManager.SetActiveScene(s.Result.Scene);
        }
    }

    /// <summary>
    /// 异步操作卸载场景
    /// </summary>
    /// <returns></returns>
    private async Awaitable UnloadSceneTask()
    {
        fadePanel.FadeIn(0.6f);
        await Awaitable.WaitForSecondsAsync(0.65f);
        await Awaitable.FromAsyncOperation(SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene()));
    }

    /// <summary>
    /// 监听返回房间的加载函数
    /// </summary>
    public async void LoadMap()
    {
        await UnloadSceneTask();
        if (currentRoomVector != Vector2.one * -1)
        {
            updateRoomEvent.RaiseEvent(currentRoomVector, this);
        }
        currentScene = map;
        await LoadSceneTask();
    }

    public async void LoadMenu()
    {
        if (currentScene != null)
            await UnloadSceneTask();

        currentScene = menu;
        await LoadSceneTask();
    }

    public async void LoadIntro()
    {
        if (currentScene != null)
            await UnloadSceneTask();

        currentScene = intro;
        await LoadSceneTask();
    }
}
