using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

public class BlockFactory : MonoBehaviour
{

    [FoldoutGroup("BlockFactory")] public List<Block.BlockType> _spawnBlockList;
    [FoldoutGroup("BlockFactory")] public GameObject _blockPref;
    [FoldoutGroup("BlockFactory")] public float _spawnInterval = 1f;





    // =======private ============
    HeroFactory _heroFactory;

    // =======================================
    private void Start()
    {
        if (_blockPref == null) Resources.Load<GameObject>("Block_Pref");
        if (_heroFactory == null) _heroFactory = Managers._stageManager._heroFactory;


        StartCoroutine(Cor_Update());

    }


    IEnumerator Cor_Update()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {

            Spawnblock();



            yield return new WaitForSeconds(_spawnInterval);
        }
    }



    [Button]
    public void Spawnblock()
    {
        Block _block = Managers.Pool.Pop(_blockPref, transform).GetComponent<Block>();
        _block.SetInit(_spawnBlockList[Random.Range(0, _spawnBlockList.Count)]);


        _block.transform.position = transform.position;
        _block.transform.DOMove(_heroFactory.transform.position, 2f).SetEase(Ease.Linear)
            .OnComplete(() => _heroFactory.PushBlock(_block));


    }



}
