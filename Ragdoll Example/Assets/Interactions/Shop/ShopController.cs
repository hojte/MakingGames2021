using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Interactions.Shop
{
    public class ShopController : MonoBehaviour
    {
        public List<ShopItemController> shopItems;
        void Start()
        {
            shopItems = GetComponentsInChildren<ShopItemController>().ToList();
        }
    }
}
