using System;
using UnityEngine;

public class Health : MonoBehaviour, IHittable {
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    public int GetHealth() => currentHealth;
    public event Action OnDie;

    public void ReceiveDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        if (currentHealth <= 0) {
            OnDie?.Invoke();
            Destroy(gameObject);
        }
    }

    public void Hit(int damage) {
        ReceiveDamage(damage);
    }
}