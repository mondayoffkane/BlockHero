using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest : Hero
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

        //isPlay = true;
        //_rig.isKinematic = false;
        //_colls[1].enabled = true;

        _boxColl.size = GetComponent<MeshFilter>().sharedMesh.bounds.size;
        _boxColl.center = GetComponent<MeshFilter>().sharedMesh.bounds.center;

        while (_armyState != ArmyState.Dead && _armyState != ArmyState.Victory)
        {

            switch (_armyState)
            {
                case ArmyState.Wait:
                    if (_target == null) FindTarget();
                    else _armyState = ArmyState.Move;

                    //yield return null;
                    break;

                case ArmyState.Move:
                    if (_target != null || _target._currentHP > 0)
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
                    }
                    yield return null;
                    break;

                case ArmyState.Attack:
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
}
