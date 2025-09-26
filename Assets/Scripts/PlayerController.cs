using System;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour {
    [SerializeField] private Vector2 attackSize;

    [SerializeField] private Vector2 initialPos;

    [SerializeField] private bool IsDodging;
    [SerializeField] private bool IsAttacking;
    [SerializeField] private bool IsInCooldown;
    
    [SerializeField] private float attackCooldown;
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private float shieldDistance;
    [SerializeField] private float dodgeDistance;
    [SerializeField] private float dodgeTime;
    [SerializeField] private AnimationCurve dodgeCurve;
    [SerializeField] private int damage;

    [SerializeField] private GameObject visualizerPrefab;
    private GameObject _shield;

    private Collider2D _collider;
    
    private void Start() {
        _shield = Instantiate(shieldPrefab, transform);
        _shield.SetActive(false);
        initialPos = transform.position;
        _collider = GetComponent<Collider2D>();
        InputSystem.Instance.OnTap += touch => {
            Attack(GameManager.Instance.Camera.ScreenToWorldPoint(touch.CurrentPosition));
        };
        InputSystem.Instance.OnDrag += touch => {
            Dodge(GameManager.Instance.Camera.ScreenToWorldPoint(touch.CurrentPosition));
        };
        InputSystem.Instance.OnHold += touch => {
            DeployShield();
            UpdateShieldPos(GameManager.Instance.Camera.ScreenToWorldPoint(touch.CurrentPosition));
            touch.OnPositionChanged += pos => UpdateShieldPos(GameManager.Instance.Camera.ScreenToWorldPoint(pos));
            touch.OnEnded += RemoveShield;
        };
    }

    private float attackStartTime = float.MinValue;
    private void Attack(Vector2 target) {
        if (!IsDodging && !IsAttacking) {
            IsAttacking = true;
            attackStartTime = Time.time;
            
            Vector2 attackCenter = (target - (Vector2)transform.position).normalized * attackSize.x/2;
            float angle = Vector2.Angle(Vector2.right, attackCenter);

            GameObject visualizer = Instantiate(visualizerPrefab, transform);
            visualizer.transform.position = (Vector2)transform.position + attackCenter;
            visualizer.transform.right = attackCenter;
            visualizer.transform.localScale = attackSize;
            
            var colliders = Physics2D.OverlapBoxAll(attackCenter + (Vector2)transform.position, attackSize, angle);
            foreach (var collider in colliders) {
                if (TryGetComponent(out IHittable hittable)) {
                    hittable.Hit(damage);
                }
            }
            Debug.Log("Attack");
        }
    }
    private void HandleAttack() {
        if (Time.time - attackStartTime >= attackCooldown) {
            IsAttacking = false;
        }
    }

    private float dodgeStartTime = float.MinValue;
    private Vector2 dodgeDirection;
    private void Dodge(Vector2 direction) {
        if (!IsAttacking && !IsDodging) {
            Debug.Log("Dodge");
            dodgeStartTime = Time.time;
            _collider.enabled = false;
            IsDodging = true;
            direction.y = math.abs(direction.y);
            if (math.abs(direction.x) > direction.y) {
                dodgeDirection = new Vector2(math.sign(direction.x), 0);
            }
            else {
                dodgeDirection = Vector2.up;
            }
        }
    }

    private void HandleDodge() {
        if (IsDodging) {
            float percentage = (Time.time - dodgeStartTime) / dodgeTime;
            if (percentage >= 1) {
                transform.position = initialPos;
                _collider.enabled = true;
                IsDodging = false;
            }
            else {
                transform.position = initialPos + dodgeDirection * (dodgeDistance * dodgeCurve.Evaluate(percentage));
            }
                
        }
    }

    private void Update() {
        HandleAttack();
        HandleDodge();
    }

    private void DeployShield() {
        _shield.SetActive(true);
    }

    private void UpdateShieldPos(Vector2 target) {
        Debug.Log(target);
        
        Vector2 translation = (target - (Vector2)transform.position).normalized * shieldDistance;
        _shield.transform.position = translation;
        _shield.transform.right = translation;
    }

    private void RemoveShield() {
        _shield.SetActive(false);
    }
    
}