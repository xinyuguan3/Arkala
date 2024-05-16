using ClickNext.Scripts.Effects;
using System;
using UnityEngine;

namespace ClickNext.Scripts.Units.Interfaces
{
    public abstract class Unit : MonoBehaviour
    {
        public event Action<Unit> OnHpEmpty = delegate { };

        public Transform[] archeryTargets;
        public Transform NameTag { get { return nameTag; } }
        [SerializeField] private Transform nameTag;

        [SerializeField] Effect unspawnParticle;

        [Serializable]
        public struct Stat
        {
            public float hp;
            public float range;
            public float speed;
            public float actionDelay;
        }

        [HideInInspector] public Stat Current;
        public float MaxHp { get { return stats.hp; } }
        public float Range { get { return stats.range; } }
        public float Speed { get { return stats.speed; } }
        public float ActionDelay { get { return stats.actionDelay; } }

        [SerializeField] private Stat stats;

        protected void Awake()
        {
            Current.hp = MaxHp;
            Current.range = Range;
            Current.speed = Speed;
            Current.actionDelay = ActionDelay;
        }

        public virtual void ReceiveValue(float value, Vector3 takePosition)
        {
            if (Current.hp <= 0f)
            {
                if (unspawnParticle)
                    unspawnParticle.Play();

                OnHpEmpty(this);
            }
        }

        public virtual void SetAllow(bool value) { }
    }
}
