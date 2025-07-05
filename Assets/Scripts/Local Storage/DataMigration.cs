using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMigration : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteKey("UserData");
        PlayerPrefs.Save();
        Debug.Log("Existing user data cleared.");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
