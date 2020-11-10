using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveData
{
    private static string savePath = Application.persistentDataPath + "/saves/";
    public static void Save<T>(T objectToSave, string key)
    {
        Directory.CreateDirectory(savePath);
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fs = new FileStream(savePath + key + ".dat", FileMode.Create);
        
        using (fs)
        {
            formatter.Serialize(fs, objectToSave);
        }
    }

    public static void Save<T>(T objectToSave, string subPath, string key)
    {
        Directory.CreateDirectory(savePath + subPath);
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fs = new FileStream(savePath + subPath + key + ".dat", FileMode.Create);

        using (fs)
        {
            formatter.Serialize(fs, objectToSave);
        }
    }

    public static T Load<T>(string key)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fs = new FileStream(savePath + key + ".dat", FileMode.Open);
        T returnValue = default;


        using (fs)
        {
            returnValue = (T)formatter.Deserialize(fs);
            
        }

        return returnValue;
    }

    public static List<T> LoadAll<T>(string subPath)
    {
        Directory.CreateDirectory(savePath + subPath);
        BinaryFormatter formatter = new BinaryFormatter();

        List<T> returnValue = new List<T>();


        foreach (string file in Directory.EnumerateFiles(savePath + subPath, "*.dat"))
        {
            
            FileStream fs = new FileStream(file, FileMode.Open);
            
            T deserializedValue = default;
            using (fs)
            {
                
                deserializedValue = (T)formatter.Deserialize(fs);
                
                returnValue.Add(deserializedValue);

            }
        }
        
        

        

        return returnValue;
    }

    public static bool SaveAlreadyExists(string key)
    {
        return File.Exists(savePath + key + ".dat");
    }

    
}
