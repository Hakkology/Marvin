using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat {
    
    private int baseValue;
    public List<int> modifiers;
    
    public int GetValue(){

        int finalValue =  baseValue;

        foreach (int modifier in modifiers)
            finalValue += modifier;
        
        return finalValue;
    }
    public void AddModifier(int _modifier){
        modifiers.Add(_modifier);
    }
    public void RemoveModifier(int _modifier){
        modifiers.Remove(_modifier);
    }
}