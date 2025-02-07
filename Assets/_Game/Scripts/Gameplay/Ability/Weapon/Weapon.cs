using MEC;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.Serialization;

public class Weapon : MonoBehaviour
{
    [SerializeField] private ScriptableListEnemy _scriptableListEnemy;
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private FloatVariable _spawnOffset;
    [Header("Weapon Stats")]
    [SerializeField] private FloatVariable _playerAttack;
    [SerializeField] private FloatVariable _playerAttackSpeed;
    [SerializeField] private FloatVariable _playerCritRate;
    [SerializeField] private FloatVariable _playerCritMult;
    [SerializeField] private FloatVariable _playerAmp;
    [SerializeField] private FloatVariable _playerPenetration;
    [SerializeField] private FloatVariable _playerMaxRange;
    
    private Transform _ownerTransform;
    private float _timer;
    
    private void Awake()
    {
        _ownerTransform = transform.parent == null ? transform : transform.parent;
    }

    private void Start()
    {
        Timing.RunCoroutine(Utility.EmulateUpdate(MyUpdate, this).CancelWith(gameObject));
    }

    private void MyUpdate()
    {
        _timer += Time.deltaTime;
        if (_timer < 1f / _playerAttackSpeed) return;
        ShootAtClosestEnemy();
        _timer = 0;
    }

    private void ShootAtClosestEnemy()
    {
        var closestEnemy = _scriptableListEnemy.GetClosest(transform.position,_playerMaxRange);

        if (closestEnemy == null) return;
        var direction = closestEnemy.transform.position - _ownerTransform.position;
        SpawnProjectile(direction.normalized);
    }

    private void SpawnProjectile(Vector3 directionNormalized)
    {
        var spawnPoint = _ownerTransform.position + directionNormalized * _spawnOffset;
        
        spawnPoint.y = _ownerTransform.position.y;
        
        var projectile = Instantiate(_projectilePrefab, spawnPoint, Quaternion.identity);
        
        projectile.Init(GetFinalDamage(), _playerPenetration, directionNormalized);
    }

    private float GetFinalDamage()
    {
        var baseDamage = _playerAttack.Value;
        
        var amplifiedDamage = baseDamage + baseDamage * _playerAmp.Value;
        
        bool isCriticalHit = Random.value < _playerCritRate.Value;

        var finalDamage = isCriticalHit ?
            amplifiedDamage + amplifiedDamage * _playerCritMult.Value :
            amplifiedDamage;
        
        return finalDamage;
    }
}
