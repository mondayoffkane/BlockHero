using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Monster
{

    public Mushroom(int _tempLevel = 0)
    {
        _level = _tempLevel;

        _maxHP = 10f * _level;
        _currentHP = _maxHP;
        _damage = 2f * _level;
        _speed = 5f + _level;
        _attackInterval = 1f;
    }


}
