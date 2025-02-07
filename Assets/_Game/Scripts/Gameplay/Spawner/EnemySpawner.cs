using System.Collections;
using MEC;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.Serialization;
using Yade.Runtime;
using Random = UnityEngine.Random;

public class EnemySpawner : Spawner
{
    [FormerlySerializedAs("_enemySpawnStats")] [SerializeField] private YadeSheetData _enemySpawnData;
    [SerializeField] private IntVariable _currentRoundPhase;
    [SerializeField] private Enemy _normalPrefab;
    [SerializeField] private Enemy _tankPrefab;
    [SerializeField] private Enemy _roguePrefab;
    [SerializeField] private Enemy _slayerPrefab;
    [SerializeField] private Enemy _sniperPrefab;
    [SerializeField] private Enemy _jokerPrefab;

    //FIX
    private Enemy[] _enemyPrefabs;
    
    private class EnemySpawn
    {
        [DataField(0)] public string Id;
        [DataField(1)] public string Delay;
        [DataField(2)] public string Interval;
        [DataField(3)] public string Amount;
    }
    
    protected override IEnumerator Start()
    {
        var list = _enemySpawnData.AsList<EnemySpawn>();
        
        _initialDelay = float.Parse(list[_currentRoundPhase].Delay);
        _spawnInterval = float.Parse(list[_currentRoundPhase].Interval);
        _amount = int.Parse(list[_currentRoundPhase].Amount);
        
        _currentRoundPhase.OnValueChanged += OnRoundPhaseChange;
        
        yield return base.Start();

        //FIX
        _enemyPrefabs = new Enemy[]
        {
            _normalPrefab,
            _tankPrefab,
            _roguePrefab,
            _slayerPrefab,
            _sniperPrefab,
            _jokerPrefab
        };
    }

    private void OnRoundPhaseChange(int obj)
    {
        var list = _enemySpawnData.AsList<EnemySpawn>();
        
        _spawnInterval = float.Parse(list[_currentRoundPhase].Interval);
        _amount = int.Parse(list[_currentRoundPhase].Amount);
    }


    protected override void Spawn()
    {
        base.Spawn();
        //FIX
        int randomIndex = Random.Range(0, _enemyPrefabs.Length);

        if (_enemyPrefabs[randomIndex] == null)
            return;

        Enemy enemyObj = ObjectPool.Instance.SpawnObject<Enemy>(
            _enemyPrefabs[randomIndex].gameObject,
            new Vector3(spawnPosition.x, 0f, spawnPosition.z),
            Quaternion.identity,
            PoolType.GameObject);
    }
}

public enum EnemyType
{
    Normal = 1,
    Tank = 2,
    Rogue = 3,
    Slayer = 4,
    Joker = 5,
    Sniper = 6,
}