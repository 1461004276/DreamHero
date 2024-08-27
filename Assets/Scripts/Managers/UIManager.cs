using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI面板")]
    public GameObject gamePlayPanel;
    public GameObject gameWinPanel;
    public GameObject gameOverPanel;
    public GameObject pickCardPanel;
    public GameObject restRoomPanel;

    public void OnAfterRoomLoadedEvent(object data)
    {
        Room currentRoom = (Room)data;

        switch (currentRoom.roomData.roomType)
        {
            case RoomType.MinorEnemy:
            case RoomType.EliteEnemy:
            case RoomType.Boss:
                gamePlayPanel.SetActive(true);
                break;
            case RoomType.Shop:
                break;
            case RoomType.Treasure:
                break;
            case RoomType.ResetRoom:
                restRoomPanel.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// 关闭所有UI面板，  loadmap / load menu
    /// </summary>
    public void HideAllPanels()
    {
        gamePlayPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        restRoomPanel.SetActive(false);
    }

    public void OnGameWinEvent()
    {
        gameWinPanel.SetActive(true);
        gamePlayPanel.SetActive(false);
    }

    public void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);
        gamePlayPanel.SetActive(false);
    }

    public void OnPickCardEvent()
    {
        pickCardPanel.SetActive(true);
    }

    public void OnFinighPickCardEvent()
    {
        pickCardPanel.SetActive(false);
    }

}
