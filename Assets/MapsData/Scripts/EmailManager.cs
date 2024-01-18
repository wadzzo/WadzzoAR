using System.Collections;
using UnityEngine;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;


public class EmailManager : MonoBehaviour
{
  
   // [SerializeField] TMPro.TextMeshProUGUI txtData;
    //[SerializeField] UnityEngine.UI.Button btnSubmit;
    [SerializeField] bool sendDirect;

    const string kSenderEmailAddress = "myalldevices2021@gmail.com";
    const string kSenderPassword = "Player2013";
    const string kReceiverEmailAddress = "onegreggybrady@gmail.com";

    // Method 2: Server request
    const string url = "https://coderboy6000.000webhostapp.com/emailer.php";

    void Start()
    {
        //UnityEngine.Assertions.Assert.IsNotNull(txtData);
        //UnityEngine.Assertions.Assert.IsNotNull(btnSubmit);
        //btnSubmit.onClick.AddListener(delegate {
        //    if (sendDirect)
        //    {
        //        SendAnEmail(txtData.text);
        //    }
        //    else
        //    {
        //        SendServerRequestForEmail(txtData.text);
        //    }
        //});
    }

    public static bool bzb1=true, bzb2=true, bzb3=true, bzb4=true, Dad=true;

    public void SendEmail(string targetName)
    {
        Debug.Log("Sending Email");
        switch (targetName)
        {
            case "bzb1":
                if (bzb1)
                {
                    string message = "User Scaaned " + targetName;
                    SendAnEmail(message);
                    bzb1 = false;
                }
                break;

            case "bzb2":
                if (bzb2)
                {
                    string message = "User Scaaned " + targetName;
                    SendAnEmail(message);
                    bzb2 = false;
                }
                break;

            case "bzb3":
                if (bzb3)
                {
                    string message = "User Scaaned " + targetName;
                    SendAnEmail(message);
                    bzb3 = false;
                }
                break;

            case "bzb4":
                if (bzb4)
                {
                    string message = "User Scaaned " + targetName;
                    SendAnEmail(message);
                    bzb4 = false;
                }
                break;

            case "Dad":
                if (Dad)
                {
                    string message = "User Scaaned " + targetName;
                    SendAnEmail(message);
                    Dad = false;
                }
                break;
        }


        
    }

    // Method 1: Direct message
    public static void SendAnEmail(string message)
    {
        // Create mail
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(kSenderEmailAddress);
        mail.To.Add(kReceiverEmailAddress);
        mail.Subject = "Scan Update";
        mail.Body = message;

        // Setup server 
        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new NetworkCredential(
            kSenderEmailAddress, kSenderPassword) as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslPolicyErrors) {
                Debug.Log("Email success!");
                return true;
            };

        // Send mail to server, print results
        try
        {
            smtpServer.Send(mail);
        }
        catch (System.Exception e)
        {
            Debug.Log("Email error: " + e.Message);
        }
        finally
        {
            Debug.Log("Email sent!");
        }
    }

    // Method 2: Server request
    private void SendServerRequestForEmail(string message)
    {
        StartCoroutine(SendMailRequestToServer(message));
    }

    // Method 2: Server request
    static IEnumerator SendMailRequestToServer(string message)
    {
        // Setup form responses
        WWWForm form = new WWWForm();
        form.AddField("name", "It's me!");
        form.AddField("fromEmail", kSenderEmailAddress);
        form.AddField("toEmail", kReceiverEmailAddress);
        form.AddField("message", message);

        // Submit form to our server, then wait
        WWW www = new WWW(url, form);
        Debug.Log("Email sent!");

        yield return www;

        // Print results
        if (www.error == null)
        {
            Debug.Log("WWW Success!: " + www.text);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
}
