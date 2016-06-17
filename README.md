# Ecommerce Google Analytics Measurement Protocol
Using Google  Google Analytics Measurement Protocol with C# to send orders to Google server side

Class and functions to send ecommerce transactions and items to Google Analytics using  Measurement Protocol,

Usage:
 
		GoogleMeasurementProtocol gmp = new GoogleMeasurementProtocol();

        gmp.SendGoogleTrackingOrder("UA-000000-00", 
        "12345", 
        "0005", 
        "SiteName", 
        "19.99", 
        "5.99", 
        "mysite.co.uk",
        "callback.aspx", 
        "Paypal Payment Complete");
            
        // now send items.
        foreach (DataRow row in DataTable)
        {
            gmp.SendGoogleTrackingItem(
            "UA-000000-00", 
            "12345", 
            "0005", 
           "ProductName", 
            "12.99", 
            "1", 
            "ProductRef", 
            "POption"
            );
       }
