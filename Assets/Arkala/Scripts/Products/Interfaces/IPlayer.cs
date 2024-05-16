using UnityEngine;

namespace ClickNext.Scripts.Products.Interfaces
{
    public abstract class IPlayer: MonoBehaviour
    {
        public abstract void RunProcess();
        public abstract void StopProcess();
    }
}
