/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyIAPManager (version 1.1)
 */

using System;
using UnityEngine;
using UnityEngine.Purchasing;
using MyClasses;

public class MyIAPManager : MonoBehaviour, IStoreListener
{
    #region ----- Variable -----

    private static IStoreController mStoreController;
    private static IExtensionProvider mStoreExtensionProvider;
    private string mPurchasingProductID;

    private Action mOnInitializationSuccessCallback;
    private Action<InitializationFailureReason> mOnInitializationFailureCallback;
    private Action<string> mOnPurchaseSuccessCallback;
    private Action<Product, PurchaseFailureReason> mOnPurchaseFailureCallback;

    #endregion

    #region ----- Property -----

    public bool IsInitialized
    {
        get { return mStoreController != null && mStoreExtensionProvider != null; }
    }

    public bool IsNeedInitialization
    {
        get; set;
    }

    public Product[] Products
    {
        get { return mStoreController != null ? mStoreController.products.all : null; }
    }

    #endregion

    #region ----- Singleton -----

    private static object mSingletonLock = new object();
    private static MyIAPManager mInstance;

    public static MyIAPManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                lock (mSingletonLock)
                {
                    mInstance = (MyIAPManager)FindObjectOfType(typeof(MyIAPManager));
                    if (mInstance == null)
                    {
                        GameObject obj = new GameObject(typeof(MyIAPManager).Name);
                        mInstance = obj.AddComponent<MyIAPManager>();
                        DontDestroyOnLoad(obj);
                    }
                }
            }
            return mInstance;
        }
    }

    #endregion

    #region ----- Implement IStoreListener -----

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
#if DEBUG_MY_IAP || UNITY_EDITOR
        Debug.Log(string.Format("[" + typeof(MyIAPManager).Name + "] OnInitialized()"));
#endif

        mStoreController = controller;
        mStoreExtensionProvider = extensions;

        mStoreExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions(result =>
        {
            if (result)
            {
#if DEBUG_MY_IAP || UNITY_EDITOR
                Debug.Log(string.Format("[" + typeof(MyIAPManager).Name + "] OnInitialized(): restoration process succeeded"));
#endif

                if (mOnInitializationSuccessCallback != null)
                {
                    mOnInitializationSuccessCallback();
                    mOnInitializationSuccessCallback = null;
                }
            }
            else
            {
#if DEBUG_MY_IAP || UNITY_EDITOR
                Debug.LogError(string.Format("[" + typeof(MyIAPManager).Name + "] OnInitialized(): restoration process failed"));
#endif
            }
        });
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
#if DEBUG_MY_IAP || UNITY_EDITOR
        Debug.Log(string.Format("[" + typeof(MyIAPManager).Name + "] OnInitializeFailed(): error=", error));
