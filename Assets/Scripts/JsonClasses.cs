
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonClasses : MonoBehaviour
{
}

[Serializable]
public class Search_User
{
    internal static object item;
    public int id;
    public string first_name;
    public string last_name;
    public string email;
    public object phone_number;
    public string username;
    public string wallet_address;
    internal object ethereum_address;
}

[Serializable]
public class Meta
{
    public string token;
}

[Serializable]
public class DailyPointsRewardRoot
{
    public Search_User user;
    public Meta meta;
    public Datum Datum;

}

////////////////////////////////
[Serializable]
public class Link
{
    public int id;
    public string name;
    public string link;
}
[Serializable]
public class LinksRoot
{
    public List<Link> links;
}

[Serializable]
public class UserData
{
    public double latitude;
    public double longitude;
    public Search_User user;
}
[Serializable]
public class LocationUserRoot
{
    public List<UserData> user_locations;
}

//////////////////////////////Actions Location Json/////
//////
///
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

//[Serializable]
//public class Datum
//{
//    public int id;
//    public double lat;
//    public double lng;
//    public string image_url;
//    public string modal_url;
//}

//[Serializable]
//public class LocationRoot
//{
//    public List<Datum> data;
//}
[Serializable]
public class Datum
{
    public int id;
    public double lat;
    public double lng;
    public string image_url;
    public string modal_url;
    public string title;
    public string description;
    public string brand_name;
    public string url;
    public string consumption_date;
    public bool viewed;
    public string collection_limit_remaining;
    public bool auto_collect;
    public string brand_image_url;
    public int brand_id;
}
[Serializable]
public class LocationRoot
{
    public List<Datum> locations;
}

namespace MapLocations
{
    [Serializable]
    public class Location
    {
        public int id;
        public double lat;
        public double lng;
        public string title;
        public string description;
        public string brand_name;
        public string url;
        public string image_url;
        public bool collected;
        public int collection_limit_remaining;
        public bool auto_collect;
        public string brand_image_url;
        public int brand_id;
    }
    [Serializable]
    public class Root
    {
        public List<Location> locations;
    }
}





[Serializable]
public class ButtonData
{
    public int Number;
}
[Serializable]
public class VerifyEmailRoot
{
    public bool sucess;
    public string message;
}

[Serializable]
public class PostLoginMsg
{
    public bool sucess;
    public string message;
}

[Serializable]
public class WalletConnect
{
    public bool success;
    public string msg;
}
[Serializable]
public class ethereum_address
{
    public bool success;
    public string msg;
}
[Serializable]
public class deletedUserAcount
{
    public bool success;
    public string error;
}
[Serializable]
public class deactivateUserAcount
{
    public bool success;
    public string msg;
}

public class ConsumeLocation
{
    public bool succes;
}
public class JsonUpdatePassword
{
    public bool success;
    public string msg;
}


public class  AllLocations
{
    public int id;
    public double lat;
    public double lng;
    public string image_url;
    public bool collected;
   
}

public class AllLocationRot
{
    public List<AllLocations> locations;
}
[Serializable]
public class UserAccountRoot
{
    public bool success;
    public string message;
}
