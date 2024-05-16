using ClickNext.Scripts.Items.Weapons;
using ClickNext.Scripts.UI.Managers.Templates;
using ClickNext.Scripts.Units.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace ClickNext.Scripts.UI.Managers
{
    public class CommandManager : MonoBehaviour
    {
        [SerializeField] LabelTemplate labelTemplate;        
        public GameObject currentAgent; 
        public GameObject AgentPanel;
        private RaycastManager raycastManager;
        private readonly Dictionary<Unit, LabelTemplate> labels = new Dictionary<Unit, LabelTemplate>();

        void Awake()
        {
            raycastManager = FindObjectOfType<RaycastManager>();
            labelTemplate.gameObject.SetActive(false);
        }

        private void OnEnable() => raycastManager.OnHit += OnHit;
        private void OnDisable() =>  raycastManager.OnHit -= OnHit;

        void AddCommandLabel(Unit unit)
        {
            if (unit.NameTag)
            {
                string item = 
                    unit is Units.Tree? Tools.Hatchet.ToString() :
                    unit is Units.Rock? Tools.Pickaxe.ToString() : 
                    unit is Units.Animal? Weapons.Bow.ToString() : 
                    // unit is Units.Worker? 
                    string.Empty;

                var label = Instantiate(labelTemplate, transform);
                    label.SetTask(item.ToString(), unit.NameTag);
                    label.gameObject.SetActive(true);
                labels.Add(unit, label);

                unit.OnHpEmpty += RemoveCommandLabel;
                unit.SetAllow(true);
            }
        }

        private void RemoveCommandLabel(Unit unit)
        {
            if (!labels.ContainsKey(unit))
                return;

            unit.SetAllow(false);

            var label = labels[unit];
            if(label)
                Destroy(label.gameObject);
            labels.Remove(unit);
        }

        private void OnHit(GameObject hitObject, Vector3 position)
        {
            // 点击到的是Unit
            var unit = hitObject.GetComponent<Unit>();
            if (unit) {
                
                if(unit is Units.Worker){
                    //打开小人信息面板
                    //TODO: 传入当前点击的小人
                    // OpenAgentPanel();
                    //切换当前操控小人
                    currentAgent=hitObject;
                }

                //用以分辨点击到的是人还是物，暂时废弃
                    // string unitName = 
                    // unit is Units.Worker? "Worker" :
                    // unit is Units.Thing? "Thing" : 
                    // string.Empty;


                if (!labels.ContainsKey(unit)) 
                {
                    // 加上标签
                    AddCommandLabel(unit);
                }
                else
                {
                    // 移除标签
                    RemoveCommandLabel(unit);
                }

            }

            //点击到地面
            //指挥小人行走逻辑
            if(hitObject.layer==LayerMask.NameToLayer("Ground")){
                if(currentAgent!=null){
                    currentAgent.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(position);
                    ClickManager.Instance.ClickVFX();
                }
            }
        }

        private void OpenAgentPanel()
        {
            
        }
    }
}
