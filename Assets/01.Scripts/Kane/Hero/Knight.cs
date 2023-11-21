using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Hero
{
    public Knight(/* Knight _knight*/ )
    {
        //_maxHP = _knight._maxHP;
        _maxHP = 100f;



    }


    private void Start()
    {
        FindTarget();
    }



    protected override void Attack()
    {
        base.Attack();


    }

    

}
