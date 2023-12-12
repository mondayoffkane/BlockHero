using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

public class BlockMachine : MonoBehaviour
{
    [FoldoutGroup("Upgrade")] public int _level;



    //[FoldoutGroup("BlockFactory")] public List<Block.BlockType> _spawnBlockList;
    [FoldoutGroup("BlockMachine")] public Block.BlockType _spawnBlockType;
    [FoldoutGroup("BlockMachine")] public GameObject _blockPref;
    [FoldoutGroup("BlockMachine")] public Material[] _colorMats = new Material[4];
    [FoldoutGroup("BlockMachine")] public float _spawnInterval = 1f;
    [FoldoutGroup("BlockMachine")] public Transform _factoryTop_Obj;
    [FoldoutGroup("BlockMachine")] public Rail _nextNode;
    [FoldoutGroup("BlockMachine")] public MeshFilter _connectMeshfilter;
    [FoldoutGroup("BlockMachine")] public Mesh _connectMesh;
    MeshRenderer _cubeObj;








    // =======private ============
    HeroFactory _heroFactory;

    // =======================================
    private void OnEnable()
    {
        if (_blockPref == null) Resources.Load<GameObject>("Block_Pref");
        if (_heroFactory == null) _heroFactory = Managers._stageManager._heroFactory;
        if (_factoryTop_Obj == null) _factoryTop_Obj = transform.GetChild(0);
        if (_cubeObj == null) _cubeObj = transform.Find("Cube_Obj").GetComponent<MeshRenderer>();


        if (_connectMeshfilter != null)
            _connectMeshfilter.sharedMesh = _connectMesh;

        LoadData();

        StartCoroutine(Cor_Update());


    }

    public void LoadData()
    {
        _spawnInterval = 5.5f - 0.5f * _level;
    }

    public void SetBlockType(int _num)
    {
        _spawnBlockType = (Block.BlockType)_num;
    }

    public void UpgradeFactory()
    {
        _level++;
        _spawnInterval = 5.5f - 0.5f * _level;
    }



    IEnumerator Cor_Update()
    {
        yield return new WaitForSeconds(1f + Random.Range(0f, 1f));

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
        _block.SetInit(_spawnBlockType);

        _block.transform.position = transform.position;

        DOTween.Sequence().Append(_factoryTop_Obj.DOLocalMoveY(1.3f, 0.25f)).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Linear);
        DOTween.Sequence().AppendInterval(0.25f)
            .AppendCallback(() => _nextNode.MoveNextNode(_block.transform));
        //.Append(_block.transform.DOMove(_heroFactory.transform.position, 2f).SetEase(Ease.Linear))
        //.OnComplete(() => _heroFactory.PushBlock(_block));

        //DOTween.Sequence().Append(_factoryTop_Obj.DOLocalMoveY(1.3f, _spawnInterval * 0.5f)).SetEase(Ease.Linear)
        //    .Append(_factoryTop_Obj.DOLocalMoveY(2.7f, _spawnInterval * 0.5f)).SetEase(Ease.Linear);

    }



}
