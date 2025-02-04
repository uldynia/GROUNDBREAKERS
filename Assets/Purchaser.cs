using UnityEngine;

public class Purchaser : MonoBehaviour
{
    public void Purchase(string productID)
    {
        IAPManager.instance.controller.InitiatePurchase(productID);
    }
}
