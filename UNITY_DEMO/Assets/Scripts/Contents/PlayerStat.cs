using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    protected int _exp;
    [SerializeField]
    protected int _gold;

    public int Exp 
    { 
        get { return _exp; } 
        set 
        {
            _exp = value;
            // 레벨업 체크는 여기서
            int level = 1;
            while (true)
            {
                Data.Stat stat;
                if (Managers.Data.StatDict.TryGetValue(level + 1, out stat) == false) //다음 레벨 정보
                    break;
                if (_exp < stat.totalExp)
                    break;
                level++;

                if (level != Level)
                {
                    Debug.Log("Level Up!");
                    Level = level;
                    SetStat(Level);
                }
            }
        } 
    }
    public int Gold { get { return _gold; } set { _gold = value; } }

    private void Start()
    {
        _level = 1;
        _defense = 5;
        _moveSpeed = 5.0f;
        _exp = 0;
        _gold = 0;

        SetStat(_level);
    }

    public void SetStat(int level)
    {
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;
        Data.Stat stat = dict[Level];
        _hp = stat.maxHp;
        _maxHp = stat.maxHp;
        _attack = stat.attack;
    }

    protected override void OnDead(Stat attacker)
    {
        Debug.Log("Player Dead");
    }
}
