using System;


//通过Flags特性，可以给枚举类型实现编辑监视器中多选的效果
//不过需要给枚举类型设置数值，且数值为2的幂次方值
//利用二进制数值或运算的特性，可以实现多选
[Flags]
public enum RoomType
{
    MinorEnemy = 1,
    EliteEnemy = 2,
    Shop = 4,
    Treasure = 8,
    ResetRoom = 16,
    Boss = 32
}

public enum RoomState // 房间状态
{
    Locked,// 上锁
    Visited,// 已访问
    Attainable// 可以访问
}

public enum CardType //卡牌类型
{
    Attack,//攻击
    Defense,//防御
    Abilities//能力
}
// 卡牌效果目标类型
public enum EffectTargetType
{
    Self,//自身
    Target,//目标
    All//全体
}