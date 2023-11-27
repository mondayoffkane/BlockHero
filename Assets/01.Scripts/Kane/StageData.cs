
using UnityEngine;

[CreateAssetMenu(fileName = "StageData_", menuName = "ScriptableObj/StageData", order = int.MaxValue)]
public class StageData : ScriptableObject
{

    public int _stageLevel;
    public Vector2Int _enemyLevelRange = new Vector2Int(0, 1);
    public int _maxEnemyCount;
    //public GameObject _monsterPref;
    public GameObject _enemyCastlePref;
    //public GameObject _bossPref;

    public EnemyStatus[] _enemyStatusList;





}
