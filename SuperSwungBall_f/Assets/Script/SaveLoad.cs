using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad {

	public static User savedUser;

	public static void save_user() {
		SaveLoad.savedUser = User.Instance;
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/user.gd");
		bf.Serialize(file, SaveLoad.savedUser);
		file.Close();
	}  

	public static void reset_user(){
		File.Delete (Application.persistentDataPath + "/user.gd");
	}

	public static bool load_user() {
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
}