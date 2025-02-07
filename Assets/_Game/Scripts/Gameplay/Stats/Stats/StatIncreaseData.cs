using System;
using System.ComponentModel;
using Obvious.Soap;
using UnityEngine;
using System.Reflection;
using UnityEngine.Serialization;
using Yade.Runtime;

[CreateAssetMenu(menuName = "AbilityData/StatIncreaseData")]
public class StatIncreaseData : AbilityData
{
    [SerializeField] private YadeSheetData _statIncreaseData;
    [SerializeField] private FloatVariable _stat;
    [SerializeField] private StatType _statType;
    [SerializeField] private StatIncreaseType _statIncreaseType;
    private float _increase;
    private bool _isPercent;
    private RarityType _rarityType;
    
    private class StatIncrease
    {
        [DataField(0)] public string StatType;
        [DataField(1)] public string IncreaseType;
        [DataField(2)] public string IsPercent;
        [DataField(3)] public string IncreaseValue;
        [DataField(4)] public string RarityType;
    }
    
    [ContextMenu("Get Increase Value from Config")]
    public void Init()
    {
        if(_statIncreaseData == null)
            return;
        
        var list = _statIncreaseData.AsList<StatIncrease>();
        
        int index = list.FindIndex(item => 
            item.StatType.Equals(_statType.ToString()) &&
            item.IncreaseType.Equals(_statIncreaseType.ToString()));

        if (index == -1)
        {
            Debug.Log("No matching item found in the list." + _statType.GetEnumDescription() + _statIncreaseType.GetEnumDescription() 
                      + "/nPlease update config");
            return;
        }
        
        _increase = float.Parse(list[index].IncreaseValue);
        _isPercent = Convert.ToBoolean(list[index].IsPercent);
        _rarityType = (RarityType)Enum.Parse(typeof(RarityType), list[index].RarityType);
    }
    
    public override void Apply()
    {
        var increase = _increase;
        
        _stat.Value += increase;
        
        base.Apply();
    }
    
    public override string GetDescription()
    {
        _description = "+{0} " + _statIncreaseType.GetEnumDescription() + " " + _statType.GetEnumDescription();
        var formattedValue = $"{(_isPercent ? _increase * 100 : _increase)}{(_isPercent ? "%" : "")}";
        return string.Format(_description, formattedValue);
    }

    public RarityType GetRarityType()
    {
        return _rarityType;
    }
}

public enum StatIncreaseType
{
    [Description("Base")]
    Base,
    [Description("TOTAL Base")]
    Mult,
}

public enum StatType
{
    [Description("Attack")]
    Attack,
    [Description("Attack Speed")]
    AttackSpeed,
    [Description("Crit Rate")]
    CritRate,
    [Description("Crit Multiplier")]
    CritMult,
    [Description("Amp")]
    Amp,
    [Description("Penetration")]
    Penetration,
    [Description("Max Health")]
    MaxHealth,
    [Description("Defense")]
    Defense,
    [Description("Health Regen")]
    HealthRegen,
    [Description("Speed")]
    Speed,
    [Description("Max Range")]
    MaxRange
}

public enum RarityType
{
    Common,
    Rare,
    Epic,
    Legendary,
}

public static class StatExtensions
{
    public static string GetEnumDescription<TEnum>(this TEnum value) where TEnum : Enum
    {
        // Get the enum type based on the passed value
        Type enumType = value.GetType();

        // Get the field info for the enum value
        FieldInfo field = enumType.GetField(value.ToString());

        // Get the description attribute from the field
        var attribute = field?.GetCustomAttribute<DescriptionAttribute>();

        // Return the description if it exists, otherwise return the enum name
        return attribute?.Description ?? value.ToString();
    }
}