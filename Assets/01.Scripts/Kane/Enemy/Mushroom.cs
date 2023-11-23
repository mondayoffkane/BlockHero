using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Enemy
{

    public override void Fight()
    {


        InitStatus();
        StartCoroutine(Cor_Fight());
    }


    IEnumerator Cor_Fight()
    {
        yield return null;


        while (_enemyState != EnemyState.Dead || _enemyState != EnemyState.Victory)
        {

            switch (_enemyState)
            {
                case EnemyState.Wait:
                    if (_target == null) FindTarget();

                    else _enemyState = EnemyState.Move;

                    yield return null;
                    break;

                case EnemyState.Move:
                    if (_target != null)
                    {

                        transform.LookAt(_target.transform);
                        transform.Translate(Vector3.forward * _speed * Time.deltaTime);

                        if (Vector3.Distance(transform.position, _target.transform.position) <= _attackRange)
                        {
                            _enemyState = EnemyState.Attack;
                        }

                    }
                    else
                    {
                        _enemyState = EnemyState.Wait;
                    }
                    yield return null;
                    break;

                case EnemyState.Attack:
                    Attack();
                    yield return new WaitForSeconds(_attackInterval);
                    break;

                case EnemyState.Dead:


                    yield return null;
                    break;
            }



            yield return null;
        }



    }





}
