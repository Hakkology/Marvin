using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "Items/ItemEffect", order = 0)]
public class ItemEffect : ScriptableObject
{
    public virtual void ExecuteEffect(Transform transform){
        Debug.Log("Effect executed.");
    }
}
