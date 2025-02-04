using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPManager : IStoreListener, IDetailedStoreListener
{
    public static IAPManager instance {  get; private set; }
    IAPManager()
    {
        instance = this;
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct("100coins", ProductType.Consumable, new IDs
        {
            {"100coins", GooglePlay.Name}
        });

        UnityPurchasing.Initialize(this, builder);
    }
    public IStoreController controller;
    public bool initialised;
    public void OnInitialized(IStoreController _controller, IExtensionProvider extensions)
    {
        controller = _controller;
        initialised = true;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        //throw new System.NotImplementedException();
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        throw new System.NotImplementedException();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        throw new System.NotImplementedException();
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        //throw new System.NotImplementedException();
        return new();
    }
}
