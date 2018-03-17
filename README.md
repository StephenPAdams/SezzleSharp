# SezzleSharp
A Sezzle C# SDK

This library is still in development. This SDK will wire up the following endpoints and features from the Sezzle Pay API:

* Authentication POST https://gateway.sezzle.com/v1/authentication - DONE
* Configuration POST https://gateway.sezzle.com/v1/configuration - INCOMPLETE
* Checkouts (Create) POST https://gateway.sezzle.com/v1/checkouts - DONE
* Checkouts (Complete) POST https://gateway.sezzle.com/v1/checkouts/{order_reference_id}/complete - PENDING
* Orders (Refund) POST https://gateway.sezzle.com/v1/orders/{order_reference_id}/refund - INCOMPLETE
* Order Webhook modeling - INCOMPLETE

In addition, there will be appropriate model(s) for the Order Webhook format that is supported by the Sezzle Pay API. 
