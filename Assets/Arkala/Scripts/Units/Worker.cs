using System.Collections;
using System.Collections.Generic;
using ClickNext.Scripts.Movements.Tasks.Interfaces;
using ClickNext.Scripts.Units.Interfaces;
using UnityEngine;

namespace ClickNext.Scripts.Units
{
    [DisallowMultipleComponent]
    public class Worker : EquippedUnit
    {
        public enum Gender
        {
            Male,
            Female
        }

        public Gender GenderType = Gender.Male;

        private ITask[] _workTasks;
        
        private new void Awake()
        {
            _workTasks = GetComponents<ITask>();

            foreach (var task in _workTasks)
                task.enabled = false;

            base.Awake();
        }

        private void OnEnable()
        {
            foreach (var task in _workTasks)
                task.OnDone += OnTaskFinished;

            StopAllCoroutines();
            StartCoroutine(StartSpawn());
        }

        private void OnDisable()
        {
            foreach (var task in _workTasks)
                task.OnDone -= OnTaskFinished;

            ClearTarget();
        }

        private void OnTaskFinished()
        {
            ClearTarget();
            StopAllCoroutines();
            StartCoroutine(ChangeTask());
        }

        private IEnumerator StartSpawn()
        {
            yield return new WaitForSeconds(2f);
            yield return ChangeTask();
        }

        private IEnumerator ChangeTask()
        {
            while (!AnySelectedTask())
            {
                yield return new WaitForSeconds(1f);

                var probabilities = GenerateProbabilities();
                if (probabilities.Length == 0)
                {
                    yield return new WaitForSeconds(1f);
                    continue;
                }

                var random = Random.Range(0, probabilities.Length);
                if (probabilities[random].StartWork())
                    yield break;
            }
        }

        private bool AnySelectedTask()
        {
            foreach (var task in _workTasks)
                if (task.enabled)
                    return true;

            return false;
        }

        private ITask[] GenerateProbabilities()
        {
            var list = new List<ITask>();
            foreach (var task in _workTasks)
                if(task.IsAvailable())
                    for(int i = 0; i< task.chanceValue; ++i)
                        list.Add(task);
            return list.ToArray();
        }

        public override void ReceiveValue(float value, Vector3 takePosition) { }
    }
}
