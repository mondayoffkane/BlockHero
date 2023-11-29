using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using DG.Tweening;




public class PuzzleManager : MonoBehaviour
{

    static public PuzzleManager _instance;
    public StageData _currentStage;

    //public Block.BlockType _blockType;

    public enum PuzzleState
    {
        BlockSpawn,
        BlockCheck,
        WaitInput,
        Match,
        ArmySpawn,
        Fight,
        Clear,
        Fail,
        Lobby

    }
    public PuzzleState _puzzleState;


    public Block _selectBlock;
    public Block _targetBlock;


    // =====================
    Camera _mainCam;
    public GameObject _lobby_Cam;
    public GameObject _puzzle_Cam;
    public GameObject _fight_Cam;

    // =================


    //==== Puzzle
    public int _stageLevel = 0;
    [TabGroup("Puzzle")] public int _changeCount = 10;
    [TabGroup("Puzzle")] public GameObject _block_Pref;
    [TabGroup("Puzzle")] public Transform _blockGroup;
    [TabGroup("Puzzle")] public Vector2Int _size = new Vector2Int(6, 6);
    [TabGroup("Puzzle")] public int _maxColorCount = 4;
    [TabGroup("Puzzle")] public Vector2 _posInterval = new Vector2(0f, -0.15f);
    [ShowInInspector] public Block[,] _grid;

    [TabGroup("Puzzle")] public float _blockMoveSpeed = 0.5f;
    [TabGroup("Puzzle")] public float _camz = 10f;
    [TabGroup("Puzzle")] public GameObject _gridObj;

    // ======== Hero Type
    //[TabGroup("Hero")] public Mesh[] _Hero_0_Meshes;
    //[TabGroup("Hero")] public Mesh[] _Hero_1_Meshes;
    //[TabGroup("Hero")] public Mesh[] _Hero_2_Meshes;
    //[TabGroup("Hero")] public Mesh[] _Hero_3_Meshes;

    [TabGroup("Hero")] public HeroStatus[] _selectHeroes = new HeroStatus[4];





    //[TabGroup("Hero")][ShowInInspector] public List<Mesh[]> _heroAllMeshes = new List<Mesh[]>();


    // ======= Battle

    [TabGroup("Battle")] public List<Hero> _heroList = new List<Hero>();
    [TabGroup("Battle")] public List<Enemy> _enemyList = new List<Enemy>();


    // Same Block Check ===============

    [TabGroup("Direction")] public int _jumpCount = 2;
    [TabGroup("Direction")] public float _jumpPower = 5f;
    [TabGroup("Direction")] public float _jumpTime = 0.5f;


    // ====== floating

    [TabGroup("Direction")] public float _floatingY = 2f;
    [TabGroup("Direction")] public float _floatingTime = 1f;
    [TabGroup("Direction")] public Vector3 _floatingPos = new Vector3(0f, 0.5f, 5.5f);

    // ====== Private ==========================================
    Vector3 _startMousePos;
    Vector3 _endMousePos;
    float _testDis;
    float _mouseDis = 100;


    bool isMove = false;
    Vector3 _selectStartPos;
    float _moveDistance = 1f;
    Vector3 _dir;
    float _dis;
    int _comboCount = 0;


    GameObject _floating_Pref;
    UiEffectManager _uiEffecter;
    // =========================================================
    //public void SetBlockMeshes()
    //{
    //_heroAllMeshes.Add(_selectHeroes[0]);
    //_heroAllMeshes.Add(_Hero_1_Meshes);
    //_heroAllMeshes.Add(_Hero_2_Meshes);
    //_heroAllMeshes.Add(_Hero_3_Meshes);

    //}


    private void Awake()
    {
        _instance = this;
        if (_uiEffecter == null) _uiEffecter = GetComponent<UiEffectManager>();

        _stageLevel = ES3.Load<int>("StageLevel", 0);
    }


    void Start()
    {
        _mainCam = Camera.main;
        _grid = new Block[_size.x, _size.y];
        //SetBlockMeshes();

        _puzzleState = PuzzleState.Lobby;

        Managers._gameUI.ChangePanel(0);


        //LoadStage(); // will modify, when press the  start button

    }

    public void StartStage()
    {
        CamChange(1);
        //this.TaskDelay(1f, () =>
        //{
        InitStage();
        LoadStage();
        //});

    }



