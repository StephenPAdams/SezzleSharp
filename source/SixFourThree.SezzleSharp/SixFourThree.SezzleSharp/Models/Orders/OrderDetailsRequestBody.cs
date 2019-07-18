namespace SixFourThree.SezzleSharp.Models.Orders
{
    public class OrderDetailsRequestBody
    {
        //this is the only parameter in their interface that is serialized with dashes instead of snake-case
        public bool IncludeShippingInfo { get; set; }
    }
}
