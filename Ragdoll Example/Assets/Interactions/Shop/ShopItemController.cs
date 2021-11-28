using System;
using TMPro;
using UI;
using UnityEngine;

namespace Interactions.Shop
{
    public class ShopItemController : MonoBehaviour
    {
        private ShopController _shopController;
        private ScoreController _scoreController;
        private TextMeshPro _textMeshPrice;
        private TextMeshPro _textMeshDescription;
        public Pickup pickup;
        private int _index = -1;

        private void Awake()
        {
            _shopController = GetComponentInParent<ShopController>();
            _scoreController = FindObjectOfType<ScoreController>();
            _textMeshDescription = transform.Find("ItemDescription").GetComponent<TextMeshPro>();
            _textMeshPrice = transform.Find("ItemCost").GetComponent<TextMeshPro>();
        }

        public void SetShopItem(Pickup setPickup, int price)
        {
            pickup = setPickup;
            if (pickup == null || pickup.pickupType == PickupType.None)
            {
                _textMeshDescription.text = "";
                _textMeshPrice.text = "";
                return;
            }
            _textMeshDescription.text = pickup.pickupType.ToString();
            _textMeshPrice.text = price.ToString();
        }
        public void BuyPickup()
        {
            if(_scoreController == null) _scoreController = FindObjectOfType<ScoreController>();

            if(pickup == null) return;
            if (pickup.pickupType == PickupType.None) return;
            
            if (!_scoreController.BuyAmount(int.Parse(_textMeshPrice.text))) return; // player can't afford the item
            pickup.OnPickup();
        
            if (_index == 0)
            {
                _shopController.pickupToSellInSlot1 = PickupType.None;
            }
            else if(_index == 1)
            {
                _shopController.pickupToSellInSlot2 = PickupType.None;
            }
            else if (_index == 2)
            {
                _shopController.pickupToSellInSlot3 = PickupType.None;
            }
            SetShopItem(null, 0);
        }

        public void SetIndex(int index)
        {
            _index = index;
        }
    }
}
