using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class PuzzleManager : MonoBehaviour
{

    static public PuzzleManager _instance;

    //public Block.BlockType _blockType;



    public Block _selectBlock;
    public Block _targetBlock;


    // =====================
    Camera _mainCam;

    // =================


    //==== Puzzle
    public GameObject _block_Pref;
    public Transform _blockGroup;
    public Vector2Int _size = new Vector2Int(6, 6);
    public int _maxColorCount = 4;
    public Vector2 _posInterval = new Vector2(0f, -0.15f);
    [ShowInInspector] public Block[,] _grid;

    public float _blockMoveSpeed = 0.5f;
    public float _camz = 10f;

    public Vector3 _selectStartPos;
    public float _moveDistance = 1f;
    public Vector3 _dir;
    public float _dis;

    //[ShowInInspector]
    //public List<Block[]> _matchList = new List<Block[]>();


    private void Awake()
    {
        _instance = this;
    }




    void Start()
    {
        _mainCam = Camera.main;


        _grid = new Block[_size.x, _size.y];
        SpawnBlock();
        //SpawnStage();

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

        }




        if (Input.GetMouseButtonDown(0))
        {
            Ray ray;
            RaycastHit hit;

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red, 1.5f);

                if (hit.collider.tag == "Block")
                {
                    _selectBlock = hit.transform.GetComponent<Block>();
                    _selectBlock.gameObject.layer = LayerMask.NameToLayer("SelectBlock");
                    //CheckBlock(_selectBlock);
                    _selectStartPos = _selectBlock.transform.position;
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            //
            if (_selectBlock != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;


                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 temp = hit.point;
                    temp.y = 0;
                    //if (Vector3.Distance(_selectStartPos, _selectBlock.transform.position) <= _moveDistance)
                    //{
                    _selectBlock.transform.position = temp;

                    //}

                    _dir = (temp - _selectStartPos).normalized;
                    _dis = Vector3.Distance(_selectStartPos, _selectBlock.transform.position) <= _moveDistance ? Vector3.Distance(_selectStartPos, _selectBlock.transform.position) : _moveDistance;

                    _selectBlock.transform.position = _selectStartPos + _dir * _dis;


                }
            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (_selectBlock != null)
            {
                Ray ray;
                RaycastHit hit;

                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                int _mask = 1 << LayerMask.NameToLayer("WaitBlock");
                if (Physics.Raycast(ray, out hit, 1000, _mask))
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.red, 1.5f);

                    if (hit.collider.tag == "Block")
                    {
                        _targetBlock = hit.transform.GetComponent<Block>();
                        MoveBlock();
                    }
                }
                else if (_selectBlock != null)
                {
                    _selectBlock.SetOrigin();


                    _selectBlock.gameObject.layer = LayerMask.NameToLayer("WaitBlock");



                    _selectBlock = null;

                }

                Debug.Log("CheckBlock");
                CheckBlock();
            }
        }
    }


    [Button]
    public void CheckBlock() // check 3 match
    {
        bool ExistMatch = false;
        StartCoroutine(Cor_CheckBlock());

        IEnumerator Cor_CheckBlock()
        {
            yield return null;

            for (int j = 0; j < _size.y; j++) // horizontal check
            {
                for (int i = 0; i < _size.x - 2; i++)
                {
                    if ((_grid[i, j]._blockType == _grid[i + 1, j]._blockType)
                        && (_grid[i, j]._blockType == _grid[i + 2, j]._blockType)
                        && (_grid[i, j]._level == _grid[i + 1, j]._level)
                        && (_grid[i, j]._level == _grid[i + 2, j]._level)
                        )
                    {
                        _grid[i, j].isMatch = true;
                        _grid[i + 1, j].isMatch = true;
                        _grid[i + 1, j].isPromotion = true;
                        _grid[i + 2, j].isMatch = true;
                        ExistMatch = true;
                    }
                }
            }
            Debug.Log("Horizontal check");
            //yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < _size.x; i++) // vertical check
            {
                for (int j = 0; j < _size.y - 2; j++)
                {
                    if ((_grid[i, j]._blockType == _grid[i, j + 1]._blockType)
                        && (_grid[i, j]._blockType == _grid[i, j + 2]._blockType)
                        && (_grid[i, j]._level == _grid[i, j + 1]._level)
                        && (_grid[i, j]._level == _grid[i, j + 2]._level)
                        )
                    {
                        _grid[i, j].isMatch = true;
                        _grid[i, j + 1].isMatch = true;
                        _grid[i, j + 1].isPromotion = true;
                        _grid[i, j + 2].isMatch = true;
                        ExistMatch = true;
                    }
                }
            }
            Debug.Log("Vertical check");
            yield return new WaitForSeconds(0.5f);

            if (ExistMatch == true)
            {
                for (int i = 0; i < _size.x; i++)
                {
                    for (int j = 0; j < _size.y; j++)
                    {
                        _grid[i, j].OnMatchBlock(); // block merge

                    }
                }
                Debug.Log("OnMatch");
                //yield return new WaitForSeconds(0.5f);


                SpawnBlock();
            }

        }


    }

    //public void MergeBlocks() // 3 match and Pop
    //{


    //    SpawnBlock(); // 
    //}




    public void MoveBlock() // check can move position
    {

        int _count = 0;

        _count = Mathf.Abs(_selectBlock._pos.x - _targetBlock._pos.x);
        _count += Mathf.Abs(_selectBlock._pos.y - _targetBlock._pos.y);

        if (_count <= 1)
        {
            // Swap Block
            Vector2Int _tempPos = _selectBlock._pos;
            _selectBlock.SetPos(_targetBlock._pos.x + 2, _targetBlock._pos.y + 2);
            _targetBlock.SetPos(_tempPos.x + 2, _tempPos.y + 2);


            //check
            //Block _tempBlock = new Block();
            //_tempBlock = _grid[_selectBlock._pos.x + 2, _selectBlock._pos.y + 2];
            //_grid[_selectBlock._pos.x + 2, _selectBlock._pos.y + 2] = _targetBlock;
            //_grid[_targetBlock._pos.x + 2, _targetBlock._pos.y + 2] = _tempBlock;

            _grid[_selectBlock._pos.x + 2, _selectBlock._pos.y + 2] = _selectBlock;
            _grid[_targetBlock._pos.x + 2, _targetBlock._pos.y + 2] = _targetBlock;



        }
        else
        {
            // reverse Block
            _selectBlock.SetOrigin();
        }

        _selectBlock.gameObject.layer = LayerMask.NameToLayer("WaitBlock");
        _targetBlock.gameObject.layer = LayerMask.NameToLayer("WaitBlock");


        _selectBlock = null;
        _targetBlock = null;

    }



    public void SpawnArmy(int _count, int _blocktype)
    {





    }

    public void SpawnEnemy(int _count, int _blocktype)
    {




    }


    [Button]
    public void SpawnBlock()
    {
        if (_blockGroup == null) _blockGroup = new GameObject().transform;
        _blockGroup.name = "BlockGroup";
        _blockGroup.SetParent(transform);


        //RemoveAllBlock();



        SortGrid();

        StartCoroutine(Cor_Spawn());
        IEnumerator Cor_Spawn()
        {
            yield return null;
            //yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < _size.x; i++)
            {
                for (int j = 0; j < _size.y; j++)
                {
                    if (_grid[i, j] == false)
                    {
                        Block _block = Managers.Pool.Pop(_block_Pref, transform).GetComponent<Block>();
                        // Instantiate(_block_Pref, transform).GetComponent<Block>();
                        _block.transform.SetParent(_blockGroup);
                        //_block.transform.name = $"({i},{j})";
                        //_block ._typeNum = Random.Range(0, _maxColorCount);
                        _block._blockType = (Block.BlockType)Random.Range(0, 4);
                        _block.SetType();
                        //  Random.Range(0, System.Enum.GetValues(typeof(Block.BlockType)).Length);
                        _block.transform.localPosition = new Vector3(i - 2 + _posInterval.x, 0, j - 2 + _posInterval.y - 15);
                        _block.SetPos(i, j);
                        _block.isConnect = false;
                        _grid[i, j] = _block;
                        _block.gameObject.layer = LayerMask.NameToLayer("WaitBlock");
                    }
                }
            }

            yield return new WaitForSeconds(_blockMoveSpeed + 0.2f);
            CheckBlock();

        }

    }

    public void SortGrid()
    {
        for (int k = _size.y - 1; k >= 0; k--)
        {
            for (int j = _size.y - 1; j > 0; j--)
            {
                for (int i = 0; i < _size.x; i++)
                {
                    if (_grid[i, j] == null)
                    {
                        _grid[i, j] = _grid[i, j - 1];
                        if (_grid[i, j] != null) // 여러개가 없어졋을 경우 한칸만 내려가는 ?
                            _grid[i, j].SetPos(i, j);
                        _grid[i, j - 1] = null;

                    }
                }
            }
        }


        //for (int k = 0; k < _size.y - 1; k++)
        //{
        //    for (int j = 0; j < _size.y - 1; j++)
        //    {
        //        for (int i = 0; i < _size.x; i++)
        //        {
        //            if (_grid[i, j] == null)
        //            {
        //                _grid[i, j] = _grid[i, j + 1];
        //                if (_grid[i, j] != null)
        //                    _grid[i, j].SetPos(i, j);
        //                _grid[i, j + 1] = null;

        //            }
        //        }
        //    }
        //}

    }



    public void RemoveAllBlock()
    {
        int _count = _blockGroup.childCount;
        for (int i = 0; i < _count; i++)
        {

            Managers.Pool.Push(_blockGroup.GetChild(0).GetComponent<Poolable>());

        }
    }



    public bool CheckForMatches(int x, int y)
    {
        // 현재 위치의 타일을 가져옴
        Block currentTile = _grid[x, y];

        // 가로로 매치 확인
        if (CheckHorizontalMatches(x, y, currentTile))
        {
            // 매치가 있음
            return true;
        }

        // 세로로 매치 확인
        if (CheckVerticalMatches(x, y, currentTile))
        {
            // 매치가 있음
            return true;
        }

        // 매치가 없음
        return false;
    }

    // 가로로 매치 확인하는 함수
    private bool CheckHorizontalMatches(int x, int y, Block currentTile)
    {
        int matchCount = 1; // 현재 타일을 포함한 매치 개수

        // 왼쪽으로 이동하면서 매치 확인
        for (int i = x - 1; i >= 0; i--)
        {
            Block tile = _grid[i, y];
            if (tile.CompareTag(currentTile.tag))
            {
                matchCount++;
            }
            else
            {
                break; // 매치가 끊겼으므로 종료
            }
        }

        // 오른쪽으로 이동하면서 매치 확인
        for (int i = x + 1; i < _size.x; i++)
        {
            Block tile = _grid[i, y];
            if (tile.CompareTag(currentTile.tag))
            {
                matchCount++;
            }
            else
            {
                break; // 매치가 끊겼으므로 종료
            }
        }

        return matchCount >= 3; // 3 이상의 매치가 있으면 true 반환
    }

    // 세로로 매치 확인하는 함수
    private bool CheckVerticalMatches(int x, int y, Block currentTile)
    {
        int matchCount = 1; // 현재 타일을 포함한 매치 개수

        // 아래로 이동하면서 매치 확인
        for (int i = y - 1; i >= 0; i--)
        {
            Block tile = _grid[x, i];
            if (tile.CompareTag(currentTile.tag))
            {
                matchCount++;
            }
            else
            {
                break; // 매치가 끊겼으므로 종료
            }
        }

        // 위로 이동하면서 매치 확인
        for (int i = y + 1; i < _size.y; i++)
        {
            Block tile = _grid[x, i];
            if (tile.CompareTag(currentTile.tag))
            {
                matchCount++;
            }
            else
            {
                break; // 매치가 끊겼으므로 종료
            }
        }

        return matchCount >= 3; // 3 이상의 매치가 있으면 true 반환
    }






}
