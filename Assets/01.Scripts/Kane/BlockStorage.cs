using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BlockStorage : MonoBehaviour
{

    public Vector3 _scale_1 = new Vector3(1f, 1.1f, 1f);
    public Vector3 _scale_2 = new Vector3(1.05f, 0.9f, 1.05f);
    public float _scaleTime = 0.25f;

    // ====================================

    public int[] _blockCountArray
        = new int[System.Enum.GetValues(typeof(Block.BlockType)).Length];


    public Rail _prevNode;

    // ===================================

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
                  .Append(transform.DOScale(_scale_1, _scaleTime).SetEase(Ease.Linear))
                  .Append(transform.DOScale(_scale_2, _scaleTime).SetEase(Ease.Linear));

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
