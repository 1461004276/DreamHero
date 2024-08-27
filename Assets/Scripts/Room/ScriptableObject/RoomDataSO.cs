using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "RoomDataSO", menuName = "Map/RoomDataSO")]
public class RoomDataSO : ScriptableObject
{
    public Sprite roomIcon;
    public RoomType roomType;
    public AssetReference sceneToLoad; //需要加载的场景 通过Addressable管理资产
}