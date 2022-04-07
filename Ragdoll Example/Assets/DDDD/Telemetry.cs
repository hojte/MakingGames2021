using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Globalization;
using System.Threading;

public class Telemetry : MonoBehaviour
{
    public struct LevelData
    {
        public int level;
        public float levelCompletionTime;
        public int clickUsed;
        public int airRedirectAmount;
        public int airRedirectUsed;
        public string deathBy;
        public bool won;
    }

    /*Telemetry form link*/ //Remember to remove the last part of the link *viewform...*
    private const string GoogleFormBaseUrl = "https://docs.google.com/forms/d/e/1FAIpQLSdAXaw7gSR-0hxqvdXOLYtw34bJlejvF3bfsbTLh02c21cSfA/";

    /*Telemtry form variables*/
    private const string form_game_version = "entry.1750377727";
    private const string form_id_player = "entry.454193688";
    private const string form_id_run = "entry.1319642982";
    private const string form_level = "entry.171390795";
    private const string form_levelTime = "entry.2057710780";
    private const string form_clickUsed = "entry.192905949";
    private const string form_airRedirectAmount = "entry.1887151140";
    private const string form_airRedirectUsed = "entry.1454549837";
    private const string form_deathBy = "entry.1347168386";
    private const string form_won = "entry.2100446783";
    private static Guid runID;
    public static string gameVersion = "A";
    public static string playerID = "null";
    public static int deaths_training = 0;
    public static int deaths = 0;
    public static int enemyCount = 0;
    public static IEnumerator SubitGoogleForm(LevelData lvlData)
    {
        //These lines make sure that you are not going to have any comma/dot problems. 
        //But you also need to make sure that your spreadsheet settings are UK as well.
        CultureInfo ci = CultureInfo.GetCultureInfo("en-GB");
        Thread.CurrentThread.CurrentCulture = ci;

        string urlGoogleFormResponse = GoogleFormBaseUrl + "formResponse";

        WWWForm form = new WWWForm();

        form.AddField(form_game_version, gameVersion);
        form.AddField(form_id_player, playerID);
        form.AddField(form_id_run, GUIDToShortString(runID));
        form.AddField(form_level, lvlData.level);
        form.AddField(form_levelTime, (int)lvlData.levelCompletionTime);
        form.AddField(form_clickUsed, lvlData.clickUsed);
        form.AddField(form_airRedirectAmount, lvlData.airRedirectAmount);
        form.AddField(form_airRedirectUsed, lvlData.airRedirectUsed);
        form.AddField(form_deathBy, lvlData.deathBy);
        form.AddField(form_won, lvlData.won.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post(urlGoogleFormResponse, form))
        {
            yield return www.SendWebRequest(); //Decomment this line to send the data!
            
            //You can keep these 2 lines just to be sure that everything is working and there are no errors :)
            yield return null;
            print("Request sent");
        }
    }

    public static void openForm()
    {
        Application.OpenURL($"https://docs.google.com/forms/d/e/1FAIpQLSc2FTh_uSVQIIRBtngsfl5TdqApQ4_HhMM0aTuhbrc7k12Icg/viewform?&entry.159802043={Telemetry.playerID}&entry.1650724599={Telemetry.enemyCount}&entry.38789385={Telemetry.deaths}&entry.726257956={Telemetry.deaths_training}");
    }

    public static void GenerateNewRunID()
    {
        runID = Guid.NewGuid();
    }    
    public static void SetGameVersion(string v)
    {
        gameVersion = v;
    }
    public static void SetPlayerID(string id)
    {
        playerID = id;
    }

    public static void SetDeaths_Training(int deaths)
    {
        deaths_training = deaths;
    }

    public static void SetDeaths(int playerDeaths)
    {
        deaths = playerDeaths;
    }

    public static void SetEnemyCount(int amountOfEnemies)
    {
        enemyCount = amountOfEnemies;
    }

    public static string GUIDToShortString(Guid guid)
    {
        var base64Guid = Convert.ToBase64String(guid.ToByteArray());

        // Replace URL unfriendly characters with better ones
        base64Guid = base64Guid.Replace('+', '-').Replace('/', '_');

        // Remove the trailing ==
        return base64Guid.Substring(0, base64Guid.Length - 2);
    }

    public static void SubmitRating(int rating)
    {
        throw new NotImplementedException(); //Do stuff here
    }

}
