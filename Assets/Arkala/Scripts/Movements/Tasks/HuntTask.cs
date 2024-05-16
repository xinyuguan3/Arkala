using ClickNext.Scripts.Animations;
using ClickNext.Scripts.Items.Weapons;
using ClickNext.Scripts.Movements.Tasks.Interfaces;
using ClickNext.Scripts.Units;
using UnityEngine;

namespace ClickNext.Scripts.Movements.Tasks
{
    [RequireComponent(typeof(Worker))]
    public class HuntTask : GeneralTask
    {
        public override bool IsAvailable() => 
            AnyTarget(TagType.Animal) && 
            FindItem<Bow>(WorkerAnimation.ATTACK);

        private new void OnEnable()
        {
            _workerAnimation.SetHunting();
            base.OnEnable();
        }
    }
}
