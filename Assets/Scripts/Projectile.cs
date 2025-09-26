using UnityEngine;

public class Projectile : MonoBehaviour, IHittable{
    public Vector2 speed;
    public float projectileRadius;
    public int damage;

    private void Update() {
        transform.position = (Vector2)transform.position + speed * Time.deltaTime;
        var colliders = Physics2D.OverlapCircleAll(transform.position, projectileRadius);
        foreach (var collider in colliders) {
            if (collider.TryGetComponent(out Health health)) {
                health.ReceiveDamage(damage);
                Destroy(gameObject);
            }else if (collider.TryGetComponent(out Shield shield)) {
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, projectileRadius);
    }

    public void Hit(int damage) {
        speed = -speed;
    }
}