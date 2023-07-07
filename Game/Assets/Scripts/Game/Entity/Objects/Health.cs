using System;
using UnityEngine;

namespace Game.Entity
{
    public class Health : MonoBehaviour
    {
        public int MaxHitPoints { get; protected set; }
        public int HitPoints { get; protected set; }

        public void SetupHitPoints(int maxHP) {
            MaxHitPoints = maxHP;
            HitPoints = maxHP;
        }

        public enum DamageType {
            non,
            Shot,
            Explosion,
            Fire,
            PlayerContact
        }
    }
}