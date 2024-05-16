using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace ClickNext.Scripts.Units.Interfaces
{
    public abstract class MoveUnit : Unit
    {
        private NavMeshPath path;
        private NavMeshAgent agent;
        private Coroutine moveRoutine;

        private GameObject followingTarget;

        private readonly float fixedAngularSpeed = 70f;
        private readonly float fixedStoppingDistance = 0.1f;
        private readonly float fixedAcceleration = 100f;

        protected new void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.autoTraverseOffMeshLink = true;
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            agent.stoppingDistance = fixedStoppingDistance;
            agent.acceleration = fixedAcceleration;

            path = new NavMeshPath();

            base.Awake();
        }

        protected void Update()
        {
            if (agent.isOnOffMeshLink)
                agent.CompleteOffMeshLink();
        }

        protected void FixedUpdate()
        {
            agent.speed = Current.speed;
            agent.angularSpeed = agent.speed * fixedAngularSpeed;
        }

        public GameObject FindTarget(string targetTag)
        {
            var targets = GameObject.FindGameObjectsWithTag(targetTag);
            var selectedCost = float.MaxValue;
            GameObject selectedTarget = null;

            foreach(var target in targets)
            {
                var cost = CalculatePathCost(target.transform.position);
                if (selectedCost <= cost) continue;
                    selectedCost = cost;
                    selectedTarget = target;
            }

            return selectedTarget;
        }

        public void Follow(GameObject target, Action<bool> reached)
        {
            if (!target)
                return;

            if (followingTarget == target && InRange(target))
            {
                reached(true);
                return;
            }

            followingTarget = target;
            
            if (moveRoutine != null)
                StopCoroutine(moveRoutine);
            
            moveRoutine = StartCoroutine(MoveRoutine(
                isFollowTarget: true, 
                target.transform.position, 
                reached,
                target));
        }

        public Vector3 Runaway(Vector3 source, float distance, Action<bool> reached)
        {
            if (moveRoutine != null)
                StopCoroutine(moveRoutine);

            Vector3 targetPosition;
            var direction = (transform.position - source).normalized * distance;
            var position = transform.position + direction;
            var runAwayPosition = new Vector3(position.x, transform.position.y, position.z);

            targetPosition = NavMesh.SamplePosition(runAwayPosition, out NavMeshHit hit, 1.0f, NavMesh.AllAreas)? 
                hit.position: RandomPoint(transform.position, distance);
            Debug.DrawRay(targetPosition, Vector3.up, Color.blue, 1.0f);

            moveRoutine = StartCoroutine(MoveRoutine(
                isFollowTarget: false,
                targetPosition, 
                reached));

            return position;
        }

        private Vector3 RandomPoint(Vector3 center, float range)
        {
            for (int i = 0; i < 30; ++i)
            {
                Vector3 randomPoint = center + (UnityEngine.Random.insideUnitSphere * range);
                if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                    return hit.position;
            }

            return center;
        }

        private IEnumerator MoveRoutine(bool isFollowTarget, Vector3 position, 
            Action<bool> reached, GameObject target = null)
        {
            var distance = float.MaxValue;
            while ( distance > Current.range)
            {
                if (isFollowTarget)
                {
                    if (!target || !agent || !gameObject)
                    {
                        reached(false);
                        yield break;
                    }

                    position = target.transform.position;
                }

                if (agent.isOnNavMesh)
                {
                    agent.SetDestination(position);
                    distance = agent.remainingDistance;
                }

                if (agent.pathStatus != NavMeshPathStatus.PathComplete ||
                    !agent.CalculatePath(position, path))
                {
                    ResetPath();
                    reached(false);
                    yield break;
                }
                yield return null;
            }

            ResetPath();
            reached(true);
        }

        private void ResetPath()
        {
            if(agent.isOnNavMesh)
                agent.ResetPath();
            moveRoutine = null;
        }


        protected void ClearTarget() => followingTarget = null;

        public void CancelPath()
        {
            if(moveRoutine != null)
                StopCoroutine(moveRoutine);

            if(agent.isOnNavMesh)
                agent.ResetPath();

            followingTarget = null;
        }

        public bool InRange(GameObject target) => 
            Vector3.Distance(agent.transform.position, target.transform.position) <= Current.range;

        // calulate path without set to target.
        private float CalculatePathCost(Vector3 position)
        {
            NavMesh.CalculatePath(agent.transform.position, position, NavMesh.AllAreas, path);
            if (path.status != NavMeshPathStatus.PathComplete)
                return float.MaxValue;

            var corners = path.corners;
            if (corners.Length < 2)
                return Mathf.Infinity;

            NavMesh.SamplePosition(corners[0], out NavMeshHit hit, 0.1f, NavMesh.AllAreas);

            var pathCost = 0.0f;
            var costMultiplier = NavMesh.GetAreaCost(IndexFromMask(hit.mask));
            var rayStart = corners[0];

            foreach (var corner in corners)
            {
                // the segment may contain several area types - iterate over each
                while (NavMesh.Raycast(rayStart, corner, out hit, hit.mask))
                {
                    var otherArea = IndexFromMask(hit.mask);
                    if (otherArea == -1)
                        continue;
                    pathCost += costMultiplier * hit.distance;
                    costMultiplier = NavMesh.GetAreaCost(otherArea);
                    rayStart = hit.position;
                }

                // advance to next segment
                var areaIndex = IndexFromMask(hit.mask);
                if (areaIndex == -1)
                    continue;
                pathCost += costMultiplier * hit.distance;
                costMultiplier = NavMesh.GetAreaCost(areaIndex);
                rayStart = hit.position;
            }

            return pathCost;
        }

        private int IndexFromMask(int mask)
        {
            for (int i = 0; i < 32; ++i)
            {
                if ((1 << i) == mask)
                    return i;
            }
            return -1;
        }
    }
}
