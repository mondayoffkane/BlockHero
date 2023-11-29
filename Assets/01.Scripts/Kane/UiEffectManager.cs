using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UiEffectManager : MonoBehaviour
{
    public GameObject[] _firecrackers;



    public void FireCrackerEffect(int _num)
    {
        DOTween.Sequence().AppendCallback(() => _firecrackers[_num].SetActive(true))
            .AppendInterval(3f).
            AppendCallback(() => _firecrackers[_num].SetActive(false));

    }


}