#endif

        if (mOnInitializationFailureCallback != null)
        {
            mOnInitializationFailureCallback(error);
            mOnInitializationFailureCallback = null;
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
#if DEBUG_MY_IAP || UNITY_EDITOR
        Debug.LogError(string.Format("[" + typeof(MyIAPManager).Name + "] OnPurchaseFailed(): storeSpecificId={0} | failureReason={1}", product.definition.storeSpecificId, failureReason));
#endif

        mPurchasingProductID = null;

        if (mOnPurchaseFailureCallback != null)
        {
            mOnPurchaseFailureCallback(product, failureReason);
            mOnPurchaseFailureCallback = null;
        }
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (String.Equals(args.purchasedProduct.definition.id, mPurchasingProductID, StringComparison.Ordinal))
        {
#if DEBUG_MY_IAP || UNITY_EDITOR
            Debug.Log(string.Format("[" + typeof(MyIAPManager).Name + "] ProcessPurchase(): receipt={0}", args.purchasedProduct.definition.id));
#endif

            if (mOnPurchaseSuccessCallback != null)
            {
                mOnPurchaseSuccessCallback(args.purchasedProduct.receipt);
                mOnPurchaseSuccessCallback = null;
            }
        }
        else
        {
#if DEBUG_MY_IAP || UNITY_EDITOR
            Debug.LogError(string.Format("[" + typeof(MyIAPManager).Name + "] ProcessPurchase(): unrecognized product {0}", args.purchasedProduct.definition.id));
#endif
        }

        mPurchasingProductID = null;

        return PurchaseProcessingResult.Complete;
    }

    #endregion

    #region ----- Public Method -----

    public void Initialize(string[] consumablePackages, string[] nonComsumablePackages, string[] subscriptionPackages, Action onSuccessCallback, Action<InitializationFailureReason> onFailureCallback)
    {
#if DEBUG_MY_IAP || UNITY_EDITOR
        Debug.Log(string.Format("[" + typeof(MyIAPManager).Name + "] Initialize(): consumablePackages={0} | nonComsumablePackages={1} | subscriptionPackages={2}", consumablePackages, nonComsumablePackages, subscriptionPackages));
#endif

        mOnInitializationSuccessCallback = onSuccessCallback;
        mOnInitializationFailureCallback = onFailureCallback;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        if (consumablePackages != null)
        {
            for (int i = 0; i < consumablePackages.Length; i++)
            {
                builder.AddProduct(consumablePackages[i], ProductType.Consumable);
            }
        }
        if (nonComsumablePackages != null)
        {
            for (int i = 0; i < nonComsumablePackages.Length; i++)
            {
                builder.AddProduct(nonComsumablePackages[i], ProductType.NonConsumable);
            }
        }
        if (subscriptionPackages != null)
        {
            for (int i = 0; i < subscriptionPackages.Length; i++)
            {
                builder.AddProduct(subscriptionPackages[i], ProductType.Subscription);
            }
        }

        UnityPurchasing.Initialize(this, builder);
    }

    public void Purchase(string productId, Action<string> onSuccessCallback, Action<Product, PurchaseFailureReason> onFailureCallback)
    {
#if DEBUG_MY_IAP || UNITY_EDITOR
        Debug.Log(string.Format("[" + typeof(MyIAPManager).Name + "] Purchase(): productId=", productId));
#endif

        try
        {
            if (!IsInitialized)
            {
#if DEBUG_MY_IAP || UNITY_EDITOR
                Debug.LogError(string.Format("[" + typeof(MyIAPManager).Name + "] Purchase(): not initialized yet"));
#endif
                return;
            }

            Product product = mStoreController.products.WithID(productId);
            if (product == null || !product.availableToPurchase)
            {
#if DEBUG_MY_IAP || UNITY_EDITOR
                Debug.LogError(string.Format("[" + typeof(MyIAPManager).Name + "] Purchase(): either is not found or is not available for purchase"));
#endif
            }

            if (product != null && product.availableToPurchase)
            {
                mPurchasingProductID = productId;
                mOnPurchaseSuccessCallback = onSuccessCallback;
                mOnPurchaseFailureCallback = onFailureCallback;

                mStoreController.InitiatePurchase(product);
            }
        }
        catch (Exception e)
        {
#if DEBUG_MY_IAP || UNITY_EDITOR
            Debug.LogError(string.Format("[" + typeof(MyIAPManager).Name + "] Purchase(): exception={0}", e));
#endif
        }
    }

    public void ConfirmPendingPurchase(string productId)
    {
#if DEBUG_MY_IAP || UNITY_EDITOR
        Debug.Log(string.Format("[" + typeof(MyIAPManager).Name + "] ConfirmPendingPurchase(): productId={0}", productId));
#endif

        Product product = mStoreController.products.WithID(productId);
        mStoreController.ConfirmPendingPurchase(product);
    }

    public void RestorePurchases()
    {
        if (!IsInitialized)
        {
#if DEBUG_MY_IAP || UNITY_EDITOR
            Debug.LogError(string.Format("[" + typeof(MyIAPManager).Name + "] ConfirmPendingPurchase(): not initialized"));
#endif
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
#if DEBUG_MY_IAP || UNITY_EDITOR
            Debug.Log(string.Format("[" + typeof(MyIAPManager).Name + "] RestorePurchases()"));
#endif

            mStoreExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions((result) =>
            {
#if DEBUG_MY_IAP || UNITY_EDITOR
                Debug.Log(string.Format("[" + typeof(MyIAPManager).Name + "] RestorePurchases(): result={0}", result));
#endif
            });
        }
        else
        {
#if DEBUG_MY_IAP || UNITY_EDITOR
            Debug.LogError(string.Format("[" + typeof(MyIAPManager).Name + "] RestorePurchases(): not supported on this platform"));
#endif
        }
    }

    #endregion
}