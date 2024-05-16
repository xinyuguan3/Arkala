using ClickNext.Scripts.UI.Managers.Interfaces;
using ClickNext.Scripts.UI.Managers.Templates;
using ClickNext.Scripts.Units;
using UnityEngine;
using UnityEngine.UI;

namespace ClickNext.Scripts.UI.Managers
{
    //管理所有的Meme
    public class MemeManager : IManager
    {
        [SerializeField] Color[] colors;
        private new void Awake()
        {
            base.Awake();
        }

        void Start(){
            gameObject.SetActive(false);
        }

        //将控制的面板设置为相反的可见状态
        public override void OpenDialog(bool value)
        {
            gameObject.SetActive(!gameObject.activeSelf);
            // base.OpenDialog(value);
        }
    }
}