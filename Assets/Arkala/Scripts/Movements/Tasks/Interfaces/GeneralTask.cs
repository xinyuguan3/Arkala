using ClickNext.Scripts.Items.Weapons;
using ClickNext.Scripts.Units.Interfaces;
using System;
using System.Collections;
using UnityEngine;

namespace ClickNext.Scripts.Movements.Tasks.Interfaces
{
    public abstract class GeneralTask : ITask
    {
        protected string _actionName;

        private GameObject target;
        private Coroutine runRoutine;

        protected new void OnEnable()
        {
            target = _worker.FindTarget(_findTag.ToString());
            runRoutine = null;

            base.OnEnable();
        }

        private void Update()
        {
            if (_worker.Current.actionDelay > 0)
                _worker.Current.actionDelay -= Time.deltaTime;

            if (runRoutine == null)
            {
                FindAndMoveTo(target, _findTag,
                    targetChanged: newTarget => enabled = false,
                    targetReached: () =>
                    {
                        if (runRoutine == null)
                            runRoutine = StartCoroutine(Run());
                    }
                );
            }
        }

        private IEnumerator Run()
        {
            yield return new WaitUntil(() => _worker.Current.actionDelay <= 0);

            if (!IsTargetValid(target, _findTag))
            {
                target = null;
                runRoutine = null;
                yield break;
            }

            var unit = target.GetComponentInParent<Unit>();
            if (unit)
            {
                if (_weapon)
                {
                    StartCoroutine(_weapon.DelayAttack(unit,
                        onAttack: () =>
                        {
                            TurnAround(unit.transform);
                            _worker.UpdateStatus();
                            _workerAnimation.PlayAction(_actionName);
                        },
                        onHit: damage =>
                        {
                            if(unit)
                                unit.ReceiveValue(damage, takePosition: transform.position);
                        }));
                }
            }

            //wait for animation.
            yield return new WaitForSeconds(2f);
            runRoutine = null;
        }

        protected bool FindItem<T>(string actionName, Predicate<Tools> predicate = null)
        {
            var items = GetComponentsInChildren<T>(includeInactive: true);
            
            foreach (var item in items)
            {
                if (typeof(T) == typeof(Tool))
                {
                    var tool = item as Tool;
                    if (predicate != null && predicate(tool.Type))
                    {
                        _weapon = tool;
                        break;
                    }
                }
                else
                {
                    var weapon = item as Weapon;
                    _weapon = weapon;
                    break;
                }
            }

            _actionName = actionName;

            return _weapon != null;
        }
    }
}
