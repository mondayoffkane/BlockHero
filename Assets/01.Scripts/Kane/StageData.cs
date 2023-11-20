
using UnityEngine;

[CreateAssetMenu(fileName = "StageData_", menuName = "ScriptableObj/StageData", order = int.MaxValue)]
public class StageData : ScriptableObject
{

    public int _stageLevel;
    public int _maxMonsterCount;
    public GameObject _monsterPref;
    public GameObject _bossPref;





}
