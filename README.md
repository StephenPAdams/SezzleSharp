# SezzleSharp
A Sezzle C# SDK

This library is still in development. This SDK will wire up the following endpoints and features from the Sezzle Pay API:

* Authentication POST https://gateway.sezzle.com/v1/authentication - DONE
* Configuration POST https://gateway.sezzle.com/v1/configuration - DONE
* Checkouts (Create) POST https://gateway.sezzle.com/v1/checkouts - DONE*
* Checkouts (Complete) POST https://gateway.sezzle.com/v1/checkouts/{order_reference_id}/complete - DONE*
* Orders (Details) GET https://gateway.sezzle.com/v1/orders/{order_reference_id} - PENDING TESTS
* Orders (Refund) POST https://gateway.sezzle.com/v1/orders/{order_reference_id}/refund - PENDING TESTS
* Order Webhook modeling - PENDING TESTS

In addition, there will be appropriate model(s) for the Order Webhook format that is supported by the Sezzle Pay API. 

*Pending Sezzle developer support giving information regarding why the shipping address isn't being populated on the ORders (Details) call. Need to ensure it's being saved on checkout create appropriately.
