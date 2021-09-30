using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Domain
{
    public class StripeSettings
    {
        public string SecretKey { get; set; }
        public string PublishableKey { get; set; }
    }
}
