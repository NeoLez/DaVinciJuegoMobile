using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NonMonobehaviorUpdates;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace PlayerLoopCleaner {
    public static class PlayerLoopCleaner {
        private static List<PlayerLoopSystem> insertedSystems = new ();
        //List of subSystems to remove
        private static readonly Type[] TypesToRemove = {
            typeof(TimeUpdate.WaitForLastPresentationAndUpdateTime), //ALLEGEDLY BREAKS A LOT OF TIME.DELTATIME THINGS
            
            //typeof(Initialization.AsyncUploadTimeSlicedUpdate), //
            typeof(Initialization.DirectorSampleTime),//DIRECTOR
            typeof(Initialization.ProfilerStartFrame),//PROFILER
            //typeof(Initialization.SynchronizeInputs), //BREAKS INPUTS
            //typeof(Initialization.SynchronizeState), //ALLEGEDLY BREAKS A LOT OF THINGS
            typeof(Initialization.UpdateCameraMotionVectors), //ALLEGEDLY BREAKS TAA, MOTION BLUR AND SUCH
            typeof(Initialization.XREarlyUpdate),
            
            typeof(EarlyUpdate.PerformanceAnalyticsUpdate),
            typeof(EarlyUpdate.ClearLines), //COULD BREAK GIZMOS
            typeof(EarlyUpdate.GpuTimestamp), //ALLEGEDLY BREAKS PERFORMANCE ANALYTICS
            typeof(EarlyUpdate.PhysicsResetInterpolatedTransformPosition), //ALLEGEDLY BREAKS INTERPOLATION FOR 3D PHYSICS
            typeof(EarlyUpdate.AnalyticsCoreStatsUpdate),
            typeof(EarlyUpdate.ARCoreUpdate),
            typeof(EarlyUpdate.ClearIntermediateRenderers), //COULD CAUSE RENDERING ISSUES
            typeof(EarlyUpdate.DeliverIosPlatformEvents), //ALLEGEDLY BREAKS A LOT OF THINGS ON IOS
            //typeof(EarlyUpdate.DispatchEventQueueEvents), //?
            typeof(EarlyUpdate.ExecuteMainThreadJobs), //?
            //typeof(EarlyUpdate.Physics2DEarlyUpdate),
            //typeof(EarlyUpdate.PlayerCleanupCachedData),
            typeof(EarlyUpdate.PollHtcsPlayerConnection),
            //typeof(EarlyUpdate.PollPlayerConnection), //ALLEGEDLY BREAKS EDITOR AND REMOTE DEBUGGING
            //typeof(EarlyUpdate.PresentBeforeUpdate), //ALLEGEDLY MORE INPUT LAG
            //typeof(EarlyUpdate.ProcessMouseInWindow),
            //typeof(EarlyUpdate.ProcessRemoteInput), //ALLEGEDLY BREAKS UNITY REMOTE
            //typeof(EarlyUpdate.RendererNotifyInvisible),
            typeof(EarlyUpdate.ResetFrameStatsAfterPresent),
            //typeof(EarlyUpdate.ScriptRunDelayedStartupFrame),
            //typeof(EarlyUpdate.SpriteAtlasManagerUpdate),
            typeof(EarlyUpdate.UnityWebRequestUpdate),
            //typeof(EarlyUpdate.UpdateAsyncReadbackManager),
            //typeof(EarlyUpdate.UpdateCanvasRectTransform),
            //typeof(EarlyUpdate.UpdateContentLoading),
            //typeof(EarlyUpdate.UpdateInputManager), //BREAKS LEGACY INPUT SYSTEM
            typeof(EarlyUpdate.UpdateKinect),
            //typeof(EarlyUpdate.UpdateMainGameViewRect),
            //typeof(EarlyUpdate.UpdatePreloading),
            //typeof(EarlyUpdate.UpdateStreamingManager),
            //typeof(EarlyUpdate.UpdateTextureStreamingManager),
            typeof(EarlyUpdate.XRUpdate),
            
            //typeof(FixedUpdate.AudioFixedUpdate),
            typeof(FixedUpdate.ClearLines),
            typeof(FixedUpdate.DirectorFixedSampleTime),
            typeof(FixedUpdate.DirectorFixedUpdate),
            typeof(FixedUpdate.DirectorFixedUpdatePostPhysics),
            typeof(FixedUpdate.LegacyFixedAnimationUpdate),
            typeof(FixedUpdate.NewInputFixedUpdate), // BREAKS NEW INPUT SYSTEM
            //typeof(FixedUpdate.Physics2DFixedUpdate),
            typeof(FixedUpdate.PhysicsFixedUpdate),
            //typeof(FixedUpdate.ScriptRunBehaviourFixedUpdate), //BREAKS FIXED UPDATE SCRIPTS
            //typeof(FixedUpdate.ScriptRunDelayedFixedFrameRate), //BREAKS FIXED UPDATE SCRIPTS
            typeof(FixedUpdate.XRFixedUpdate),
            
            typeof(PreUpdate.AIUpdate),
            typeof(PreUpdate.PhysicsUpdate),
            typeof(PreUpdate.UpdateVideo),
            typeof(PreUpdate.WindUpdate),
            //typeof(PreUpdate.CheckTexFieldInput),
            typeof(PreUpdate.IMGUISendQueuedEvents),
            typeof(PreUpdate.NewInputUpdate),
            //typeof(PreUpdate.Physics2DUpdate),
            //typeof(PreUpdate.SendMouseEvents),
            
            typeof(Update.DirectorUpdate),
            //typeof(Update.ScriptRunBehaviourUpdate),
            //typeof(Update.ScriptRunDelayedDynamicFrameRate),
            //typeof(Update.ScriptRunDelayedTasks),
            
            typeof(PreLateUpdate.AIUpdatePostScript),
            //typeof(PreLateUpdate.ConstraintManagerUpdate), // ALLEGEDLY BREAKS PHYSICS CONSTRAINTS (3D and 2D)
            typeof(PreLateUpdate.DirectorUpdateAnimationBegin),
            typeof(PreLateUpdate.DirectorDeferredEvaluate),
            typeof(PreLateUpdate.DirectorUpdateAnimationEnd),
            //typeof(PreLateUpdate.EndGraphicsJobsAfterScriptUpdate),
            typeof(PreLateUpdate.LegacyAnimationUpdate),
            //typeof(PreLateUpdate.ParticleSystemBeginUpdateAll),
            //typeof(PreLateUpdate.Physics2DLateUpdate),
            typeof(PreLateUpdate.PhysicsLateUpdate),
            //typeof(PreLateUpdate.ScriptRunBehaviourLateUpdate),
            typeof(PreLateUpdate.UIElementsUpdatePanels),
            typeof(PreLateUpdate.UpdateMasterServerInterface),
            typeof(PreLateUpdate.UpdateNetworkManager),
            
            typeof(PostLateUpdate.BatchModeUpdate),
            //typeof(PostLateUpdate.ClearImmediateRenderers),
            typeof(PostLateUpdate.DirectorLateUpdate),
            typeof(PostLateUpdate.DirectorRenderImage),
            //typeof(PostLateUpdate.EndGraphicsJobsAfterScriptLateUpdate),
            typeof(PostLateUpdate.EnlightenRuntimeUpdate),
            typeof(PostLateUpdate.ExecuteGameCenterCallbacks),
            //typeof(PostLateUpdate.FinishFrameRendering),
            //typeof(PostLateUpdate.GraphicsWarmupPreloadedShaders),
            typeof(PostLateUpdate.GUIClearEvents),
            //typeof(PostLateUpdate.InputEndFrame),
            //typeof(PostLateUpdate.MemoryFrameMaintenance),
            //typeof(PostLateUpdate.ObjectDispatcherPostLateUpdate),
            //typeof(PostLateUpdate.ParticleSystemEndUpdateAll),
            typeof(PostLateUpdate.PhysicsSkinnedClothBeginUpdate),
            typeof(PostLateUpdate.PhysicsSkinnedClothFinishUpdate),
            //typeof(PostLateUpdate.PlayerEmitCanvasGeometry),
            //typeof(PostLateUpdate.PlayerSendFrameComplete),
            //typeof(PostLateUpdate.PlayerSendFramePostPresent),
            //typeof(PostLateUpdate.PlayerSendFrameStarted),
            //typeof(PostLateUpdate.PlayerUpdateCanvases),
            //typeof(PostLateUpdate.PresentAfterDraw),
            typeof(PostLateUpdate.ProcessWebSendMessages),
            typeof(PostLateUpdate.ProfilerEndFrame),
            typeof(PostLateUpdate.ProfilerSynchronizeStats),
            //typeof(PostLateUpdate.ResetInputAxis), //BREAKS OLD INPUT SYSTEM
            //typeof(PostLateUpdate.ScriptRunDelayedDynamicFrameRate),
            typeof(PostLateUpdate.ShaderHandleErrors), //MAYBE DISABLE LATER
            //typeof(PostLateUpdate.SortingGroupsUpdate),
            typeof(PostLateUpdate.ThreadedLoadingDebug),
            //typeof(PostLateUpdate.TriggerEndOfFrameCallbacks),
            //typeof(PostLateUpdate.UpdateAllRenderers),
            typeof(PostLateUpdate.UpdateAllSkinnedMeshes),
            //typeof(PostLateUpdate.UpdateAudio),
            //typeof(PostLateUpdate.UpdateCanvasRectTransform),
            typeof(PostLateUpdate.UpdateCaptureScreenshot),
            typeof(PostLateUpdate.UpdateCustomRenderTextures), //?
            typeof(PostLateUpdate.UpdateLightProbeProxyVolumes),
            //typeof(PostLateUpdate.UpdateRectTransform),
            //typeof(PostLateUpdate.UpdateResolution),
            typeof(PostLateUpdate.UpdateSubstance),
            typeof(PostLateUpdate.UpdateVideo),
            typeof(PostLateUpdate.UpdateVideoTextures),
            //typeof(PostLateUpdate.VFXUpdate),
            typeof(PostLateUpdate.XRPostLateUpdate),
            typeof(PostLateUpdate.XRPostPresent),
            typeof(PostLateUpdate.XRPreEndFrame),
        };

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RegisterNewSystems() {
            InsertSystemBefore(typeof(UpdatesManager), UpdatesManager.TickUpdate, typeof(Update.ScriptRunBehaviourUpdate));
            InsertSystemBefore(typeof(UpdatesManager), UpdatesManager.TickFixedUpdate, typeof(FixedUpdate.ScriptRunBehaviourFixedUpdate));
        }
        public static void InsertSystemBefore(Type newSystemMarker, PlayerLoopSystem.UpdateFunction newSystemUpdate, Type insertBefore) {
            var playerLoopSystem = new PlayerLoopSystem {type = newSystemMarker, updateDelegate = newSystemUpdate};
            InsertSystemBefore(playerLoopSystem, insertBefore);
        }
        public static void InsertSystemBefore(PlayerLoopSystem toInsert, Type insertBefore) {
            var rootSystem = PlayerLoop.GetCurrentPlayerLoop();
            InsertSystem(ref rootSystem, toInsert, insertBefore, InsertType.Before, out var couldInsert);
            insertedSystems.Add(toInsert);
            PlayerLoop.SetPlayerLoop(rootSystem);
        }
        private enum InsertType {
            Before,
            After
        }
        private static void InsertSystem(ref PlayerLoopSystem currentLoopRecursive, PlayerLoopSystem toInsert, Type insertTarget, InsertType insertType,
            out bool couldInsert) {
            var currentSubSystems = currentLoopRecursive.subSystemList;
            if (currentSubSystems == null) {
                couldInsert = false;
                return;
            }

            int indexOfTarget = -1;
            for (int i = 0; i < currentSubSystems.Length; i++) {
                if (currentSubSystems[i].type == insertTarget) {
                    indexOfTarget = i;
                    break;
                }
            }

            if (indexOfTarget != -1) {
                var newSubSystems = new PlayerLoopSystem[currentSubSystems.Length + 1];

                var insertIndex = insertType == InsertType.Before ? indexOfTarget : indexOfTarget + 1;

                for (int i = 0; i < newSubSystems.Length; i++) {
                    if (i < insertIndex)
                        newSubSystems[i] = currentSubSystems[i];
                    else if (i == insertIndex) {
                        newSubSystems[i] = toInsert;
                    }
                    else {
                        newSubSystems[i] = currentSubSystems[i - 1];
                    }
                }

                couldInsert = true;
                currentLoopRecursive.subSystemList = newSubSystems;
            }
            else {
                for (var i = 0; i < currentSubSystems.Length; i++) {
                    var subSystem = currentSubSystems[i];
                    InsertSystem(ref subSystem, toInsert, insertTarget, insertType, out var couldInsertInInner);
                    if (couldInsertInInner) {
                        currentSubSystems[i] = subSystem;
                        couldInsert = true;
                        return;
                    }
                }

                couldInsert = false;
            }
        }
        
        //Removes all instances of the systems specified in TypesToRemove
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RunChanges() {
            
            var systems = PlayerLoop.GetCurrentPlayerLoop();
            DeleteSubsystems(ref systems, TypesToRemove);
            PlayerLoop.SetPlayerLoop(systems);
        }
        
        private static void DeleteSubsystems(ref PlayerLoopSystem system, Type[] typesToDelete) {
            if (system.subSystemList == null)
                return;
            List<PlayerLoopSystem> subSystems = system.subSystemList.ToList();
            subSystems.RemoveAll(loopSystem => typesToDelete.Contains(loopSystem.type));
            system.subSystemList = subSystems.ToArray();

            for (int i = 0; i < system.subSystemList.Length; i++) {
                DeleteSubsystems(ref system.subSystemList[i], typesToDelete);
            }
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void PrintCurrentPlayerLoop() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Unity Player Loop");
            foreach (PlayerLoopSystem subSystem in PlayerLoop.GetCurrentPlayerLoop().subSystemList) {
                PrintSubsystem(subSystem, sb, 0);
            }
            Debug.Log(sb.ToString());
        }

        private static void PrintSubsystem(PlayerLoopSystem system, StringBuilder sb, int level) {
            sb.Append(' ', level * 2).AppendLine(system.type.ToString());
            if (system.subSystemList == null || system.subSystemList.Length == 0) return;

            foreach (PlayerLoopSystem subSystem in system.subSystemList) {
                PrintSubsystem(subSystem, sb, level + 1);
            }
        }
    }
}