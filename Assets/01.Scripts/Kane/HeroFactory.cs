using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HeroFactory : MonoBehaviour
{


    public SkinnedMeshRenderer _guage_Obj;
    public Image _bluePrint_Img;



    public float _currentTime = 0f;
    public float _maxTime = 1f;

    public bool isProduction = false;

    // ===============================================

    private void OnEnable()
    {
        if (_guage_Obj == null) _guage_Obj = transform.Find("Guage_Obj").GetComponent<SkinnedMeshRenderer>();
        if (_bluePrint_Img == null) _bluePrint_Img = transform.Find("Canvas").GetChild(0).GetComponent<Image>();

        StartCoroutine(Cor_Update());
    }


    IEnumerator Cor_Update()
    {
        while (true)
        {
            yield return null;

            if (isProduction)
            {
                _currentTime += Time.deltaTime;
                _guage_Obj.SetBlendShapeWeight(0, _currentTime / _maxTime);

                if (_currentTime >= _maxTime)
                {
                    _currentTime = 0f;
                }
            }

        }
    }


    public void MakeHeroOnOff(bool isOn)
    {
        isProduction = isOn;

        if (isProduction)
        {



        }
        else
        {

        }
    }

    public void SetRecipe(Recipe_Model _newRecipe)
    {
        _bluePrint_Img.sprite = _newRecipe._bluePrint_Sprite;

    }



}
