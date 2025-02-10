using System.Collections;
using System.Collections.Generic;
using MEC;
using Obvious.Soap;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] private Vector3Variable _playerPosition;
    [SerializeField] private Vector2 _spawnRange;
    protected float _initialDelay;
    protected float _spawnInterval;
    protected int _amount;

    private float _currentAngle;
    private float _timer;
    private bool _isActive;
    protected Vector3 spawnPosition;
    
    protected virtual IEnumerator Start()
    {
        yield return new WaitForSeconds(_initialDelay);
        _timer = 0f;
        _isActive = true;
    }
    
    private void Update()
    {
        if (!_isActive)
            return;
        _timer += Time.deltaTime;
        if (!(_timer >= _spawnInterval)) return;
        for (int i = 0; i < _amount; i++)
            Spawn();
        _timer = 0;
    }
    
    protected virtual void Spawn()
    {
        _currentAngle += 180f + Random.Range(-45, 43);
        var angleInRad = _currentAngle * Mathf.Deg2Rad;
        var range = Random.Range(_spawnRange.x, _spawnRange.y);
        var relativePosition = new Vector3
        (Mathf.Cos(angleInRad) * range,
            0f, 
            Mathf.Sin(angleInRad) * range);
        spawnPosition = _playerPosition.Value + relativePosition;
    }
}
