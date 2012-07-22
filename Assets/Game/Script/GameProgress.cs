// ======================================================================================
// File         : GameProgress.cs
// Author       : nantas 
// Last Change  : 03/08/2012 | 22:43:18 PM | Thursday,March
// Description  : custom classes to save player progress and continue games. 
// ======================================================================================

//TODO: using a lot of namespaces just to make it work quick
//maybe it's not worth the effort to find new solution for the sake of 1MB library in binary.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization; 
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;




///////////////////////////////////////////////////////////////////////////////
// class GameProgress
// 
// Purpose: initiate date and save them when player pause the game or application
// quit.
// 
///////////////////////////////////////////////////////////////////////////////

public class GameProgress : MonoBehaviour {


	///////////////////////////////////////////////////////////////////////////////
    //data instance for custom classes.
    ///////////////////////////////////////////////////////////////////////////////


    void Awake () {
        LoadGameProgress();
    }


    ///////////////////////////////////////////////////////////////////////////////
    //save and load proper instance of custom classes.
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 


	public void SavePlayerProfile () {
        Save<PlayerProfile>("PlayerProfile", GlobalSettings.instance.playerProfile);
	}

    public void LoadPlayerProfile () {
        GlobalSettings.instance.playerProfile = Load<PlayerProfile>("PlayerProfile");
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

	public void LoadGameProgress () {

	}


	///////////////////////////////////////////////////////////////////////////////
	// Save and load with encrypt
	// code source: http://www.lucianoiurino.it/savingloading-game-data-in-unity
	///////////////////////////////////////////////////////////////////////////////

	// Save game data
	public static void Save<T> (string name, T instance)
    {
        XmlSerializer serializer = new XmlSerializer (typeof(T));
        using (var ms = new MemoryStream ()) {
            serializer.Serialize (ms, instance);

			string tmpString = Encrypt(System.Text.ASCIIEncoding.ASCII.GetString (ms.ToArray ()));	

            PlayerPrefs.SetString (name, tmpString);
        }
    }

	// Load game data
    public static T Load<T> (string name)
    {
        if(!PlayerPrefs.HasKey(name)) return default(T);
        XmlSerializer serializer = new XmlSerializer (typeof(T));
        T instance;
        using (var ms = new MemoryStream (System.Text.ASCIIEncoding.ASCII.GetBytes (Decrypt(PlayerPrefs.GetString (name))))) {	// Decrypt before loading
            instance = (T)serializer.Deserialize (ms);
        }
        return instance;
    }

	// Encrypt game data
	public static string Encrypt (string toEncrypt)
	{
		byte[] keyArray = UTF8Encoding.UTF8.GetBytes ("12345678901234567890123456789012"); // 256-AES key
		byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes (toEncrypt);
		RijndaelManaged rDel = new RijndaelManaged ();
		rDel.Key = keyArray;
		rDel.Mode = CipherMode.ECB;
		rDel.Padding = PaddingMode.PKCS7;
		ICryptoTransform cTransform = rDel.CreateEncryptor ();
		byte[] resultArray = cTransform.TransformFinalBlock (toEncryptArray, 0, toEncryptArray.Length);
		return Convert.ToBase64String (resultArray, 0, resultArray.Length);
	}

	// Decrypt game data
	public static string Decrypt (string toDecrypt)
	{
		byte[] keyArray = UTF8Encoding.UTF8.GetBytes ("12345678901234567890123456789012"); // AES-256 key
		byte[] toEncryptArray = Convert.FromBase64String (toDecrypt);
		RijndaelManaged rDel = new RijndaelManaged ();
		rDel.Key = keyArray;
		rDel.Mode = CipherMode.ECB;
		rDel.Padding = PaddingMode.PKCS7;
		ICryptoTransform cTransform = rDel.CreateDecryptor ();
		byte[] resultArray = cTransform.TransformFinalBlock (toEncryptArray, 0, toEncryptArray.Length);
		return UTF8Encoding.UTF8.GetString (resultArray);
	}

}
