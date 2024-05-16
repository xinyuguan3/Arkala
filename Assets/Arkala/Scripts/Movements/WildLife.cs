using ClickNext.Scripts.Units;
using ClickNext.Scripts.Units.Interfaces;
using System.Collections;
using UnityEngine;
namespace ClickNext.Scripts.Movements
{
    [RequireComponent(typeof(Animal))]
    public class WildLife : BaseMovement
    {
        [SerializeField] float duration;
        [SerializeField] float runawaysDistance;
        [SerializeField] float increaseSpeed;

        private GameObject target;
        private float currentDuration;
        private Coroutine eatRoutine;

        private Animal animal;
        private bool isRunaway;

        protected new void Awake()
        {
            animal = GetComponent<Animal>();
            base.Awake();
        }

        private void OnEnable() =>
            animal.OnTakeDamage += OnTakeDamage;

        protected void OnDisable()
        {
            animal.OnTakeDamage -= OnTakeDamage;
            target = null;
            animal = null;
            eatRoutine = null;
            StopAllCoroutines();
        }

        private void OnTakeDamage(Vector3 attackerPosition)
        {
            isRunaway = true;
            animal.Current.speed = animal.Speed + increaseSpeed;
            animal.Runaway(attackerPosition, runawaysDistance, isReached =>
            {
                isRunaway = false;  
                if(animal)
                    animal.Current.speed = animal.Speed;
            });

            target = null;
        }

        void Start()
        {
            _findTag = TagType.Grass;
            currentDuration = duration;
        }

        void Update()
        {
            if (!isRunaway)
                RandomTargetAndMoveTo(ref target, _findTag,
                        targetReached: () => 
                        { 
                            if(eatRoutine == null)
                                eatRoutine = StartCoroutine(Eat()); 
                        }
                    );
        }

        private IEnumerator Eat()
        {
            var unit = target.GetComponentInParent<LocationUnit>();

            while (currentDuration > 0 && target)
            {
                currentDuration -= Time.deltaTime;

                if (unit)
                    unit.ReceiveValue(Time.deltaTime, transform.position);
                else
                    break;

                yield return null;
            }

            currentDuration = duration;
            target = null;
            eatRoutine = null;
        }
    }
}
