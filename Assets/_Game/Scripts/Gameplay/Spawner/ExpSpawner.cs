using UnityEngine;

public class ExpSpawner : MonoBehaviour
{
    [SerializeField] private ScriptableListEnemy _scriptableListEnemy;
    [SerializeField] private FloatVariablePickup _expPickupPrefab;
    
    private void Awake()
    {
        _scriptableListEnemy.OnItemRemoved += OnEnemyDied;
    }

    private void OnDestroy()
    {
        _scriptableListEnemy.OnItemRemoved -= OnEnemyDied;
    }
    
    private void OnEnemyDied(Enemy obj)
    {
        Instantiate(_expPickupPrefab.gameObject, obj.transform.position, Quaternion.identity);
    }
}
