using System;
using UnityEngine;

public class GameManager : MonoBehaviour{
    public static GameManager Instance {
        get;
        private set;
    }
    private void Awake() {
        Instance = this;
        Camera = Camera.main;
    }

    [NonSerialized] public Camera Camera;
}