using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class BattleManager : MonoBehaviour { 
    public static BattleManager Instance;
    private void Awake() {
        Instance = this;
    }

    public bool _running;
    public List<Enemy> enemies = new();
    public float tickTime;
    private Timer _tickTimer = new();
    public GameObject Player;

    public void SetRunning(bool running) {
        _running = running;
        _tickTimer.Start(Time.time);
    }
    
    private void Update() {
        if (_running && _tickTimer.HasFinished()) {
            _tickTimer.AddTime(tickTime);
            foreach (var enemy in enemies) {
                enemy.Tick();
            }
        }

        if (enemies.Count == 0) SceneManager.LoadScene(0);
    }
}