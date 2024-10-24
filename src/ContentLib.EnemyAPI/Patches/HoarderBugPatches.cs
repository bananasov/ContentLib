using System;
using ContentLib.EnemyAPI.Model.Enemy;
using ContentLib.EnemyAPI.Model.Enemy.Vanilla.HoarderBug;
using UnityEngine;

namespace ContentLib.EnemyAPI.Patches;

public class HoarderBugPatches
{
    public static void Init()
    {
        Debug.Log("Hoarder Bug Patches");
        On.HoarderBugAI.Start += HoarderBugAI_Start;
    }

    private static void HoarderBugAI_Start(On.HoarderBugAI.orig_Start orig, HoarderBugAI self)
    {
        orig(self);
        Debug.Log("HoarderBugSpawnPatch");
        IEnemy vanillaHoarderBug = new LocalHoarderBug(self);
        Debug.Log("Hoarder Bug registration");
        EnemyManager.Instance.RegisterEnemy(vanillaHoarderBug);
    }

    private class LocalHoarderBug(HoarderBugAI bugAi) : IHoarderBug
    {
        // IEnemy / IEntity METHODS
        public ulong Id => bugAi.NetworkObjectId;
        public bool IsAlive => !bugAi.isEnemyDead;
        public int Health => bugAi.enemyHP;
        public Vector3 Position => bugAi.transform.position;
        public IEnemyProperties EnemyProperties => new HoarderBugProperties(bugAi);
        public bool IsSpawned => bugAi.IsSpawned;
        public bool IsHostile => true;
        public bool IsChasing => bugAi.inChase; // NOTE(bananasov): Should we check if angryAtPlayer is not null too?
        
        //IHoarderBug METHODS:
        //-----------------------------------------------------------------------------
        public void Kill() => throw new NotImplementedException();
    }

    private class HoarderBugProperties(HoarderBugAI bugAi) : IEnemyProperties
    {
        private EnemyType _type = bugAi.enemyType;
        public string Name { get; set; }
        public bool IsCustom => false;
        public Type EnemyClassType => typeof(IHoarderBug);
        public bool SpawningDisabled { get; }
        public AnimationCurve ProbabilityCurve { get; }

        public GameObject EnemyPrefab
        {
            get => _type.enemyPrefab; 
            set => _type.enemyPrefab = value;
        }
        
        public bool IsOutsideEnemy { get; }
        public bool IsDaytimeEnemy { get; }
        public bool SpawnFromWeeds { get; }
        
        public AnimationCurve SpawnWeightMultiplier
        {
            get => _type.probabilityCurve;
            set => _type.probabilityCurve = value;
        }
        
        public int MaxCount
        {
            get => _type.MaxCount;
            set => _type.MaxCount = value;
        }
        
        public float PowerLevel
        {
            get => _type.PowerLevel;
            set => _type.PowerLevel = value; 
        }
        
        public bool CanBeStunned { get; }
        public bool CanDie { get; }
        public bool DestroyOnDeath { get; }
        public float StunTimeMultiplier { get; }
        public float DoorSpeedMultiplier { get; }
        public float StunGameDifficultyMultiplier { get; }
        public bool CanSeeThroughFog { get; }
        public float PushPlayerForce { get; }
        public float PushPlayerDistance { get; }
        public AudioClip HitBodySFX { get; set; }
        public AudioClip HitEnemyVoiceSFX { get; set; }
        public AudioClip DeathSFX { get; set; }
        public AudioClip StunSFX { get; set; }
        public MiscAnimation[] MiscAnimations { get; set; }
        public AudioClip[] AudioClips { get; set; }
        public float TimeToPlayAudio { get; set; }
        public float LoudnessMultiplier { get; set; }
        public AudioClip OverrideVentSFX { get; set; }
        public IEnemyHordeProperties? HordeProperties { get; }
    }
}