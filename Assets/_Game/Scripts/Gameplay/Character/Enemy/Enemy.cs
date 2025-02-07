using System;
using Obvious.Soap;
using UnityEngine;
using Yade.Runtime;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyType _type;
    [SerializeField] private YadeSheetData _initEnemyStatsData;
    [SerializeField] private ScriptableListEnemy _scriptableListEnemy;
    [SerializeField] private FloatVariable _defCoefficient;
    [field: SerializeField] public HealthBar HealthBar { get; set; }
    private float _attack;
    private float _attackSpeed;
    private float _critRate;
    private float _critMult;
    private float _amp;
    private float _penetration;
    private float _maxHealth;
    private float _defense;
    private float _range;
    
    private FloatVariable _runtimeMaxHealth;
    private FloatVariable _runtimeCurrentHealth;
        
    private class EnemyStat
    {
        [DataField(1)] public string Attack;
        [DataField(2)] public string MaxHealth;
        [DataField(3)] public string Defense;
        [DataField(5)] public string AttackSpeed;
        [DataField(6)] public string Range;
        [DataField(7)] public string CritRate;
        [DataField(8)] public string CritMult;
        [DataField(9)] public string Amp;
        [DataField(10)] public string Penetration;
    }
    
    private void OnEnable()
    {
        Init();
    }

    private void OnDisable()
    {
        _runtimeMaxHealth.OnValueChanged -= OnFinalMaxHealthChanged;
        _runtimeCurrentHealth.OnValueChanged -= OnCurrentHealthChanged;
    }

    public void Init()
    {
        var list = _initEnemyStatsData.AsList<EnemyStat>();
        
        int index = (int)_type;
        
        _attack = float.Parse(list[index].Attack);
        _attackSpeed = float.Parse(list[index].AttackSpeed);
        _critRate = float.Parse(list[index].CritRate);
        _critMult = float.Parse(list[index].CritMult);
        _amp = float.Parse(list[index].Amp);
        _penetration = float.Parse(list[index].Penetration);
        _maxHealth = float.Parse(list[index].MaxHealth);
        _defense = float.Parse(list[index].Defense);
        _range = float.Parse(list[index].Range);
        
        if (_runtimeMaxHealth == null)
            _runtimeMaxHealth = SoapUtils.CreateRuntimeInstance<FloatVariable>($"float_{gameObject.name}MaxHealth");
        
        
        if (_runtimeCurrentHealth == null)
            _runtimeCurrentHealth = SoapUtils.CreateRuntimeInstance<FloatVariable>($"float_{gameObject.name}CurrentHealth");
        
        _runtimeMaxHealth.OnValueChanged += OnFinalMaxHealthChanged;
        _runtimeCurrentHealth.OnValueChanged += OnCurrentHealthChanged;
        
        _runtimeMaxHealth.Value = _maxHealth;
        _runtimeCurrentHealth.Value = _runtimeMaxHealth;
        
        _scriptableListEnemy.Add(this);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            player.TakeDamage(GetDamage(), _penetration);
            Die();
        }
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

    public void OnFinalMaxHealthChanged(float newMaxHealth)
    {
        _runtimeCurrentHealth.MinMax = new Vector2(0, newMaxHealth);
        var diff = newMaxHealth - _runtimeMaxHealth.PreviousValue;
        _runtimeCurrentHealth.Add(diff);
    }

    public void OnCurrentHealthChanged(float newHealthValue)
    {
        HealthBar.UpdateHealthBar(_runtimeCurrentHealth, _runtimeMaxHealth);
        
        if (_runtimeCurrentHealth.Value <= 0)
            Die();
    }

    public void Die()
    {
        _scriptableListEnemy.Remove(this);
        // Destroy(gameObject);
        ObjectPool.Instance.ReturnObjectToPool(gameObject);
    }
    
    public void TakeDamage(float damage, float penetration)
    {
        if (penetration >= 1)
        {
            _runtimeCurrentHealth.Add(-damage);
        }
        else
        {
            var effectiveDefense = _defense - (_defense * penetration) ;

            var damageMultiplier = 1 - (effectiveDefense / (effectiveDefense + _defCoefficient));

            var finalDamage = damage * damageMultiplier;

            _runtimeCurrentHealth.Add(-finalDamage);
        }
    }
}