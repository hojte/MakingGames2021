using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public List<ShopItemController> shopItems;
    // Start is called before the first frame update
    void Start()
    {
        shopItems = GetComponentsInChildren<ShopItemController>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
