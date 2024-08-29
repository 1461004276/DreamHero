using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDataSO", menuName = "Card/CardDataSO")]
public class CardDataSO : ScriptableObject
{
    public string cardName; // 卡牌名字
    public Sprite cardImage; // 卡牌图片
    public int cost; // 卡牌费用
    public CardType cardType; // 卡牌类型
    [TextArea]
    public string description; // 卡牌描述
    
    // 执行的实际效果
    public List<CardEffect> cardEffects;
}