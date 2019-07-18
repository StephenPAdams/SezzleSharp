namespace SixFourThree.SezzleSharp.Models.Checkouts
{
    public class CompleteCheckoutRequest
    {
        /// <summary>
        /// This is YOUR OrderId, not Sezzle's
        /// </summary>
        public string LocalOrderId { get; set; }
    }
}
