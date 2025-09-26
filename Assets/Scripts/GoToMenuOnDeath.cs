using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Health))]
public class GoToMenuOnDeath : MonoBehaviour{
    private void Awake() {
        GetComponent<Health>().OnDie += () => {
            SceneManager.LoadScene(0);
        };
    }
}