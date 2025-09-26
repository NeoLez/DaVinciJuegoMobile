using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
    [SerializeReference, SubclassSelector] private List<IEnemyAction> _actions = new();
    private int currentActionIndex;
    public void Tick() {
        if (_actions[currentActionIndex].Execute(gameObject)) {
            currentActionIndex = (currentActionIndex + 1) % _actions.Count;
        }
    }
}