using ClickNext.Scripts.Animations;
using ClickNext.Scripts.Items.Weapons;
using ClickNext.Scripts.Movements.Tasks.Interfaces;
using ClickNext.Scripts.Units;
using UnityEngine;

namespace ClickNext.Scripts.Movements.Tasks
{
    [RequireComponent(typeof(Worker))]
    public class CutTask : GeneralTask
    {
        public override bool IsAvailable() =>
            AnyTarget(TagType.Tree) && 
            FindItem<Tool>(WorkerAnimation.CUT, type => type == Tools.Hatchet);

        private new void OnEnable()
        {
            _workerAnimation.SetTool();
            base.OnEnable();
        }
    }
}
