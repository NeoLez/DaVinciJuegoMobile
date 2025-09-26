using UnityEngine;

[RequireComponent(typeof(Health), typeof(Enemy))]
public class RemoveEnemyFromBattleManagerOnDeath : MonoBehaviour {
    private void Awake() {
        GetComponent<Health>().OnDie += () => {
            BattleManager.Instance.enemies.Remove(gameObject.GetComponent<Enemy>());
        };
    }
}