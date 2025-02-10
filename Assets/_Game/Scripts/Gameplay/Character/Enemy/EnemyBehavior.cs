using System;
using MEC;
using Obvious.Soap;
using UnityEngine;
using Yade.Runtime;
using Random = UnityEngine.Random;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private EnemyType _type;
    [SerializeField] private AttackType _attackType;
    [SerializeField] private YadeSheetData _initEnemyStatsData;
    [SerializeField] private YadeSheetData _enemyGrowthData;
    [SerializeField] private IntVariable _currentRoundPhase;
    [SerializeField] private Vector3Variable _playerPosition;
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Slash _slashPrefab;
    [SerializeField] private FloatReference _spawnProjectileOffset;
    [SerializeField] private FloatReference _spawnSlashOffset;

    private StateType _stateType;
    private float _attackTimer = 0;
    private bool _hasInstantAttack = true;
    private float _instantAttackTimer = 0;
    private float _attack;
    private float _critRate;
    private float _critMult;
    private float _amp;
    private float _penetration;
    private float _speed;
    private float _attackSpeed;
    private float _range;
    private float _attackMult;

    
    private class EnemyStat
    {
        [DataField(1)] public string Attack;
        [DataField(4)] public string Speed;
        [DataField(5)] public string AttackSpeed;
        [DataField(6)] public string Range;
        [DataField(7)] public string CritRate;
        [DataField(8)] public string CritMult;
        [DataField(9)] public string Amp;
        [DataField(10)] public string Penetration;
    }
    
    private class EnemyGrowth
    {
        [DataField(1)] public string Attack;
    }

    private void Start()
    {
        Init();
        Timing.RunCoroutine(Utility.EmulateUpdate(MyUpdate, this).CancelWith(gameObject));
    }
        
    private void Init()
    {
        int index = (int)_type;

        var growthList = _enemyGrowthData.AsList<EnemyGrowth>();

        _attackMult = float.Parse(growthList[index].Attack);
        
        var statList = _initEnemyStatsData.AsList<EnemyStat>();

        _attack = float.Parse(statList[index].Attack) * (1 + _attackMult * _currentRoundPhase);
        _critRate = float.Parse(statList[index].CritRate);
        _critMult = float.Parse(statList[index].CritMult);
        _amp = float.Parse(statList[index].Amp);
        _penetration = float.Parse(statList[index].Penetration);
        _attackSpeed = float.Parse(statList[index].AttackSpeed);
        _speed = float.Parse(statList[index].Speed);
        _range = float.Parse(statList[index].Range);
        
        SwitchState(StateType.Chase);
    }
    
    private void MyUpdate()
    {
        float distanceToTarget = Vector3.Distance(_playerPosition.Value, transform.position);

        if (distanceToTarget > _range && _stateType != StateType.Chase)
        {
            SwitchState(StateType.Chase);
        }
        else if (distanceToTarget <= _range && _stateType != StateType.Attack)
        {
            SwitchState(StateType.Attack);
        }

        switch (_stateType)
        {
            case StateType.Chase:
                Chase();
                break;
            case StateType.Attack:
                PrepareAttack();
                break;
            default:
                Debug.LogWarning("Something went wrong"); 
                break;
        }
    }

    private void Chase()
    {
        _instantAttackTimer += Time.deltaTime;

        //this attack is ready twice slower than regular attack
        if (!_hasInstantAttack && _instantAttackTimer >= 1f / (_attackSpeed * 2))
        {
            _hasInstantAttack = true;
            _instantAttackTimer = 0f;
        }
        
        var direction = (_playerPosition.Value - transform.position).normalized;
        
        transform.position += direction * (_speed * Time.deltaTime);
    }
    
    private void PrepareAttack()
    {
        if (_hasInstantAttack)
        {
            PerformAttack();
            
            _hasInstantAttack = false;
        }
        else
        {
            _attackTimer += Time.deltaTime;

            if (_attackTimer < 1f / _attackSpeed) return;

            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        var directionNormalized = (_playerPosition - transform.position).normalized;

        if (_attackType == AttackType.Slash)
        {
            var spawnPoint = transform.position + directionNormalized * _spawnSlashOffset;
        
            spawnPoint.y = transform.position.y;
            
            var slash = Instantiate(_slashPrefab, spawnPoint, Quaternion.LookRotation(directionNormalized));
        
            slash.Init(GetDamage(), _penetration, directionNormalized);
            
        }
        else if (_attackType == AttackType.Projectile)
        {
            var spawnPoint = transform.position + directionNormalized * _spawnProjectileOffset;
        
            spawnPoint.y = transform.position.y;
            
            var projectile = Instantiate(_projectilePrefab, spawnPoint, Quaternion.identity);
        
            projectile.Init(GetDamage(), _penetration, directionNormalized);
        }
        _attackTimer = 0f;
    }

    private float GetDamage()
    {
        var damage = _attack * (1 + _amp);
        
        bool isCriticalHit = Random.value < _critRate;

        if (isCriticalHit)
        {
            damage *= _critMult;
        }
        
        return damage;
    }
    
    private void SwitchState(StateType newState)
    {
        _stateType = newState;
    }
    
    private enum StateType
    {
        Chase,
        Attack,
    }

    private enum AttackType
    {
        Slash,
        Projectile,
    }
}
