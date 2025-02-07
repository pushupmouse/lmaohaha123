using UnityEngine;

public abstract class AbilityData : ScriptableObject
{
    protected string _description;
    
    public int ApplyCount {get; protected set;}

    public virtual void Apply()
    {
        ApplyCount++;
    }
    
    public abstract string GetDescription();

    public void Reset()
    {
        ApplyCount = 0;
    }
}
