using System;
using System.Collections;
using System.Collections.Generic;
using AFSInterview;
using UnityEngine;


[Serializable]
public class TowerTypeHolder
{

    [SerializeField] TowerType type;
    [SerializeField] SimpleTower towerPrefab;

    public TowerType Type => type;

    public SimpleTower TowerPrefab => towerPrefab;
}
