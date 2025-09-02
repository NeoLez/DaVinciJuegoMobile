using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NonMonobehaviorUpdates {
    /// <summary>
    /// Custom manager for update and fixedUpdate events
    /// </summary>
    public static class UpdatesManager {
        static readonly List<ITickableUpdate> tickables = new(10000);
        static readonly List<ITickableUpdate> sweep = new();
        
        static readonly List<ITickableFixedUpdate> tickablesFixed = new(10000);
        static readonly List<ITickableFixedUpdate> sweepFixed = new();
        
        public static void RegisterUpdate(ITickableUpdate tickableUpdate) => tickables.Add(tickableUpdate);
        public static void DeregisterUpdate(ITickableUpdate tickableUpdate) => tickables.Remove(tickableUpdate);
        
        public static void RegisterFixedUpdate(ITickableFixedUpdate tickableFixedUpdate) => tickablesFixed.Add(tickableFixedUpdate);
        public static void DeregisterFixedUpdate(ITickableFixedUpdate tickableFixedUpdate) => tickablesFixed.Remove(tickableFixedUpdate);

        public static void TickUpdate() {
            if (tickables.Count == 0) return;
            
            sweep.Clear();
            sweep.AddRange(tickables);
            
            foreach (var tickable in sweep) {
                tickable.OnUpdate();
            }
        }
        
        public static void TickFixedUpdate() {
            if (tickablesFixed.Count == 0) return;
            
            sweepFixed.Clear();
            sweepFixed.AddRange(tickablesFixed);
            
            foreach (var tickable in sweepFixed) {
                tickable.OnFixedUpdate();
            }
        }
        
        [InitializeOnEnterPlayMode]
        public static void Clear() {
            tickables.Clear();
            sweep.Clear();
            tickablesFixed.Clear();
            sweepFixed.Clear();
        }
    }
}