using System;
using ContentLib.Core.Model.Event;
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
        #region IEnemy / IEntity methods
        public ulong Id => bugAi.NetworkObjectId;
        public bool IsAlive => !bugAi.isEnemyDead;
        public int Health => bugAi.enemyHP;
        public Vector3 Position => bugAi.transform.position;
        public IEnemyProperties EnemyProperties => new HoarderBugProperties(bugAi);
        public bool IsSpawned => bugAi.IsSpawned;
        public bool IsHostile => true;
        public bool IsChasing => bugAi.inChase; // NOTE(bananasov): Should we check if angryAtPlayer is not null too?
        #endregion
        
        #region IHoarderBug methods
        public void Kill() => throw new NotImplementedException();
        #endregion
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
        
        public bool IsOutsideEnemy { get; set; }
        public bool IsDaytimeEnemy { get; set; }
        public bool SpawnFromWeeds { get; set; }
        
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
        
        public bool CanBeStunned { get; set; }
        public bool CanDie { get; set; }
        public bool DestroyOnDeath { get; set; }
        public float StunTimeMultiplier { get; set; }
        public float DoorSpeedMultiplier { get; set; }
        public float StunGameDifficultyMultiplier { get; set; }
        public bool CanSeeThroughFog { get; set; }
        public float PushPlayerForce { get; set; }
        public float PushPlayerDistance { get; set; }
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

    private class LocalHoarderBugSpawnEvent(IEnemy enemy) : MonsterSpawnEvent
    {
        public override IEnemy Enemy => enemy;
    }
}