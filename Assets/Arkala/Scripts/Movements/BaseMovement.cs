using ClickNext.Scripts.Units.Interfaces;
using System;
using UnityEngine;

namespace ClickNext.Scripts.Movements
{
    public class BaseMovement : MonoBehaviour
    {
        protected TagType _findTag;

        private MoveUnit _moveUnit;

        protected void Awake() => _moveUnit = GetComponent<MoveUnit>();

        protected void FindAndMoveTo(GameObject target, TagType tagType,
            Action<GameObject> targetChanged, 
            Action targetReached)
        {
            if (!IsTargetValid(target, tagType))
            {
                _moveUnit.CancelPath();
                var newTarget = _moveUnit.FindTarget(tagType.ToString());
                targetChanged(newTarget);
            }

            _moveUnit.Follow(target, isReached =>
            {
                if (isReached)
                    targetReached();
            });
        }

        protected void RandomTargetAndMoveTo(ref GameObject target, TagType tagType, Action targetReached)
        {
            if (!IsTargetValid(target, tagType))
            {
                _moveUnit.CancelPath();
                var targets = GameObject.FindGameObjectsWithTag(tagType.ToString());
                if (targets.Length > 0)
                    target = targets[UnityEngine.Random.Range(0, targets.Length)];
                else
                    target = null;
            }

            _moveUnit.Follow(target, isReached =>
            {
                if (isReached)
                    targetReached();
            });
        }

        protected bool AnyTarget(TagType findTag)
        {
            _findTag = findTag;
            return _moveUnit.FindTarget(findTag.ToString());
        }            

        protected static bool IsTargetValid(GameObject target, TagType tagType) =>
            target &&
            target.activeInHierarchy &&
            target.CompareTag(tagType.ToString());

        protected bool IsRangeValid(GameObject target) => 
            _moveUnit.InRange(target);

        protected void TurnAround(Transform target)
        {
            var lookPos = target.position - transform.position; 
                lookPos.y = 0;
            transform.rotation = Quaternion.LookRotation(lookPos);
        }
    }
}
