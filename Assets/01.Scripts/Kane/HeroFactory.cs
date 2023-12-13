using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HeroFactory : MonoBehaviour
{
    public int[] _blockCountArray
        = new int[System.Enum.GetValues(typeof(Block.BlockType)).Length];

    public Image _guageBar;




    public void PushBlock(Block _block)
    {
        DOTween.Sequence()
            .Append(_block.transform.DOMove(transform.position + Vector3.up, Managers._stageManager._railSpeed))
            .OnComplete(() =>
            {
                _blockCountArray[(int)_block._blockType]++;
                Managers.Pool.Push(_block.GetComponent<Poolable>());
                Managers._stageManager.FactoryCheckButtons();
            });


    }



    public void MakeHeroOnOff(bool isOn)
    {
        if (isOn)
        {

        }
        else
        {

        }
    }



}
