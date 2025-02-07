using System.Collections.Generic;
using UnityEngine;
////MAYBE USEFUL LATER ON???
//[CreateAssetMenu(menuName = "StatGroupData/StatGroup")]
public class StatGroupData : ScriptableObject
{
    // [SerializeField] private StatGroup[] _stats;
    //
    // [System.Serializable]
    // public class StatGroup
    // {
    //     public StatType statType;
    //     public FinalStatData statData;
    // }

    // private Dictionary<StatType, FinalStatData> _statDictionary;
    //
    // private void InitializeDictionary()
    // {
    //     if (_statDictionary == null)
    //     {
    //         _statDictionary = new Dictionary<StatType, FinalStatData>();
    //         foreach (var statGroup in _stats)
    //         {
    //             if (!_statDictionary.ContainsKey(statGroup.statType))
    //             {
    //                 _statDictionary.Add(statGroup.statType, statGroup.statData);
    //             }
    //         }
    //     }
    // }
    //
    // public FinalStatData GetStatData(StatType type)
    // {
    //     InitializeDictionary();
    //
    //     if (_statDictionary.TryGetValue(type, out var statData))
    //     {
    //         return statData;
    //     }
    //
    //     return null;
    // }
}

    

