using UnityEngine;

public class Player : CharacterBase
{
    public IntVariable playerMana;          // 玩家能量
    public int maxMana;

    public int CurrentMana
    {
        get => playerMana.currentValue;
        set => playerMana.SetValue(value);
    }

    private void OnEnable()
    {
        playerMana.maxValue = maxMana;
        CurrentMana = playerMana.maxValue;
    }

    /// <summary>
    /// 玩家新回合，监听事件函数
    /// </summary>
    public void NewTurn()
    {
        CurrentMana = maxMana;
    }
    // 更新能量 被 CardCost Event 监听
    public void UpdateMana(int cost)
    {
        CurrentMana -= cost;
        if (CurrentMana < 0)
        {
            CurrentMana = 0;
        }
    }

    public void NewGame()
    {
        CurrentHp = MaxHp;
        isDead = false;
        buffRound.currentValue = 0;
        NewTurn();
    }

    public void SetSleepAnimation()
    {
        animator.Play("death");
    }

}
