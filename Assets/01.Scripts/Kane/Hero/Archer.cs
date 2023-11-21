using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Hero
{

    public override void  Fight()
    {
        Debug.Log("Child");
        StartCoroutine(Cor_Fight());
    }


    IEnumerator Cor_Fight()
    {
        yield return null;

        _rig.isKinematic = false;
        isPlay = true;
        _colls[1].enabled = true;

        _colls[1].size = GetComponent<MeshFilter>().sharedMesh.bounds.size;
        _colls[1].center = GetComponent<MeshFilter>().sharedMesh.bounds.center;

        while (isPlay)
        {

            switch (_armyState)
            {
                case ArmyState.Wait:
                    if (_target == null) FindTarget();
                    else _armyState = ArmyState.Move;

                    yield return null;
                    break;

                case ArmyState.Move:
                    if (_target != null)
                    {

                        transform.LookAt(_target.transform);
                        transform.Translate(Vector3.forward * _speed * Time.deltaTime);

                        if (Vector3.Distance(transform.position, _target.transform.position) <= _attackRange)
                        {
                            _armyState = ArmyState.Attack;
                        }

                    }
                    else
                    {
                        _armyState = ArmyState.Wait;
                    }
                    yield return null;
                    break;

                case ArmyState.Attack:
                    Attack();
                    yield return new WaitForSeconds(_attackInterval);
                    break;

                case ArmyState.Dead:

                    isPlay = false;
                    yield return null;
                    break;
            }



            yield return null;
        }

        // add Victory Animation

    }










}
