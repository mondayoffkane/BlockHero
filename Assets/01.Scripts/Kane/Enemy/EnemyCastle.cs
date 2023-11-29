using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

[Button]
public class EnemyCastle : Enemy
{

    public override void OnDamage(float _tempDamage)
    {
        //DOTween.Kill(this.gameObject);
        if (!DOTween.IsTweening(this.gameObject))
        {
            transform.DOScale(0.9f, 0.1f).SetEase(Ease.Linear)
                .OnComplete(() => transform.DOScale(1f, 0.1f));
        }



        base.OnDamage(_tempDamage);
    }


}
