using ClickNext.Scripts.Animations;
using ClickNext.Scripts.Items.Weapons;
using ClickNext.Scripts.Movements.Tasks.Interfaces;
using ClickNext.Scripts.Units;
using UnityEngine;

namespace ClickNext.Scripts.Movements.Tasks
{
    [RequireComponent(typeof(Worker))]
    public class FarmTask : GeneralTask
    {
        public override bool IsAvailable() => 
            AnyTarget(TagType.Farm) &&
            FindItem<Tool>(WorkerAnimation.WATERING, type => type == Tools.Pot);

        private new void OnEnable()
        {
            _workerAnimation.SetTool();
            base.OnEnable();
        }
    }
}
