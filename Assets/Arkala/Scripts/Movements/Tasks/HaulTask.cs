using System.Collections;
using ClickNext.Scripts.Animations;
using ClickNext.Scripts.Items;
using ClickNext.Scripts.Movements.Tasks.Interfaces;
using ClickNext.Scripts.Units;
using UnityEngine;

namespace ClickNext.Scripts.Movements.Tasks
{
    [RequireComponent(typeof(Worker))]
    public class HaulTask : ITask
    {        
        private GameObject target;
        private GameObject storage;
        private RawMaterial handled;

        private Coroutine hualRoutine;
        private Coroutine storeRoutine;

        public override bool IsAvailable() => 
            AnyTarget(TagType.Materials) &&
            AnyTarget(TagType.Storage);

        protected new void OnEnable()
        {
            target = null;
            storage = null;
            handled = null;
            hualRoutine = null;
            storeRoutine = null;

            base.OnEnable();
        }

        protected new void OnDisable()
        {
            Drop();
            base.OnDisable();
        }

        private void Update()
        {
            if (!handled)
            {
                FindAndMoveTo(target, TagType.Materials,
                    targetChanged: newTarget =>
                    {
                        target = newTarget;
                        enabled = target || handled;
                    },
                    targetReached: () =>
                    {
                        if(hualRoutine == null)
                            hualRoutine = StartCoroutine(Haul(target));
                    }
                );
            }
            else
            {
                if (!handled.IsOwner(gameObject))
                {
                    handled = null;
                    _worker.UpdateStatus();
                    return;
                }

                FindAndMoveTo(storage, TagType.Storage,
                    targetChanged: newTarget =>
                    {
                        storage = newTarget;
                        enabled = IsStorageAvailable();
                    },
                    targetReached: () =>
                    {
                        if(storeRoutine == null)
                            storeRoutine = StartCoroutine(Keep());
                    }
                );
            }

            _workerAnimation.SetCarrying(handled != null);
        }

        private IEnumerator Haul(GameObject target)
        {
            _workerAnimation.PlayAction(WorkerAnimation.PULL);

            yield return new WaitForSeconds(_worker.Current.actionDelay);
            if (!target || !IsRangeValid(target))
            {
                hualRoutine = null;
                yield break;
            }

            if (!handled)
            {
                handled = target.GetComponent<RawMaterial>();
                if (handled.SetPicked(gameObject))
                    _worker.EquipItem(handled);
            }

            hualRoutine = null;
        }

        private IEnumerator Keep()
        {
            yield return new WaitForSeconds(_worker.Current.actionDelay);
            if (IsStorageAvailable())
            {
                var storage = this.storage.GetComponentInParent<Storage>();
                    storage.AddRawMaterial(handled.materialType, 1);

                Stored();
            }
            else
            {
                storage = null;
                storeRoutine = null;
                yield break;
            }

            //done.
            storeRoutine = null;
            enabled = false;
        }

        private bool IsStorageAvailable()
        {
            var isAvailable = storage &&
                storage.GetComponentInParent<Storage>() &&
                storage.activeInHierarchy;

            if (!isAvailable)
                storage = null;
            return isAvailable;
        }

        public void Drop()
        {
            if (!handled)
                return;

            _workerAnimation.SetCarrying(false);
            _worker.UnEquipItem(handled);
            handled.SetPicked(null);
            handled = null;
        }

        public void Stored()
        {
            _workerAnimation.SetCarrying(false);
            _worker.UnEquipItem(handled);
            Destroy(handled.gameObject);
            handled = null;
        }
    }
}
