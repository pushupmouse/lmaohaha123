using System;
using MEC;
using Obvious.Soap;
using UnityEngine;
using Yade.Runtime;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private EnemyType _type;
    [SerializeField] private YadeSheetData _initEnemyStatsData;
    [SerializeField] private Vector3Variable _playerPosition;

    private float _speed;
    
    private class EnemyStat
    {
        [DataField(9)] public string Speed;
    }

    private void OnEnable()
    {
        Init();
        Timing.RunCoroutine(Utility.EmulateUpdate(MyUpdate, this).CancelWith(gameObject));
    }

    private void MyUpdate()
    {
        var direction = (_playerPosition.Value - transform.position).normalized;
        
        transform.position += direction * (_speed * Time.deltaTime);
    }

    private void Init()
    {
        var list = _initEnemyStatsData.AsList<EnemyStat>();
        
        int index = (int)_type;
        
        _speed = float.Parse(list[index].Speed);
    }
}
