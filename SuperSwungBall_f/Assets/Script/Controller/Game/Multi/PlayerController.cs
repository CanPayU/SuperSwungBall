using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using GameKit;
using GameScene.Replay;
using GameScene.Multi.Replay;

namespace GameScene.Multi
{
	public class PlayerController : BasicPlayerController
	{


		private static int countData = 0;

		private void incrementData()
		{
			countData++;
			if (countData >= 5)
			{
				Caller.StartAnimation();
				countData = 0;
			}
		}


		private ReplayController replayController;

		public PlayerController(){
			this.eventType = GameKit.EventType.All;
		}
		
		//Network
		private PhotonView view;

		protected override void Start()
		{
			base.Start ();
			this.view = GetComponent<PhotonView>();
			this.replayController = GameObject.Find ("Main").GetComponent<ReplayController> ();
		}

		// -- Event
		public override void OnEndTimer()
		{
			SyncValues();
		}

		public override void OnStartAnimation(){
			PlayerAction action = new PlayerAction (0, this.PointDeplacement, this.Player.Button_Values);
			this.replayController.setPlayerAction(this.Player, action);
			start_Anim(false);
		}

		public override bool passe(ref Vector3 pointPasse)
		{
			Debug.Log("In PC:" + Player.Name + " - point:" + arrivalPoint);
			pointPasse = arrivalPointPasse;
			if (phaseAnimation && Vector3.Distance(arrivalPointPasse, transform.position) < player.ZonePasse * 5)
			{

				PlayerAction action = new PlayerAction(0, this.PointDeplacement, this.arrivalPointPasse, this.Player.Button_Values, transform.position);
				this.replayController.setPlayerAction(this.Player, action);
				return true;
			}
			return false;
		}

		// -- Network
		public void SyncValues() // A améliorer (voir Replay)
		{
			if (!isMine)
				return;

			var player_values = new Dictionary<string, object>();
			player_values.Add("PointDep", serialize_vector3(this.PointDeplacement));
			player_values.Add("PointPasse", serialize_vector3(this.PointPasse));
			player_values.Add("BtnValues", this.Player.Button_Values);

			view.RPC("GetMyParam", PhotonTargets.Others, view.viewID, (byte[])ObjectToByteArray(player_values));
			//start_Anim(false);
		}

		[PunRPC]
		private void GetMyParam(int viewID, byte[] param)
		{
			if (viewID != view.viewID)
				return;
			Dictionary<string, object> values = (Dictionary<string, object>)ByteToObject(param);
			this.PointDeplacement = deserialize_vector3((float[])values["PointDep"]);
			this.PointPasse = deserialize_vector3((float[])(values["PointPasse"]));
			this.Player.Button_Values = (List<string>)(values["BtnValues"]);
			incrementData();
		}

		#region Serialization
		private byte[] ObjectToByteArray(object obj)
		{
			if (obj == null)
				return null;
			BinaryFormatter bf = new BinaryFormatter();
			using (MemoryStream ms = new MemoryStream())
			{
				bf.Serialize(ms, obj);
				return ms.ToArray();
			}
		}
		private object ByteToObject(byte[] arrBytes)
		{
			MemoryStream memStream = new MemoryStream();
			BinaryFormatter binForm = new BinaryFormatter();
			memStream.Write(arrBytes, 0, arrBytes.Length);
			memStream.Seek(0, SeekOrigin.Begin);
			object obj = (object)binForm.Deserialize(memStream);

			return obj;
		}
		private float[] serialize_vector3(Vector3 v)
		{
			float[] result = new float[3];
			result[0] = v.x;
			result[1] = v.y;
			result[2] = v.z;
			return result;
		}
		private Vector3 deserialize_vector3(float[] values)
		{
			return new Vector3(values[0], values[1], values[2]);
		}
		#endregion
	}
}