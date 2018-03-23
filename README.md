# SezzleSharp
A Sezzle C# SDK

This library is still in development. This SDK will wire up the following endpoints and features from the Sezzle Pay API:

* Authentication POST https://gateway.sezzle.com/v1/authentication - DONE
* Configuration POST https://gateway.sezzle.com/v1/configuration - DONE
* Checkouts (Create) POST https://gateway.sezzle.com/v1/checkouts - DONE*
* Checkouts (Complete) POST https://gateway.sezzle.com/v1/checkouts/{order_reference_id}/complete - DONE*
* Orders (Refund) POST https://gateway.sezzle.com/v1/orders/{order_reference_id}/refund - INCOMPLETE
* Order Webhook modeling - INCOMPLETE

In addition, there will be appropriate model(s) for the Order Webhook format that is supported by the Sezzle Pay API. 

*Pending Sezzle developer supports information regarding order items showing in the Sezzle Merchant Dashboard. Just need to confirm this data is surfacing appropriately in their system.
