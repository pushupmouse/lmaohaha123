using MEC;
using Obvious.Soap;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private FloatReference _speed;
    [SerializeField] private FloatReference _lifeTime;

    //snapshot the stats
    private float _damage;
    private float _penetration;
    private bool _hasHit = false;
    
    private void Start()
    {
        Timing.RunCoroutine(Utility.EmulateUpdate(MyUpdate, this).CancelWith(gameObject));
    }

    public void Init(float damage, float penetration,Vector3 direction)
    {
        _damage = damage;
        _penetration = penetration;
        transform.forward = direction;
        Invoke(nameof(Destroy), _lifeTime);
    }
    
    private void MyUpdate()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hasHit) return;
        
        _hasHit = true;
        var enemy = other.GetComponent<Enemy>();
        enemy.TakeDamage(_damage, _penetration);
        Destroy();
    }
    
    private void Destroy() => Destroy(gameObject);
}
