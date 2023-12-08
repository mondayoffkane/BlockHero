using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroFactory : MonoBehaviour
{
    public int[] _blockCountArray
        = new int[System.Enum.GetValues(typeof(Block.BlockType)).Length];





    public void PushBlock(Block _block)
    {
        _blockCountArray[(int)_block._blockType]++;

        Managers.Pool.Push(_block.GetComponent<Poolable>());

        Managers._stageManager.FactoryCheckButtons();

    }



}
