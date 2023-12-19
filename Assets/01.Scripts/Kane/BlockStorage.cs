using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BlockStorage : MonoBehaviour
{

    public int[] _blockCountArray
        = new int[System.Enum.GetValues(typeof(Block.BlockType)).Length];


    public Rail _prevNode;


    private void Start()
    {

        StartCoroutine(Cor_Update());
    }


    IEnumerator Cor_Update()
    {
        while (true)
        {
            yield return null;

            if (_prevNode._isReady && _prevNode.GetComponent<Rail>()._currentBlock != null)
            {
                PullBlock(_prevNode.GetComponent<Rail>()._currentBlock);
                _prevNode.GetComponent<Rail>()._currentBlock = null;
            }

        }
    }

    public void PullBlock(Transform _block)
    {
        DOTween.Sequence()
             .Append(_block.transform.DOMove(transform.position + Vector3.up, Managers._stageManager._railSpeed))
             .OnComplete(() =>
             {
                 _blockCountArray[(int)_block.GetComponent<Block>()._blockType]++;
                 Managers.Pool.Push(_block.GetComponent<Poolable>());
                 //Managers._stageManager.FactoryCheckButtons();
                 Managers._gameUi.MakeButtonOnOff();

                 for (int i = 0; i < 4; i++)
                 {
                     Managers._gameUi._blockCountTexts[i].text = _blockCountArray[i].ToString();
                 }

             });

    }



}
