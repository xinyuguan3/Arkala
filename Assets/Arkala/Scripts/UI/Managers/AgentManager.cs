using ClickNext.Scripts.UI.Managers.Interfaces;
using ClickNext.Scripts.UI.Managers.Templates;
using ClickNext.Scripts.Units;
using UnityEngine;
using UnityEngine.UI;

namespace ClickNext.Scripts.UI.Managers
{
    public class AgentManager : IManager
    {
        [SerializeField]
        [Multiline(5)]
        public string[] memories;
        public MemoryTemplate memoryTemplate;

        private new void Awake()
        {
            base.Awake();
            
        }

        void Start(){
            gameObject.SetActive(false);
        }

        private void UpdateMemory()
        {
            foreach (var memory in memories)
            {
                var memoryBubble = Instantiate(memoryTemplate, memoryTemplate.transform.parent, true);
                memoryBubble.MemoryText.text = memory;
                // memoryBubble.RecentTime.text = System.DateTime.Now.ToString("HH:mm:ss");
                memoryBubble.RecentTime.text = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            
        }

        //将控制的面板设置为相反的可见状态
        public override void OpenDialog(bool value)
        {
            gameObject.SetActive(!gameObject.activeSelf);
            // base.OpenDialog(value);
        }
    }
}