    public void LoadStage()
    {

        // load stage level

        Managers._gameUI.ChangePanel(1);
        _gridObj.transform.DOLocalMoveY(0f, 0.5f).SetEase(Ease.Linear);
        //InitStage();

        this.TaskDelay(1f, (() =>
        {
            Managers._gameUI.MoveCountText.text = $"{_changeCount}";


            SpawnBlock();
        }));



    }
    public void InitStage()
    {
        for (int i = 0; i < _size.x; i++)
        {
            for (int j = 0; j < _size.y; j++)
            {
                if (_grid[i, j] != null)
                {
                    Managers.Pool.Push(_grid[i, j].GetComponent<Poolable>());
                }
                _grid[i, j] = null;

            }
        }
        _grid.Initialize();

        //UnityEditor.EditorApplication.isPaused = true;

        _changeCount = 10;
        //CamChange(2);

        _puzzleState = PuzzleState.BlockSpawn;

        for (int i = 0; i < _heroList.Count; i++)
        {
            Managers.Pool.Push(_heroList[i].GetComponent<Poolable>());
        }
        _heroList.Clear();

        for (int i = 0; i < _enemyList.Count; i++)
        {
            Managers.Pool.Push(_enemyList[i].GetComponent<Poolable>());
        }
        _enemyList.Clear();





    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            _changeCount++;
            Managers._gameUI.MoveCountText.text = $"{_changeCount}";
            _puzzleState = PuzzleState.WaitInput;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _changeCount--;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //_changeCount = 0;
            //CheckBlock();
            _puzzleState = PuzzleState.ArmySpawn;

            CamChange(2);
            Managers._gameUI.ChangePanel(2);
            this.TaskDelay(1.5f, FIghtMode);

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            for (int i = 0; i < _size.x; i++)
            {
                for (int j = 0; j < _size.y; j++)
                {
                    _grid[i, j]._heroType = Hero.HeroType.Wizard;
                    _grid[i, j]._level = 1;


                }
            }
            _puzzleState = PuzzleState.ArmySpawn;

            CamChange(2);
            Managers._gameUI.ChangePanel(2);
            this.TaskDelay(1.5f, FIghtMode);

        }



        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartStage();
            //UnityEngine.SceneManagement.SceneManager.LoadScene(
            //    UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }



        if (Input.GetMouseButtonDown(0))
        {
            if (_puzzleState == PuzzleState.WaitInput && _changeCount > 0)
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

                        DOTween.Kill(_selectBlock.gameObject);
                        Vector3 _tempPos = _selectBlock.transform.position;
                        _tempPos.y = 0;
                        _selectBlock.transform.position = _tempPos;


                        _selectBlock.gameObject.layer = LayerMask.NameToLayer("SelectBlock");
                        //CheckBlock(_selectBlock);
                        _selectStartPos = _selectBlock.transform.position;
                        _startMousePos = Input.mousePosition;
                        _endMousePos = _startMousePos;
                    }
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

                _endMousePos = Input.mousePosition;
                _testDis = Vector3.Distance(_startMousePos, _endMousePos);
                if (Vector3.Distance(_startMousePos, _endMousePos) > _mouseDis)
                {
                    isMove = true;
                }
                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 temp = hit.point;
                    temp.y = 0;

