
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class VMRestoreScript : MonoBehaviour
{
    private const string VM_RESTORE_URL = "https://infrahub-api.nexgencloud.com/v1/core/virtual-machines/98887/hibernate-restore";
    private const string API_KEY = "05205b7b-7e42-4dfd-bf8d-9292c1410994";

    private void Start()
    {
        StartCoroutine(RestoreVM());
    }

    private IEnumerator RestoreVM()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(VM_RESTORE_URL))
        {
            www.SetRequestHeader("api_key", API_KEY);
            www.SetRequestHeader("accept", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error restoring VM: {www.error}");
            }
            else
            {
                string jsonResponse = www.downloadHandler.text;
                JObject response = JObject.Parse(jsonResponse);

                // You can add more detailed parsing of the response here if needed
                Debug.Log("VM restore request sent successfully");
                Debug.Log($"Response: {response}");
            }
        }
    }
}