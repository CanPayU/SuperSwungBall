using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad {

	public static User savedUser;
	public static Settings setting;

	public static void save_user() {
		Debug.Log ("save_user "+ Application.persistentDataPath);
		SaveLoad.savedUser = User.Instance;
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/user.gd");
		bf.Serialize(file, SaveLoad.savedUser);
		file.Close();
	}  

	public static void reset_user(){
		Debug.Log ("reset_user "+ Application.persistentDataPath);
		File.Delete (Application.persistentDataPath + "/user.gd");
	}

	public static bool load_user() {
		Debug.Log ("load_user "+ Application.persistentDataPath);
		if(File.Exists(Application.persistentDataPath + "/user.gd")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/user.gd", FileMode.Open);
			SaveLoad.savedUser = (User)bf.Deserialize(file);
			User.Instance = savedUser;
			file.Close();
			return true;
		}
		return false;
	}

	public static void save_setting(){
		Debug.Log ("save_setting at "+ Application.persistentDataPath);
		SaveLoad.setting = Settings.Instance; // contient PSG & France
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/settings.txt");
		bf.Serialize(file, SaveLoad.setting);
		file.Close();
	}
	public static void load_settings() {
		Debug.Log ("load_settings at "+ Application.persistentDataPath);
		if (File.Exists (Application.persistentDataPath + "/settings.txt")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/settings.txt", FileMode.Open);
			SaveLoad.setting = (Settings)bf.Deserialize (file);
			Settings.Instance = setting;
			file.Close ();
		} else {
			BinaryFormatter bf = new BinaryFormatter ();
			TextAsset asset = Resources.Load ("settings") as TextAsset;
			Stream file = new MemoryStream (asset.bytes);
			SaveLoad.setting = (Settings)bf.Deserialize (file);
			Settings.Instance = setting;
			file.Close ();
			save_setting ();
		}
	}
}