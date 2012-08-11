// ======================================================================================
// File         : HSController.cs
// Author       : nantas 
// Last Change  : 03/31/2012 | 17:32:17 PM | Saturday,March
// Description  : 
// ======================================================================================

using UnityEngine;
using System.Collections;

public class HSController : MonoBehaviour
{
    private string secretKey = "witch777danmaku"; // Edit this value and make sure it's the same as the one stored on the server
    private string addScoreURL = "http://danmaku.comeze.com/addscore.php?"; //be sure to add a ? to your url
    private string highscoreURL = "http://danmaku.comeze.com/display.php";
    
    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Start()
    {
       // StartCoroutine(GetScores());
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    // public bool TestConnection() {
    //     float timeTaken = 0.0F;
    //     float maxTime = 2.0F;
    //     bool thereIsConnection = false;
 
    //     Ping testPing = new Ping( "31.170.163.90" );//ip of 000webhost

    //     timeTaken = 0.0F;
    //     while ( !testPing.isDone )
    //     {
    //         timeTaken += Time.deltaTime;
    //         if ( timeTaken > maxTime )
    //         {
    //             // if time has exceeded the max
    //             // time, break out and return false
    //             thereIsConnection = false;
    //             break;
    //         }
    //     }
    //     if ( timeTaken <= maxTime ) thereIsConnection = true;
    //     return thereIsConnection;
    // }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    // remember to use StartCoroutine when calling this function!
    public IEnumerator PostScores(string name, int score) //return true for submisstion successful.
    {
        if (name == " ") {
            name = "Molika ";
        }
        //This connects to a server side php script that will add the name and score to a MySQL DB.
        // Supply it with a string representing the players name and the players score.
        string hash = MD5Test.MD5Sum(name + score + secretKey);

        string post_url = addScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash;

        // Post the URL to the site and create a download object to get the result.
        WWW hs_post = new WWW(post_url);
        yield return hs_post; // Wait until the download is done

        // int tryTimes = 1000;
        // while (!hs_post.isDone) {
        //     if (tryTimes <= 0) {
        //         Debug.Log("connection over time, no score submitted.");
        //         return false;
        //     } else {
        //         tryTimes -= 1;
        //     }
        // }

        if (hs_post.error != null)
        {
            Debug.Log("There was an error posting the high score: " + hs_post.error);
        }

    }


    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    // Get the scores from the MySQL DB to display in a GUIText.
    // remember to use StartCoroutine when calling this function!
    public IEnumerator GetScoresTo(exSpriteFont _targetText)
    {
        _targetText.text = "loading scores...";
        //Debug.Log(_targetText.text);
        WWW hs_get = new WWW(highscoreURL);
        yield return hs_get;

        if (hs_get.error != null)
        {
            Debug.Log("There was an error getting the high score: " + hs_get.error);
            _targetText.text = "can't retrieve score. ";
        }
        else
        {
            _targetText.text = hs_get.text; // this is a GUIText that will display the scores in game.
            Debug.Log(hs_get.text);
        }
    }

}


