using ClickNext.Scripts.Animations;
using ClickNext.Scripts.Items.Weapons;
using ClickNext.Scripts.Movements.Tasks.Interfaces;
using ClickNext.Scripts.Units;
using UnityEngine;

namespace ClickNext.Scripts.Movements.Tasks
{
    [RequireComponent(typeof(Worker))]
    public class BuildTask : GeneralTask
    {
        public override bool IsAvailable() =>
            AnyTarget(TagType.Building) &&
            FindItem<Tool>(WorkerAnimation.BUILD, type => type == Tools.Hammer);

        private new void OnEnable()
        {
            _workerAnimation.SetTool();
            base.OnEnable();
        }
    }
}
