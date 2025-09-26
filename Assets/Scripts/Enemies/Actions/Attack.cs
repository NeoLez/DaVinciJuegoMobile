using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Enemies.Actions {
    [Serializable]
    public class Attack : IEnemyAction {
        [SerializeField] private Vector2 attackSize;
        [SerializeField] private GameObject visualizerPrefab;
        [SerializeField] private int damage;
        public bool Execute(GameObject enemy) {
            Vector2 attackCenter = ((Vector2)BattleManager.Instance.Player.transform.position - (Vector2)enemy.transform.position).normalized * attackSize.x/2;
            float angle = Vector2.Angle(Vector2.right, attackCenter);

            GameObject visualizer = Object.Instantiate(visualizerPrefab, enemy.transform);
            visualizer.transform.position = (Vector2)enemy.transform.position + attackCenter;
            visualizer.transform.right = attackCenter;
            visualizer.transform.localScale = attackSize;
            
            var colliders = Physics2D.OverlapBoxAll(attackCenter + (Vector2)enemy.transform.position, attackSize, angle);
            colliders.OrderBy(collider2D => ((Vector2)collider2D.transform.position - (Vector2)enemy.transform.position).magnitude);
            
            foreach (var collider in colliders) {
                if(collider.gameObject == enemy) continue;
                
                if (collider.gameObject.TryGetComponent(out IHittable hittable)) {
                    hittable.Hit(damage);
                }
                if (collider.gameObject.TryGetComponent(out Shield _))
                    break;
            }

            return true;
        }
    }
}