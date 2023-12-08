using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float _damage;
    public float _attackInterval = 1f;
    public float _maxHP = 10f;
    public float _currentHP = 10f;





    public virtual void SetInit(int _level)
    {
        _damage = 5 + 2 * _level;
        _attackInterval = 2f - 0.1f * (float)_level < 0 ? 0.1f : 2f - 0.1f * (float)_level;
        _maxHP = 10 + 10 * _level;
        _currentHP = _maxHP;


    }

}
