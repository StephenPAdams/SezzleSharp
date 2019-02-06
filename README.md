# SezzleSharp
A Sezzle C# SDK

[![Build status](https://ci.appveyor.com/api/projects/status/5yixr8hm3n33ej67?svg=true)](https://ci.appveyor.com/project/StephenPAdams/sezzlesharp)
[![NuGet Version](https://img.shields.io/nuget/v/SixFourThree.SezzleSharp.svg?style=flat-square)](https://www.nuget.org/packages/SixFourThree.SezzleSharp)

This library is still in development. This SDK will wire up the following endpoints and features from the Sezzle Pay API:

* Authentication POST https://gateway.sezzle.com/v1/authentication - DONE
* Configuration POST https://gateway.sezzle.com/v1/configuration - DONE
* Checkouts (Create) POST https://gateway.sezzle.com/v1/checkouts - DONE
* Checkouts (Complete) POST https://gateway.sezzle.com/v1/checkouts/{order_reference_id}/complete DONE
* Orders (Details) GET https://gateway.sezzle.com/v1/orders/{order_reference_id} - DONE
* Orders (Refund) POST https://gateway.sezzle.com/v1/orders/{order_reference_id}/refund - DONE
* Order Webhook modeling - PENDING TESTS

*Pending Sezzle developer support giving information regarding why the shipping address isn't being populated on the Orders (Details) call. Need to ensure it's being saved on checkout create appropriately.

# Sandbox
If you’re using Sezzle's sandbox, you’ll need to enter 123123 when doing a checkout (it doesn’t support customer specific phone numbers for text verification…probably for the better). Per their support:

For more information about the Sezzle sandbox, please see documentation located here: https://docs.sezzle.com/#sandbox

# Customer Shipping Address Notes
Setting requires_shipping_info to true in the Checkouts endpoint will show a shipping address UI 
in the Sezzle checkout process. The shipping_address is populated from the Orders (detail) endpoint when include_shipping_info is set to true for orders that Sezzle collects the shipping address (when requires_shipping_info is true).

However, in the event that the shipping address is collected outside of the Sezzle site and is passed over as a parameter in the shipping_address field in the Checkouts endpoint with requires_shipping_info to true, when calling the Orders (detail) endpoint with include_shipping_info set to false, the shipping_address field has all empty values and not the values that were sent in the Checkouts endpoint. This is apparently by design per Sezzle developer support.