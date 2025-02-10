using Obvious.Soap;
using UnityEngine;

public class Slash : MonoBehaviour
{
    [SerializeField] private FloatReference _lifeTime;

    //snapshot the stats
    private float _damage;
    private float _penetration;
    private bool _hasHit = false;

    public void Init(float damage, float penetration,Vector3 direction)
    {
        _damage = damage;
        _penetration = penetration;
        transform.forward = direction;
        Invoke(nameof(Destroy), _lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hasHit) return;
    
        _hasHit = true;
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && other.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.TakeDamage(_damage, _penetration);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Player") && other.TryGetComponent<Player>(out var player))
        {
            player.TakeDamage(_damage, _penetration);
        }
    }
    private void Destroy() => Destroy(gameObject);
}
