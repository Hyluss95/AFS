namespace AFSInterview
{
    using System;
    using UnityEngine;

    [Serializable]
    public class TowerTypeHolder
    {

        [SerializeField]
        TowerType type;

        [SerializeField]
        SimpleTower towerPrefab;

        public TowerType Type => type;

        public SimpleTower TowerPrefab => towerPrefab;
    }

}