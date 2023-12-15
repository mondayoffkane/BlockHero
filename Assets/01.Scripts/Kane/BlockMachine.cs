using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

public class BlockMachine : MonoBehaviour
{
    [FoldoutGroup("Upgrade")] public int _level;
    [FoldoutGroup("Upgrade")] public double[] _upgradePrices = new double[5];

    [FoldoutGroup("BlockMachine")] public Block.BlockType _spawnBlockType;
    [FoldoutGroup("BlockMachine")] public GameObject _blockPref;
    //[FoldoutGroup("BlockMachine")] public Material[] _colorMats = new Material[4];
    [FoldoutGroup("BlockMachine")] public float _spawnInterval = 1f;
    //[FoldoutGroup("BlockMachine")] public Transform _factoryTop_Obj;
    //[FoldoutGroup("BlockMachine")] public Rail _nextNode;
    [FoldoutGroup("BlockMachine")] public Transform _currentBlock;
    [FoldoutGroup("BlockMachine")] public MeshFilter _connectMeshfilter;
    [FoldoutGroup("BlockMachine")] public Mesh _connectMesh;
    //MeshRenderer _cubeObj;


    [FoldoutGroup("Setting")] public Mesh[] _topMeshes;
    [FoldoutGroup("Setting")] public Mesh[] _bodyMeshes;

    Transform _factoryTop_Obj;
    MeshFilter _topMeshfilter;
    MeshFilter _bodyMeshfilter;

    //Renderer _renderer;
    //Material _selfMat;




    // =======private ============
    HeroFactory _heroFactory;

    // =======================================
    private void OnEnable()
    {
        if (_blockPref == null) Resources.Load<GameObject>("Block_Pref");
        //if (_heroFactory == null) _heroFactory = Managers._stageManager._blockStorage;
        //if (_factoryTop_Obj == null) _factoryTop_Obj = transform.GetChild(0);
        //if (_cubeObj == null) _cubeObj = transform.Find("Cube_Obj").GetComponent<MeshRenderer>();
        //_renderer = GetComponent<Renderer>();
        //_renderer.sharedMaterial = Instantiate(_renderer.sharedMaterial);
        //_renderer.sharedMaterial.SetTextureOffset("_BaseMap", new Vector2(0f, 0.025f * (float)_spawnBlockType));

        _factoryTop_Obj = transform.Find("Top_Obj");
        _topMeshfilter = _factoryTop_Obj.GetComponent<MeshFilter>();
        _bodyMeshfilter = transform.GetComponent<MeshFilter>();


        if (_connectMeshfilter != null)
            _connectMeshfilter.sharedMesh = _connectMesh;

        SetBlockType((int)_spawnBlockType);

        LoadData();

        StartCoroutine(Cor_Update());


    }

    public void LoadData()
    {
        _spawnInterval = 6f - 1f * _level;
    }

    public void SetBlockType(int _num)
    {
        _spawnBlockType = (Block.BlockType)_num;

        //_renderer.sharedMaterial.SetTextureOffset("_BaseMap", new Vector2(0f, 0.025f * (float)_spawnBlockType));
        //_selfMat.SetTextureOffset("_MainTex", new Vector2(0f, 0.025f * _num));
        //_factoryTop_Obj.GetComponent<Renderer>().sharedMaterial = _colorMats[_num];

        _topMeshfilter.sharedMesh = _topMeshes[_num];
        _bodyMeshfilter.sharedMesh = _bodyMeshes[_num];


        // add material change

    }

    public void UpgradeMachine()
    {
        Managers._stageManager.CalcMoney(_upgradePrices[_level]);
        _level++;
        _spawnInterval = 6f - 1f * _level;
        CheckPrice();
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
        if (_currentBlock == null)
        {
            Block _block = Managers.Pool.Pop(_blockPref, transform).GetComponent<Block>();
            _block.SetInit(_spawnBlockType);

            _block.transform.position = transform.position + Vector3.up;

            DOTween.Sequence().Append(_factoryTop_Obj.DOLocalMoveY(1f, 0.25f)).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Linear);

            _currentBlock = _block.transform;




            //DOTween.Sequence().AppendInterval(0.25f)
            //    .AppendCallback(() => _nextNode.PushBlock(_block.transform));

        }

    }


    public void CheckPrice()
    {


        if (_level < _upgradePrices.Length)
        {
            Managers._gameUi.BlockMachine_Upgrade_Price_Text.text = $"{_upgradePrices[_level]}";
            Managers._gameUi.BlockMachine_Status_Text.text = $"{_spawnInterval}s";

            Managers._gameUi.BlockMachine_UpgradeValue_Text.text = $"-1s";

            if (Managers._stageManager._money >= _upgradePrices[_level])
            {
                Managers._gameUi.BlockMachine_Upgrade_Button.interactable = true;

            }
            else
            {
                Managers._gameUi.BlockMachine_Upgrade_Button.interactable = false;
            }
        }
        else
        {
            Managers._gameUi.BlockMachine_Upgrade_Price_Text.text = $"Max";
            Managers._gameUi.BlockMachine_Status_Text.text = $"{_spawnInterval}s";
            Managers._gameUi.BlockMachine_UpgradeValue_Text.text = $"";
            Managers._gameUi.BlockMachine_Upgrade_Button.interactable = false;
        }
    }





}
