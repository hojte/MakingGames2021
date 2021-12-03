using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace Interactions.Shop
{
    public class ShopController : MonoBehaviour
    {
        public List<ShopItemController> shopItems;
        public PickupType pickupToSellInSlot1 = PickupType.Random;
        public PickupType pickupToSellInSlot2 = PickupType.Random;
        public PickupType pickupToSellInSlot3 = PickupType.Random;
        public GameObject pickupPrefab;
        public int discountOnItems;
        void Start()
        {
            shopItems = GetComponentsInChildren<ShopItemController>().ToList();
            
            shopItems[0].SetIndex(0);
            shopItems[1].SetIndex(1);
            shopItems[2].SetIndex(2);
            
            FillSlots();
        }

        void FillSlots()
        {
            Array values = Enum.GetValues(typeof(PickupType));
            Random random = new Random();
            if (pickupToSellInSlot1 == PickupType.Random) pickupToSellInSlot1 = (PickupType)values.GetValue(random.Next(2, values.Length));
            if (pickupToSellInSlot2 == PickupType.Random) pickupToSellInSlot2 = (PickupType)values.GetValue(random.Next(2, values.Length));
            if (pickupToSellInSlot3 == PickupType.Random) pickupToSellInSlot3 = (PickupType)values.GetValue(random.Next(2, values.Length));
            
            // GameObject pickupPrefab =
            //     Resources.Load<GameObject>("Prefabs/PickupItem");
            
            var tmpPickup = Instantiate(pickupPrefab).GetComponent<Pickup>();
            tmpPickup.RemoveVisuals();
            tmpPickup.useInstantly = false;
            tmpPickup.pickupType = pickupToSellInSlot1;
            shopItems[0].SetShopItem(tmpPickup, tmpPickup.ShopPrice - discountOnItems);
            
            tmpPickup = Instantiate(pickupPrefab).GetComponent<Pickup>();
            tmpPickup.RemoveVisuals();
            tmpPickup.useInstantly = false;
            tmpPickup.pickupType = pickupToSellInSlot2;
            shopItems[1].SetShopItem(tmpPickup, tmpPickup.ShopPrice - discountOnItems);
            
            tmpPickup = Instantiate(pickupPrefab).GetComponent<Pickup>();
            tmpPickup.RemoveVisuals();
            tmpPickup.useInstantly = false;
            tmpPickup.pickupType = pickupToSellInSlot3;
            shopItems[2].SetShopItem(tmpPickup, tmpPickup.ShopPrice - discountOnItems);
        }
    }
}
