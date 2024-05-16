using ClickNext.Scripts.Units;
using System;
using UnityEngine;
namespace ClickNext.Scripts.UI.Managers
{
    public class IconManager : MonoBehaviour
    {
        [SerializeField] Icon iconTemplate;

        private Icon[] icons;

        void Awake()
        {
            iconTemplate.gameObject.SetActive(false);

            //Generate icons from template;
            var types = Enum.GetNames(typeof(MaterialType));
            icons = new Icon[types.Length];

            for(int i = 0; i< icons.Length; ++i)
            {
                icons[i] = Instantiate(iconTemplate, transform);
                icons[i].ItemImage.sprite = Resources.Load<Sprite>($"Icons/{types[i]}");
            }
        }

        private void FixedUpdate()
        {
            for(int i = 0; i < icons.Length; ++i)
            {
                var count = Storage.RawMaterials[i];
                icons[i].CountText.text = count.ToString();
                icons[i].gameObject.SetActive(count > 0);
            }
        }
    }
}
