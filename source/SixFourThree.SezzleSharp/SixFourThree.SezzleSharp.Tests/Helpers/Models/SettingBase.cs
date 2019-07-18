using System;

namespace SixFourThree.SezzleSharp.Tests.Helpers.Models
{
    public class SettingBase
    {
        public TimeSpan PageTimeout { get; set; }
        public TimeSpan PollInterval { get; set; }
        public int MaxPollCount => Convert.ToInt32(Math.Round(PageTimeout.TotalMilliseconds / PollInterval.TotalMilliseconds, 0));

    }
}