using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Custom/TowerTypeList")]
public class TowerTypeList : ScriptableObject
{
    [SerializeField] List<TowerTypeHolder> towerList;

    public IReadOnlyList<TowerTypeHolder> TowerList => towerList;
}

