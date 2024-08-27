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

public enum RoomState
{
    Locked,
    Visited,
    Attainable
}

public enum CardType
{
    Attack,
    Defense,
    Abilities
}

public enum EffectTargetType
{
    Self,
    Target,
    All
}