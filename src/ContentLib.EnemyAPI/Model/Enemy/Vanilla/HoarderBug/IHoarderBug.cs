using ContentLib.Core.Model.Entity.Util;
using UnityEngine;

namespace ContentLib.EnemyAPI.Model.Enemy.Vanilla.HoarderBug;

/// <summary>
/// Interface representing the general functionality of a Hoarder Bug.
/// </summary>
public interface IHoarderBug : IEnemy, IKillable
{
    /// <summary>
    /// The amount of how annoyed the Hoarder Bug is.
    /// </summary>
    float Annoyance { get; }
    
    /// <summary>
    /// Checks the current anger state of the Hoarder Bug.
    /// </summary>
    bool IsAngry { get; }
    
    /// <summary>
    /// The position of the Hoarder Bug's nest. 
    /// </summary>
    Vector3 NestPosition { get; }
    
    /// <summary>
    /// Checks whether the Hoarding Bug is waiting at their Nest
    /// </summary>
    bool IsWaitingAtNest { get;  }

}