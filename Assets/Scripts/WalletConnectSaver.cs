using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;

public class WalletConnectSaver : MonoBehaviour
{
    private string WalletConnectAddres;
    private string EthereumAddress;
    public GameObject WalletConnectInputField;
    public GameObject EthereumAddressInputField;
    public GameObject walletPanel;
    //public GameObject textDisplay;


    public void Start()
    {
        WalletConnectInputField.GetComponentInChildren<InputField>().text=AuthManager.Wallet_Address.ToString();
        EthereumAddressInputField.GetComponentInChildren<InputField>().text = AuthManager.ethereum_address.ToString();
    }

    public void GetWalletConnect()
    {
       
        WalletConnectAddres = WalletConnectInputField.GetComponentInChildren<InputField>().text;
        
        //textDisplay.GetComponent<Text>().text = WalletConnectAddres;
        Debug.Log(WalletConnectAddres);
        LoadingManager.instance.loading.SetActive(true);
        StartCoroutine(PostUpdateScoreRequest(WalletConnectAddres));


    }

    public void GetEthereumAddress()
    {

        EthereumAddress = EthereumAddressInputField.GetComponentInChildren<InputField>().text;

        //textDisplay.GetComponent<Text>().text = WalletConnectAddres;
        Debug.Log(EthereumAddress);
        LoadingManager.instance.loading.SetActive(true);
        StartCoroutine(PostUpdateScoreRequestEthereumAddress(EthereumAddress));


    }

    IEnumerator PostUpdateScoreRequest(string Walletaddress)
    {
        WWWForm form = new WWWForm();
        form.AddField("wallet_address", Walletaddress);


        string requestName = "api/v1/users/user_wallet_address";
        using (UnityWebRequest www = UnityWebRequest.Post(AuthManager.BASE_URL + requestName, form))
        {
            www.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                WalletConnect walletConnectServerResult = JsonUtility.FromJson<WalletConnect>(www.downloadHandler.text);

                LoadingManager.instance.loading.SetActive(false);
                Debug.Log(www.downloadHandler.text);
                if (walletConnectServerResult.success)
                {
                    ConsoleManager.instance.ShowMessage("Updated Successfully");
                    walletPanel.SetActive(false);
                    Debug.Log("done");
                    Debug.Log("Score Updated!");
                    AuthManager.Wallet_Address = WalletConnectAddres;


                }
                else
                {
                    Debug.Log("Error Updating Score!");
                    ConsoleManager.instance.ShowMessage("Error!");
                }


            }
        }
    }

        IEnumerator PostUpdateScoreRequestEthereumAddress(string EthereumAddress)
        {
            WWWForm form = new WWWForm();
            form.AddField("ethereum_address", EthereumAddress);


            string requestName = "api/v1/users/user_ethereum_address";
            using (UnityWebRequest www = UnityWebRequest.Post(AuthManager.BASE_URL + requestName, form))
            {
                www.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    ethereum_address ethereum_addressServerResult = JsonUtility.FromJson<ethereum_address>(www.downloadHandler.text);

                    LoadingManager.instance.loading.SetActive(false);
                    Debug.Log(www.downloadHandler.text);
                    if (ethereum_addressServerResult.success)
                    {
                        ConsoleManager.instance.ShowMessage("Updated Successfully");
                        walletPanel.SetActive(false);
                        Debug.Log("done");
                        Debug.Log("Score Updated!");
                        AuthManager.ethereum_address = EthereumAddress;


                    }
                    else
                    {
                        Debug.Log("Error Updating Score!");
                        ConsoleManager.instance.ShowMessage("Error!");
                    }


                }
            }
        }
}
        


