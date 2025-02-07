using Obvious.Soap;
using UnityEngine;
using UnityEngine.Serialization;
using Yade.Runtime;

public class RoundTimer : MonoBehaviour
{
    [FormerlySerializedAs("_roundPhaseStats")] [SerializeField] private YadeSheetData _roundPhaseData;
    [SerializeField] private IntVariable _currentRoundPhase;
    private float _duration;
    private float _elapsedTime;
    private bool _isRunning;

    private class RoundPhase
    {
        [DataField(0)] public string Id;
        [DataField(1)] public string Duration;
    }
    
    private void Awake()
    {
        _currentRoundPhase.OnValueChanged += OnRoundPhaseChange;
        
        ResetPhase();

        StartTimer();
    }
    
    private void Update()
    {
        if (_isRunning)
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _duration)
            {
                EnterNextPhase();
            }
        }
    }

    private void OnDisable()
    {
        _currentRoundPhase.OnValueChanged -= OnRoundPhaseChange;
    }

    private void OnRoundPhaseChange(int obj)
    {
        var list = _roundPhaseData.AsList<RoundPhase>();
        
        _duration = float.Parse(list[_currentRoundPhase].Duration);
                
        _elapsedTime = 0f;
    }

    
    private void EnterNextPhase()
    {
        var list = _roundPhaseData.AsList<RoundPhase>();

        if (_currentRoundPhase.Value >= list.Count - 1)
        {
            StopTimer();
        }
        else
        {
            _currentRoundPhase.Add(1);
        }
    }

    public void StartTimer()
    {
        _elapsedTime = 0;
        
        _isRunning = true;
    }

    public void StopTimer()
    {
        _isRunning = false;
    }

    public void ResetPhase()
    {
        _currentRoundPhase.Value = 1;
    }
}
