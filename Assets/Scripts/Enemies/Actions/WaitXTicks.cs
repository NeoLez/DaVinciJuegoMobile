using System;
using UnityEngine;

namespace Enemies.Actions {
    [Serializable]
    public class WaitXTicks : IEnemyAction {
        [SerializeField] public int AmountOfTicks;
        private int _ticks = 0;
        public bool Execute(GameObject enemy) {
            _ticks++;
            if (_ticks <= AmountOfTicks) {
                return false;
            }

            _ticks = 0;
            return true;
        }
    }
}