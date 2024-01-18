using UnityEngine;

public static class PlayerPrefsHandler
{
    public static string mode = "mode";
    public static string availableBrands = "AvailableBrands";
    public static string Mode
    {
        get
        {
            return PlayerPrefs.GetString(mode);
        }
        set
        {
            PlayerPrefs.SetString(mode, value);
        }
    }
    public static string AvailableBrands
    {
        get
        {
            return PlayerPrefs.GetString(availableBrands);
        }
        set
        {
            PlayerPrefs.SetString(availableBrands, value);
        }
    }
}