                    //if (Vector3.Distance(_selectStartPos, temp) > 0.2f)
                    //{
                    //    isMove = true;
                    //}
                    if (isMove)
                    {

                        _selectBlock.transform.position = temp;

                        _dir = (temp - _selectStartPos).normalized;
                        _dis = Vector3.Distance(_selectStartPos, _selectBlock.transform.position) <= _moveDistance ? Vector3.Distance(_selectStartPos, _selectBlock.transform.position) : _moveDistance;

                        _selectBlock.transform.position = _selectStartPos + _dir * _dis;

                    }


                }
            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            _startMousePos = Vector3.zero;
            _endMousePos = _startMousePos;
            if (_selectBlock != null)
            {
                if (isMove == false)
                {
                    CheckSameType();
                }



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
                        DOTween.Kill(_targetBlock.gameObject);
                        Vector3 _tempPos = _targetBlock.transform.position;
                        _tempPos.y = 0;
                        _targetBlock.transform.position = _tempPos;
                        MoveBlock();
                    }
                }
                else if (_selectBlock != null)
                {
                    if (isMove)
                        _selectBlock.SetOrigin();


                    _selectBlock.gameObject.layer = LayerMask.NameToLayer("WaitBlock");



                    _selectBlock = null;
                }
                isMove = false;

                //Debug.Log("CheckBlock");
                CheckBlock();
            }
        }
    }


    [Button]
    public void CheckBlock() // check 3 match
    {
        //Debug.Log("Check Block");
        _puzzleState = PuzzleState.BlockCheck;
        bool ExistMatch = false;
        StartCoroutine(Cor_CheckBlock());

        IEnumerator Cor_CheckBlock()
        {
            yield return null;

            for (int j = 0; j < _size.y; j++) // horizontal check
            {
                for (int i = 0; i < _size.x - 2; i++)
                {
                    if ((_grid[i, j]._heroType == _grid[i + 1, j]._heroType)
                        && (_grid[i, j]._heroType == _grid[i + 2, j]._heroType)
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
            //Debug.Log("Horizontal check");
            //yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < _size.x; i++) // vertical check
            {
                for (int j = 0; j < _size.y - 2; j++)
                {
                    if ((_grid[i, j]._heroType == _grid[i, j + 1]._heroType)
                        && (_grid[i, j]._heroType == _grid[i, j + 2]._heroType)
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
            //Debug.Log("Vertical check");
            yield return new WaitForSeconds(_blockMoveSpeed);

            if (ExistMatch == true)
            {
                _comboCount++;
                if (_comboCount > 1)
                {
                    Floating($"Combo X{_comboCount}", _floatingPos);
                    _changeCount++;
                    Managers._gameUI.MoveCountText.text = $"{_changeCount}";
                }
                _puzzleState = PuzzleState.Match;

                for (int i = 0; i < _size.x; i++)
                {
                    for (int j = 0; j < _size.y; j++)
                    {
                        _grid[i, j].OnMatchBlock(); // block merge
                        // add match particle or block.cs in OnMatchBlock()
                    }
                }
                //Debug.Log("OnMatch");
                yield return new WaitForSeconds(0.5f  /*+ 0.25f*/);
                _puzzleState = PuzzleState.WaitInput;


                SpawnBlock();
            }
            else
            {

                // add combo floating
                _comboCount = 0;

                if (_changeCount < 1)
                {
                    _puzzleState = PuzzleState.ArmySpawn;
                    // add Spawn Army Func();
                    //FIghtMode();

                    CamChange(2);
                    Managers._gameUI.ChangePanel(2);
                    this.TaskDelay(1.5f, FIghtMode);

                }
                else
                {
                    _puzzleState = PuzzleState.WaitInput;

                }
            }


        }


    }




    public void MoveBlock() // check can move position
    {

        int _count = 0;

        _count = Mathf.Abs(_selectBlock._pos.x - _targetBlock._pos.x);
        _count += Mathf.Abs(_selectBlock._pos.y - _targetBlock._pos.y);

        if (_count <= 1 && isMove)
        {
            _changeCount--;
            Managers._gameUI.MoveCountText.text = $"{_changeCount}";
            // Swap Block
            Vector2Int _tempPos = _selectBlock._pos;
            _selectBlock.SetPos(_targetBlock._pos.x + 2, _targetBlock._pos.y + 2);
            _targetBlock.SetPos(_tempPos.x + 2, _tempPos.y + 2);


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




    [Button]
    public void SpawnBlock()
    {
        //Debug.Log("Spawn Block");
        _puzzleState = PuzzleState.BlockSpawn;

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
                    if (_grid[i, j] == null)
                    {
                        Block _block = Managers.Pool.Pop(_block_Pref, transform).GetComponent<Block>();
                        _block.transform.SetParent(_blockGroup);
                        _block.name = $"({i},{j})";

                        int _num = Random.Range(0, 4);
                        //int _num = Random.Range(0, _selectHeroes.Length);

                        _block._level = 0;
                        _block.SetType(_selectHeroes[_num]);
                        _block.transform.rotation = Quaternion.Euler(new Vector3(0f, /*180f*/ 0f, 0f));
                        _block.Init(true);
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
        //Debug.Log("Sort Block");
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


    public void Floating(string _str, Vector3 _pos)
    {
        if (_floating_Pref == null) _floating_Pref = Resources.Load<GameObject>("FloatingText");


        GameObject _floating = Managers.Pool.Pop(_floating_Pref, transform).gameObject;
        _floating.transform.GetChild(0).GetComponent<Text>().text = _str;
        _floating.transform.position = _pos;
        _floating.transform.DOMoveY(transform.position.y + _floatingY, _floatingTime).SetEase(Ease.Linear)
            .OnComplete(() => Managers.Pool.Push(_floating.transform.GetComponent<Poolable>()));


    }

    public void CheckSameType()
    {
        for (int i = 0; i < _size.x; i++)
        {
            for (int j = 0; j < _size.y; j++)
            {
                if (_grid[i, j]._heroType == _selectBlock._heroType
                    && _grid[i, j]._level == _selectBlock._level)
                {
                    Vector3 _tempPos = _grid[i, j].transform.position;
                    _tempPos.y = 0f;
                    _grid[i, j].transform.DOJump(_tempPos, _jumpPower, _jumpCount, _jumpTime).SetEase(Ease.Linear);
                }


            }
        }

    }


    public void CamChange(int _num)
    {
        _lobby_Cam.SetActive(false);
        _puzzle_Cam.SetActive(false);
        _fight_Cam.SetActive(false);

        switch (_num)
        {
            case 0:
                _lobby_Cam.SetActive(true);
                break;

            case 1:
                _puzzle_Cam.SetActive(true);
                break;

            case 2:
                _fight_Cam.SetActive(true);
                break;


        }
    }

    public void FIghtMode()
    {

        //SpawnEnemy();
        //_gridObj.transform.DOLocalMoveY(-1f, 0.5f).SetEase(Ease.Linear);

        //for (int i = 0; i < _size.x; i++)
        //{
        //    for (int j = 0; j < _size.y; j++)
        //    {
        //        _grid[i, j].SpawnHero();
        //    }
        //}
        //SpawnEnemy();
        //_puzzleState = PuzzleState.Fight;
        // //////////
        DOTween.Sequence().
            AppendCallback(() =>
            {
                _gridObj.transform.DOLocalMoveY(-1f, 0.5f).SetEase(Ease.Linear);

                for (int i = 0; i < _size.x; i++)
                {
                    for (int j = 0; j < _size.y; j++)
                    {
                        _grid[i, j].SpawnHero();
                    }
                }
                SpawnEnemy();
            })
            .AppendInterval(1.5f)
            .AppendCallback(() =>
            {

                _puzzleState = PuzzleState.Fight;
            })
            ;

    }


    public void SpawnEnemy()
    {

        _enemyList = new List<Enemy>();

        for (int i = 0; i < _currentStage._maxEnemyCount; i++) // Chagne -  _stageData. monster count
        {
            Enemy.EnemyType _enemyType = _currentStage._enemyStatusList[Random.Range(0, _currentStage._enemyStatusList.Length)]._enemyType;


            GameObject _enemyPref;
            string _typeString = _enemyType.ToString();

            _enemyPref = Resources.Load<GameObject>($"Enemy_Prefs/{_typeString}_Pref");
            System.Type _enemyClassType = System.Type.GetType(_typeString);
            Enemy _newEnemy = (Enemy)Managers.Pool.Pop(_enemyPref).GetComponent(_enemyClassType);

            //_newEnemy.transform.position = transform.position;

            EnemyStatus _enemyStatus = Resources.Load<EnemyStatus>($"EnemyStatus/{_enemyType.ToString()}");

            _enemyList.Add(_newEnemy);
            _newEnemy.InitStatus(_enemyStatus, Random.Range(_currentStage._enemyLevelRange.x, _currentStage._enemyLevelRange.y));

            _newEnemy.transform.position = new Vector3(0f + Random.Range(-3f, 3f), 0f, 11f + Random.Range(-3f, 3f));


        }

        EnemyCastle _enemyCastle;

        _enemyCastle = Managers.Pool.Pop(_currentStage._enemyCastlePref).GetComponent<EnemyCastle>();
        _enemyCastle.transform.position = new Vector3(0f, 0.5f, 12f);
        _enemyList.Add(_enemyCastle);

        _enemyCastle.InitStatus(Resources.Load<EnemyStatus>($"EnemyStatus/EnemyCastle"), 0);





    }

    public void DeadArnmyNEnemy(bool isHero = true)
    {
        if (isHero)
        {
            if (_heroList.Count < 1) // Fail Stage
            {

                this.TaskDelay(1f, () =>
                {
                    _puzzleState = PuzzleState.Fail;
                    Managers._gameUI.Fail_Stage_Text.text = $"Stage {_stageLevel + 1}";
                    _uiEffecter.FailEffect();
                    this.TaskDelay(1.5f, () => Managers._gameUI.Defeat_Panel.SetActive(true));

                });



                //Managers._gameUI.Fail_Panel.SetActive(true);

                //Debug.Log("Fail Stage");
            }
        }
        else
        {

            if (_enemyList.Count < 1) // Clear Stage
            {
                this.TaskDelay(3f, () =>
                {
                    _puzzleState = PuzzleState.Clear;
                    _stageLevel++;
                    ES3.Save<int>("StageLevel", _stageLevel);
                    Managers._gameUI.Clear_Stage_Text.text = $"Stage {_stageLevel + 1}";
                    _uiEffecter.ClearEffect();
                    this.TaskDelay(1.5f, () => Managers._gameUI.Clear_Panel.SetActive(true));

                });


                //Managers._gameUI.Clear_Panel.SetActive(true);
                //Debug.Log("Clear Stage");
            }
        }


    }


}
