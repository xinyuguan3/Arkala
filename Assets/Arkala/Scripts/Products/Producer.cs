using ClickNext.Scripts.Items;
using ClickNext.Scripts.Products.Interfaces;
using ClickNext.Scripts.Units;
using UnityEngine;

namespace ClickNext.Scripts.Products
{
    public class Producer : MonoBehaviour
    {
        public int OrdersCount { get; private set; }
        public float RemainTime { get; private set; }
        public Ingredient[] Ingredients { get { return ingredients; } }
        public string ProductName { get { return rawMaterial.materialType.ToString(); } }

        [SerializeField] private Ingredient[] ingredients;
        [SerializeField] private float duration;
        [SerializeField] private RawMaterial rawMaterial;
        [SerializeField] private IPlayer processPlayer;

        private void Awake()
        {
            rawMaterial.gameObject.SetActive(false);
            RemainTime = duration;
        }

        private void Update()
        {
            if (OrdersCount <= 0)
                return;

            RemainTime -= Time.deltaTime;
            if(RemainTime <= 0)
            {
                RemainTime = duration;
                OrdersCount--;
                SpawnMaterial();
            }
        }

        protected void SpawnMaterial()
        {
            if (processPlayer && OrdersCount <= 0)
                processPlayer.StopProcess();

            var item = Instantiate(rawMaterial,
                rawMaterial.transform.position,
                rawMaterial.transform.rotation);
            item.gameObject.SetActive(true);
        }

        public int Order()
        {
            if (Storage.Withdraw(ingredients))
            {
                OrdersCount++;

                if(processPlayer)
                    processPlayer.RunProcess();
            }

            return OrdersCount;
        }
    }
}
