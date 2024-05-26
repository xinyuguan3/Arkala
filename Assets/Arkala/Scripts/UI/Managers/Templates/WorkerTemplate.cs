using ClickNext.Scripts.Movements.Tasks.Interfaces;
using ClickNext.Scripts.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClickNext.Scripts.UI.Managers.Templates
{
    public class WorkerTemplate : MonoBehaviour
    {
        public TMP_Text DisplayName;
        public Button RemoveButton;
        [SerializeField] private TaskPriority taskPriorityTemplate;
        private void Awake() =>
            taskPriorityTemplate.gameObject.SetActive(false);

        public void AssignPriorities(Worker worker, PrioritySelector prioritySelector)
        {
            var tasks = worker.GetComponents<ITask>();
            foreach (var task in tasks)
            {
                task.chanceValue = Random.Range(1, 10);
                var priority = Instantiate(taskPriorityTemplate, taskPriorityTemplate.transform.parent, true);
                priority.PriorityDisplay.text = task.chanceValue.ToString();
                priority.PriorityButton.onClick.AddListener(() => prioritySelector.Show(
                    value: task.chanceValue, 
                    position: priority.transform.position, 
                    value => 
                    {
                        task.chanceValue = value;
                        priority.PriorityDisplay.text = value.ToString();
                    }));
                priority.gameObject.SetActive(true);
            }
        }
    }
}
