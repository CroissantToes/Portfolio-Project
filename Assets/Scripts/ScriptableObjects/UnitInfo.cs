using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/UnitInfo")]
public class UnitInfo : ScriptableObject
{
    public Sprite portrait;
    public string className;

    public List<Sprite> abilityPortraits;
    public List<string> abilityNames;
    public List<string> abilityDescriptions;
}
