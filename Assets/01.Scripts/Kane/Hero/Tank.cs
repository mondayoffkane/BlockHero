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


    public override void Fight()
    {
        //Debug.Log("Hero Fight");
        _heroState = HeroState.Wait;
        StartCoroutine(Cor_Update());
    }

    IEnumerator Cor_Update()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            yield return null;
            switch (_heroState)
            {
                case HeroState.Wait:
                    if (_target == null) FindTarget();

                    break;

                case HeroState.Move:
                    transform.LookAt(_target.transform);
                    transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, Time.deltaTime * _speed);

                    if (_target == null) _heroState = HeroState.Wait;
                    else if (Vector3.Distance(transform.position, _target.transform.position) <= _attackRange)
                    {
                        _heroState = HeroState.Attack;
                    }

                    yield return null;
                    break;

                case HeroState.Attack:
                    Attack();
                    float _interval = 1.1f - (0.25f * _attackInterval) < 0 ? 0.1f : 1.1f - (0.25f * _attackInterval);
                    yield return new WaitForSeconds(_interval);
                    break;




                    //default:
                    //    yield return null;
                    //    break;
            }




        }
    }

    protected override void Attack()
    {
        if (_target._currentHP > 0)
        {

            Transform _newBullet = Managers.Pool.Pop(_bullet_Pref).transform;
            _newBullet.transform.position = transform.position;
            //_newBullet.GetComponent<Rigidbody>().AddF
            _newBullet.DOMove(_target.transform.position, /*Vector3.Distance(transform.position, _target.transform.position) / _bulletSpeed*/ 1f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    base.Attack();
                    Managers.Pool.Push(_newBullet.GetComponent<Poolable>());
                });


            //base.Attack();
        }
        else
        {
            _target = null;
            _heroState = HeroState.Wait;
        }
    }

    public override void OnDamage(float _Damage)
    {
        //Debug.Log("Boss OnDamaged");
        if (_heroState != HeroState.Dead)
        {
            base.OnDamage(_Damage);

            if (_currentHP <= 0)
            {
                _currentHP = 0;
                _heroState = HeroState.Dead;
                Dead();
                //this.TaskDelay(2f, () => Managers.Pool.Push(GetComponent<Poolable>()));
            }
        }

    }



}
