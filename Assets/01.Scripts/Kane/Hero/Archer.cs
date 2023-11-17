using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Block
{
    public Archer(Archer archer )
    {
        _maxHP = archer._maxHP;

    }




    protected override void Attack()
    {
        base.Attack();


    }
}
