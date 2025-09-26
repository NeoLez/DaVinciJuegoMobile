using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Enemies.Actions {
    [Serializable]
    public class ShootProjectile : IEnemyAction {
        public Projectile ProjectilePrefab;
        public float Speed;
        public int Damage;
        public float InitialOffset;
        
        public bool Execute(GameObject enemy) {
            var projectile = Object.Instantiate(ProjectilePrefab);
            Vector2 direction = (BattleManager.Instance.Player.gameObject.transform.position - enemy.transform.position).normalized;
            Vector2 spawnPos = (Vector2)enemy.transform.position + direction * InitialOffset;
            projectile.speed = direction * Speed;
            projectile.damage = Damage;
            projectile.transform.position = spawnPos;

            return true;
        }
    }
}