using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Mapbox.Platform;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public InputField EmailInput;

    public void verifyEmail()
    {
        if (IsValid(EmailInput.text) == true)
        {
            Debug.Log("Email is valid");
        }
        else
        {
            Debug.Log("Email is not valid");
        }
        //IsValid(EmailInput.text);
    }
    public bool IsValid(string emailaddress)
    {
        //string email = txtemail.Text;
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(emailaddress);
        if (match.Success)
            return true;
        else
            return false;
    }
}
