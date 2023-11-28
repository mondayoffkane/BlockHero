using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viking : Hero
{

    public override void InitStatus(HeroStatus HeroStatus, int Level)
    {
        base.InitStatus(HeroStatus, Level);
        Fight();
    }



    public override void Fight()
    {


        StartCoroutine(Cor_Fight());


    }


    IEnumerator Cor_Fight()
    {
        
        yield return new WaitForSeconds(1.5f);

        //_boxColl.size = GetComponent<MeshFilter>().sharedMesh.bounds.size;
        //_boxColl.center = GetComponent<MeshFilter>().sharedMesh.bounds.center;

        while (_armyState != ArmyState.Dead && _armyState != ArmyState.Victory)
        {

            switch (_armyState)
            {
                case ArmyState.Wait:
                    if (_target == null) FindTarget();
                    else
                    {
                        _armyState = ArmyState.Move;
                        _animator.SetBool("Run", true);
                    }
                    _animator.SetBool("Attack", false);

                    yield return null;
                    break;

                case ArmyState.Move:
                    if (_target != null && _target._currentHP > 0)
                    {

                        transform.LookAt(_target.transform);


                        if (Vector3.Distance(transform.position, _target.transform.position) <= _attackRange)
                        {
                            _armyState = ArmyState.Attack;
                        }
                        else
                        {
                            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
                        }

                    }
                    else
                    {
                        _target = null;
                        _armyState = ArmyState.Wait;
                        _animator.SetBool("Attack", false);
                    }
                    yield return null;
                    break;

                case ArmyState.Attack:
                    _animator.SetBool("Run", false);
                    _animator.SetBool("Attack", true);
                    Attack();
                    yield return new WaitForSeconds(_attackInterval);
                    break;

                case ArmyState.Dead:

                    //isPlay = false;
                    //yield return null;
                    break;
            }



            //yield return null;
        }

        // add Victory Animation

    }

    public override void TestFunc()
    {
        base.TestFunc();

        Debug.Log("Hero Child");
    }

    protected override void Attack()
    {
        if (!isReadySkill)
        {
            base.Attack();

        }
        else
        {
            Skill();
        }

    }

    protected override void Skill()
    {
        base.Skill();
    }



}
