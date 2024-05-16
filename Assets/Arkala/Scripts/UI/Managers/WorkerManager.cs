using ClickNext.Scripts.UI.Managers.Interfaces;
using ClickNext.Scripts.UI.Managers.Templates;
using ClickNext.Scripts.Units;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClickNext.Scripts.UI.Managers
{
    public class WorkerManager : IManager
    {
        [SerializeField] bool initWorkers;

        [Header("Common UI")]
        [SerializeField] int maximumWorkers;
        [SerializeField] TMP_Text limitText;

        [Header("UI worker list")]
        [SerializeField] GameObject panel;
        [SerializeField] Button addButton;
        [SerializeField] WorkerTemplate workerTemplate;
        [SerializeField] PrioritySelector prioritySelector;

        [Header("Worker Prefabs")]
        [SerializeField] Transform spawnPoint;
        [SerializeField] TaggerTemplate taggerTemplate;
        [SerializeField] Worker[] workerPrefabs;

        private readonly List<Worker> workerList = new List<Worker>();
        private Names boyNames;
        private Names girlNames;

        protected new void Awake()
        {
            base.Awake();

            // load json files.
            boyNames = JsonUtility.FromJson<Names>(Resources.Load("Json/Names Boy").ToString());
            girlNames = JsonUtility.FromJson<Names>(Resources.Load("Json/Names Girl").ToString());

            prioritySelector.Hide();
            workerTemplate.gameObject.SetActive(false);
            taggerTemplate.gameObject.SetActive(false);

            OpenDialog(false);

            addButton.onClick.AddListener(AddWorker);

            FindWorkers();

            // //generate a worker
            // if (initWorkers)
            //     AddWorker();
        }

        public override void OpenDialog(bool value)
        {
            prioritySelector.Hide();
            panel.SetActive(value);
            base.OpenDialog(value);
        }

        private void FindWorkers()
        {
            var workers = FindObjectsOfType<Worker>();
            foreach (var worker in workers)
                UpdateWorker(worker);
        }

        // instantiate 2D UI.
        private void UpdateWorker(Worker worker)
        {
            // random name from json file.
            var workerName = worker.GenderType == Worker.Gender.Male ? boyNames.names[UnityEngine.Random.Range(0, boyNames.names.Length)] :
                    girlNames.names[UnityEngine.Random.Range(0, girlNames.names.Length)];

            var tagger = Instantiate(taggerTemplate, taggerTemplate.transform.parent);
                tagger.AttachedWorker = worker.NameTag;
                tagger.DisplayName.text = workerName;
                tagger.gameObject.SetActive(true);

            var workerUI = Instantiate(workerTemplate, workerTemplate.transform.parent, true);
                workerUI.DisplayName.text = workerName;
                workerUI.RemoveButton.onClick.AddListener(() => 
                {
                    RemoveWorker(tagger, workerUI, worker);
                    limitText.text = $"{workerList.Count}/{maximumWorkers}";
                });
                workerUI.AssignPriorities(worker, prioritySelector);
                workerUI.gameObject.SetActive(true);
            workerList.Add(worker);

            limitText.text = $"{workerList.Count}/{maximumWorkers}";
        }

        // instantiate 3D worker character.
        private void AddWorker()
        {
            prioritySelector.Hide();

            if (workerList.Count >= maximumWorkers)
                return;

            //random gender
            var gender = UnityEngine.Random.Range(0, workerPrefabs.Length);
            var worker = Instantiate(workerPrefabs[gender]);
                worker.transform.position = spawnPoint.position;
                worker.transform.SetParent(spawnPoint.parent);
            UpdateWorker(worker);
        }

        private void RemoveWorker(TaggerTemplate tagger, WorkerTemplate workerUI, Worker worker)
        {
            prioritySelector.Hide();

            workerList.Remove(worker);

            Destroy(tagger.gameObject);
            Destroy(workerUI.gameObject);
            Destroy(worker.gameObject);
        }
    }

    [Serializable]
    public struct Names
    {
        public string[] names;
    }
}
