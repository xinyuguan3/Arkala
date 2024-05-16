using ClickNext.Scripts.Animations;
using ClickNext.Scripts.Items.Weapons;
using ClickNext.Scripts.Movements.Tasks.Interfaces;
using ClickNext.Scripts.Units;
using UnityEngine;

namespace ClickNext.Scripts.Movements.Tasks
{
    [RequireComponent(typeof(Worker))]
    public class MineTask : GeneralTask
    {
        public override bool IsAvailable() => 
            AnyTarget(TagType.Rock) && 
            FindItem<Tool>(WorkerAnimation.MINING, type => type == Tools.Pickaxe);

        private new void OnEnable()
        {
            _workerAnimation.SetTool();
            base.OnEnable();
        }
    }
}
