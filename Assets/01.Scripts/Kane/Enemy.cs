using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float _maxHP = 100f;
    public float _currentHP;

    public float _speed;
    public float _damage;



    private void Start()
    {
        Init();
    }

    void Init()
    {
        _currentHP = _maxHP;
    }


    public void OnDamage(float _tempDamage)
    {
        if (_currentHP > 0)
        {
            _currentHP -= _tempDamage;
            if (_currentHP <= 0)
            {
                Managers._puzzleManager._enemyList.Remove(this);
                Managers._puzzleManager.DeadEnemy();
                Managers.Pool.Push(transform.GetComponent<Poolable>());
            }
            //Destroy(this.gameObject);
        }

    }






}
