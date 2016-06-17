using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

/// <summary>
/// Class and functions to send ecommerce transactions and items to Google Analytics using  Measurement Protocol
/// 
/// Usage:
/// 
///  GoogleMeasurementProtocol gmp = new GoogleMeasurementProtocol();
///        gmp.SendGoogleTrackingOrder("UA-000000-00", 
///        "12345", 
///        "0005", 
///        "SiteName", 
///        "19.99", 
///        "5.99", 
///        "mysite.co.uk",
///        "callback.aspx", 
///        "Paypal Payment Complete");
///            
///        // now send items.
///        foreach (DataRow row in DataTable)
///        {
///            gmp.SendGoogleTrackingItem(
///            "UA-000000-00", 
///            "12345", 
///            "0005", 
///            "ProductName", 
///            "12.99", 
///            "1", 
///            "ProductRef", 
///            "POption"
///            );
///        }
/// </summary>
public class GoogleMeasurementProtocol
{
    public GoogleMeasurementProtocol()
    {
    }

    /// <summary>
    /// Sends ecommerce order details/totals to Google Analytics using Measurement Protocol
    /// </summary>
    /// <param name="GoogleAnalyticsID">Tracking ID / Property ID account number</param>
    /// <param name="CustomerID">Unique customer id, number format</param>
    /// <param name="TransactionID">transaction / order ID. Required.</param>
    /// <param name="Affiliation">Affiliation or Site name</param>
    /// <param name="OrderTotal">Number to 2 decimal places as string</param>
    /// <param name="ShippingCost">Number to 2 decimal places as string</param>
    /// <param name="Documenthostname">Document hostname i.e.  mysite.com</param>
    /// <param name="PageName">Page name . i.e. ordercomplete.aspx</param>
    /// <param name="Title">Page title or name</param>
    /// <returns>null</returns>
    public void SendGoogleTrackingOrder(string GoogleAnalyticsID, string CustomerID, string TransactionID, string Affiliation, string OrderTotal, string ShippingCost, string Documenthostname, string PageName, string Title)
    {
        try
        {

            var request = (HttpWebRequest)WebRequest.Create("http://www.google-analytics.com/collect");
            request.Method = "POST";
            request.KeepAlive = false;
            // the request body we want to send

            var postData = new Dictionary<string, string>
                           {
                               { "v", "1" }, // Version.
                               { "tid", GoogleAnalyticsID }, // // Tracking ID / Property ID.
                               { "cid", "555" + CustomerID }, // Anonymous Client ID.
                               { "t", "pageview" }, // Hit Type.
                               { "dh", Documenthostname }, // Document hostname.
                               { "dp", PageName }, // Page.
                               { "dt", Title }, // Title.

                              { "ti", TransactionID}, // Transaction / Order ID. Required.
                               { "ta", Affiliation }, // Affiliation.
                               { "tr", OrderTotal }, // Revenue.
                               { "tt", "0" }, // Tax.
                               { "ts", ShippingCost }, // Shipping.
                               { "pa", "purchase" }, // purchase.

                           };

            postData.Add("z", DateTime.Now.ToString("yyyyMMddHHmmss"));
            var postDataString = postData
                .Aggregate("", (data, next) => string.Format("{0}&{1}={2}", data, next.Key,
                                                             HttpUtility.UrlEncode(next.Value)))
                .TrimEnd('&');

            // set the Content-Length header to the correct value
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataString);

            // write the request body to the request
            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(postDataString);
            }

            try
            {
                var webResponse = (HttpWebResponse)request.GetResponse();
                if (webResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new HttpException((int)webResponse.StatusCode,
                                            "Google Analytics tracking did not return OK 200");
                }
                webResponse.Close();
            }
            catch (Exception ex)
            {
                DoLog(ex.ToString());
            }
        }
        catch (Exception ex)
        {
            DoLog(ex.ToString());

        }

    }
    /// <summary>
    ///  Sends a single purchased item to Google Analytics using Measurement Protocol
    /// </summary>
    /// <param name="GoogleAnalyticsID">Tracking ID / Property ID account number</param>
    /// <param name="CustomerID">Unique customer id, number format</param>
    /// <param name="TransactionID">transaction / order ID. Required.</param>
    /// <param name="Itemname">Item name. Required.</param>
    /// <param name="Itemprice">Item price. Number to 2 decimal places as string</param>
    /// <param name="Itemquantity">Item quantity. Number as string</param>
    /// <param name="Itemcode">Item code / SKU.</param>
    /// <param name="Itemvariation">Item variation / category.</param>
    /// <returns>null</returns>
    public void SendGoogleTrackingItem(string GoogleAnalyticsID, string CustomerID, string TransactionID, string Itemname, string Itemprice, string Itemquantity, string Itemcode, string Itemvariation)
    {
        try
        {

            var request = (HttpWebRequest)WebRequest.Create("http://www.google-analytics.com/collect");
            request.Method = "POST";
            request.KeepAlive = false;
            // the request body we want to send

            int x = 1;
            var postData = new Dictionary<string, string>
                           {
                               { "v", "1" }, // Version.
                               { "tid", GoogleAnalyticsID }, // Tracking ID / Property ID.
                               { "cid", "555" + CustomerID }, // Anonymous Client ID.
                               { "t", "item" }, // Item hit type.
                               { "ti",TransactionID }, // Transaction / Order ID. Required.
                               { "in",Itemname }, // Item name. Required.
                               { "ip",Itemprice }, // Item price.
                               { "iq",Itemquantity }, // Item quantity.
                               { "ic",Itemcode }, // Item code / SKU.
                               { "iv",Itemvariation }, // Item variation / category.
                               { "cu", "GBP" }, // Currency code. Set to your currency

                           };

            postData.Add("z", DateTime.Now.ToString("yyyyMMddHHmmss"));
            var postDataString = postData
                .Aggregate("", (data, next) => string.Format("{0}&{1}={2}", data, next.Key,
                                                             HttpUtility.UrlEncode(next.Value)))
                .TrimEnd('&');

            // set the Content-Length header to the correct value
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataString);

            // write the request body to the request
            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(postDataString);
            }

            try
            {
                var webResponse = (HttpWebResponse)request.GetResponse();
                //  Response.Write(webResponse.StatusCode);
                if (webResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new HttpException((int)webResponse.StatusCode,
                                            "Google Analytics tracking did not return OK 200");
                }
                webResponse.Close();
            }
            catch (Exception ex)
            {
                DoLog(ex.ToString());
            }
        }
        catch (Exception ex)
        {
           DoLog(ex.ToString());
        }
    }


    /// <summary>
    /// Write log message to hard drive, for error debugging
    /// </summary>
    /// <param name="strMsg"></param>
    private void DoLog(string strMsg)
    {
        string filename = System.Web.HttpContext.Current.Server.MapPath("\\").Replace("\\wwwroot", "") + "log.txt";
        FileStream fs = new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.Write);
        fs.Close();
        StreamWriter sw = new StreamWriter(filename, true, Encoding.ASCII);
        sw.Write(System.Environment.NewLine + DateTime.Now.ToString() + "," + strMsg);
        sw.Close();

    }
}