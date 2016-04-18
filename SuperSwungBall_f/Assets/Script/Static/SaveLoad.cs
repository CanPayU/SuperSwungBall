using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad
{
    public static User savedUser;
    public static Settings setting;

    public static void save_user()
    {
        Debug.Log("save_user " + Application.persistentDataPath);
        SaveLoad.savedUser = User.Instance;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/user.txt");
        bf.Serialize(file, SaveLoad.savedUser);
        file.Close();
    }

    public static void reset_user()
    {
        Debug.Log("reset_user " + Application.persistentDataPath);
        File.Delete(Application.persistentDataPath + "/user.txt");
        User.Instance = new User();
    }

    public static bool load_user()
    {
        Debug.Log("load_user " + Application.persistentDataPath);
        if (File.Exists(Application.persistentDataPath + "/user.txt"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/user.txt", FileMode.Open);
            try
            {
                SaveLoad.savedUser = (User)bf.Deserialize(file);
            }
            catch (System.Exception)
            {
                Debug.LogError("-- MàJ sur la classe User disponible. -- V" + User.VERSION);
                return false;
            }
            if (savedUser.version != User.VERSION)
            {
                Debug.LogWarning("-- MàJ sur la classe User disponible (recommendé). --");
            }
            User.Instance = savedUser;
            file.Close();
            return true;
        }
        return false;
    }

    public static void reset_setting()
    {
        Debug.Log("reset_setting " + Application.persistentDataPath);
        File.Delete(Application.persistentDataPath + "/settings.txt");
        Settings.Instance = new Settings();
    }
    public static void save_setting()
    {
        Debug.Log("save_setting at " + Application.persistentDataPath);
        SaveLoad.setting = Settings.Instance; // contient PSG & France
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/settings.txt");
        bf.Serialize(file, SaveLoad.setting);
        file.Close();
    }
    public static void load_settings()
    {
        Debug.Log("load_settings at " + Application.persistentDataPath);
        if (File.Exists(Application.persistentDataPath + "/settings.txt"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/settings.txt", FileMode.Open);
            try
            {
                SaveLoad.setting = (Settings)bf.Deserialize(file);
            }
            catch (System.Exception)
            {
                Debug.LogError("-- MàJ sur la classe Settings disponible. -- V" + Settings.VERSION);
                return;
            }
            if (setting.version != Settings.VERSION)
            {
                Debug.LogWarning("-- MàJ sur la classe Settings disponible (recommendé). --");
            }
            Settings.Instance = setting;
            file.Close();
        }
        else
        {
            BinaryFormatter bf = new BinaryFormatter();
            TextAsset asset = Resources.Load("settings") as TextAsset;
            Stream file = new MemoryStream(asset.bytes);
            SaveLoad.setting = (Settings)bf.Deserialize(file);
            Settings.Instance = setting;
            file.Close();
            save_setting();
        }
    }
}