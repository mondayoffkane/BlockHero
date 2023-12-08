using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tank : Hero
{
    public GameObject _bullet_Pref;

    public float _bulletSpeed = 1f;
    // ============================
    public override void SetInit(Recipe_Model _recipe)
    {
        base.SetInit(_recipe);




    }


    public void Fight()
    {
        StartCoroutine(Cor_Update());
    }

    IEnumerator Cor_Update()
    {
        yield return null;

        while (true)
        {

            switch (_heroState)
            {
                case HeroState.Wait:
                    if (_target == null) FindTarget();

                    break;

                case HeroState.Move:

                    if (_target == null) _heroState = HeroState.Wait;
                    else if (Vector3.Distance(transform.position, _target.transform.position) <= _attackRange)
                    {
                        Attack();
                    }
                    yield return new WaitForSeconds(_attackInterval);
                    break;




                default:
                    yield return null;
                    break;
            }




        }
    }

    protected override void Attack()
    {
        Transform _newBullet = Managers.Pool.Pop(_bullet_Pref).transform;
        //_newBullet.GetComponent<Rigidbody>().AddF
        _newBullet.DOMove(_target.transform.position, Vector3.Distance(transform.position, _target.transform.position) / _bulletSpeed)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                base.Attack();
                Managers.Pool.Push(_newBullet.GetComponent<Poolable>());
            });


        base.Attack();
    }




}
