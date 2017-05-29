using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyTracking
{
    public class FulfillmentRequest
    {
        public string Tienda { get; set; } = string.Empty;
        /// <summary>
        /// The date and time when the fulfillment was created. The API returns this value in ISO 8601 format.
        /// </summary>
        public string created_at { get; set; } = string.Empty;
        /// <summary>
        /// A unique numeric identifier for the fulfillment.
        /// </summary>
        public string id { get; set; } = string.Empty;
        /// <summary>
        /// A flag indicating whether the customer should be notified. If set to  true, an email will be sent when the fulfillment is created or updated. The default value is true. If you don't specify a value, the customer will receive an email.
        /// </summary>
        public bool notify_customer { get; set; } = false;
        /// <summary>
        /// The unique numeric identifier for the order.
        /// </summary>
        public string order_id { get; set; } = string.Empty;
        /// <summary>
        /// pending: The fulfillment is pending.
        /// open: The fulfillment has been acknowledged by the service and is in processing.
        /// success: The fulfillment was successful.
        /// cancelled: The fulfillment was cancelled.
        /// error: There was an error with the fulfillment request.
        /// failure: The fulfillment request failed.
        /// </summary>
        public string status { get; set; } = string.Empty;
        /// <summary>
        /// 'The name of the tracking company.'
        /// 4PX
        /// APC
        /// Amazon Logistics UK
        /// Amazon Logistics US
        /// Australia Post
        /// Bluedart
        /// Canada Post
        /// China Post
        /// DHL
        /// DHL eCommerce
        /// DHL eCommerce Asia
        /// Delhivery
        /// Eagle
        /// FSC
        /// FedEx
        /// FedEx UK
        /// GLS
        /// Globegistics
        /// Japan Post
        /// New Zealand Post
        /// PostNord
        /// Purolator
        /// Royal Mail
        /// Sagawa
        /// TNT
        /// TNT Post
        /// UPS
        /// USPS
        /// Yamato
        /// </summary>
        public string tracking_company { get; set; } = string.Empty;
        /// <summary>
        /// The URL to track the fulfillment.
        /// </summary>
        public string tracking_url { get; set; } = string.Empty;
        /// <summary>
        /// tracking numbers, provided by the shipping company.
        /// </summary>
        public string tracking_number { get; set; } = string.Empty;
    }
}
