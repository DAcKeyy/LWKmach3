using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Collections;
using System.Collections.Generic;

public class DataSaver
{
    //Save Data
    public static void SaveData<T>(T dataToSave, string dataFileName)
    {
        string tempPath = Path.Combine(Application.persistentDataPath, "data");
        tempPath = Path.Combine(tempPath, dataFileName + ".json");

        //Convert To Json then to bytes
        string jsonData = JsonUtility.ToJson(dataToSave, true);
        byte[] jsonByte = Encoding.ASCII.GetBytes(jsonData);

        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(tempPath));
        }
        //Debug.Log(path);

        try
        {
            File.WriteAllBytes(tempPath, jsonByte);
            //Debug.Log("Saved Data to: " + tempPath.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To PlayerInfo Data to: " + tempPath.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }
    }


    public static List<Сoupon> FindData<T>()
    {
        List<Сoupon> Coupons = new List<Сoupon>();

        FileInfo[] files = null;

        var root = new DirectoryInfo(Path.Combine(Application.persistentDataPath, "data"));

        try
        {
            files = root.GetFiles("*.*");
        }
        catch (UnauthorizedAccessException e)
        {
            Debug.Log(e.Message);
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.Log(e.Message);
        }

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        if (files != null)
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        {
            foreach (FileInfo file in files)
            {
                string fileFullName = file.FullName;
                //Debug.Log(fileFullName);
                Coupons.Add(LoadData<Сoupon>(fileFullName));
            }
        }
        return Coupons;
    }

    //Load Data
    public static T LoadData<T>(string fileFullName)
    {

        byte[] jsonByte = null;
        try
        {
            jsonByte = File.ReadAllBytes(fileFullName);
            //Debug.Log("Loaded Data from: " + fileFullName.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Load Data from: " + fileFullName.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }

        //Convert to json string
        string jsonData = Encoding.ASCII.GetString(jsonByte);

        //Convert to Object
        object resultValue = JsonUtility.FromJson<T>(jsonData);
        return (T)Convert.ChangeType(resultValue, typeof(T));
    }

    public static bool DeleteData(string dataFileName)
    {
        bool success = false;

        //Load Data
        string tempPath = Path.Combine(Application.persistentDataPath, "data");
        tempPath = Path.Combine(tempPath, dataFileName + ".json");

        //Exit if Directory or File does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
        {
            Debug.LogWarning("Directory does not exist");
            return false;
        }

        if (!File.Exists(tempPath))
        {
            Debug.Log("File does not exist");
            return false;
        }

        try
        {
            File.Delete(tempPath);
            Debug.Log("Data deleted from: " + tempPath.Replace("/", "\\"));
            success = true;
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Delete Data: " + e.Message);
        }

        return success;
    }
}