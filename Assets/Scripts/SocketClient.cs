using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
[Serializable]
public class SocketClient:MonoBehaviour
{
	private static SocketClient instance;
	public static SocketClient Instance {  get { return instance; } }
		public  string Host = "127.0.0.1";
		public  int Port = 1234;
		public  Socket request;
		public  string hello = "Hello from client";
		public  byte[] dataReceiveBuffer = new byte[1024];
		public UnityAction OnConnectSuccess;
		public UnityAction<string> OnConnectFail;
		public UnityAction<object> OnReceiveSuccess;
		public UnityAction<string> OnReceiveFail;
		public ConnectionStatus ConnectionStatus;
	public string UserName;
	private void Awake()
	{
		if(instance == null)instance = this;
		else if(instance!=this)Destroy(this);
	}
	public  void Connect()
		{
			request = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			IPAddress iPAddress = IPAddress.Parse(Host);
			IPEndPoint endPoint = new IPEndPoint(iPAddress, Port);
			request.BeginConnect(endPoint, OnConnectCallback, null);
			Console.ReadLine();
		}
		private  void OnConnectCallback(IAsyncResult ar)
		{
			try
			{
				request.EndConnect(ar);
				Debug.Log("connect to server success");
				ConnectionStatus = ConnectionStatus.Success;
				MainThreadDispatcher.Instance.Enqueue(BeginReceive);
				byte[] buffer = Encoding.UTF8.GetBytes(hello);
				request.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, OnSendDataToServer, null);
			}
			catch (Exception ex)
			{
				Console.WriteLine("connect to server fail " + ex.ToString());
				OnConnectFail?.Invoke(ex.ToString());
			}
		}
			public void Send<T>(T data)
			{
			int size = 0;
			byte[] buffer = new byte[size];
			AppMath.ConvertStructeToByteArr(data, ref size, ref buffer);
			request.BeginSend(buffer,0,buffer.Length,SocketFlags.None, OnSendDataToServer, null);
			}
		private void BeginReceive()
		{
			if (ConnectionStatus == ConnectionStatus.Success)
			{
				OnConnectSuccess?.Invoke();
				request.BeginReceive(dataReceiveBuffer, 0, dataReceiveBuffer.Length, SocketFlags.None, OnReceiveCallBack, null);
			}
		}

		private  void OnSendDataToServer(IAsyncResult ar)
		{
			int byteSend = request.EndSend(ar);
			if (byteSend > 0)
			{
				Debug.Log($" send data success");
			}
			//request.BeginReceive(dataReceiveBuffer, 0, dataReceiveBuffer.Length, SocketFlags.None, OnReceiveCallBack, null);
		}

		private  void OnReceiveCallBack(IAsyncResult ar)
		{
		try
		{
			int byteRead = request.EndReceive(ar);
			Debug.Log($" byte read from server {byteRead}");
			if (byteRead > 0)
			{
				ST_DATA_TRANFER sT_DATA_TRANFER = new ST_DATA_TRANFER() { DataBool=false,
				DataByteArr= new byte[byteRead],DataInt=0,DataString="",DataUshort=0
				};
				AppMath.ConvertByteArrToStructure(dataReceiveBuffer, byteRead, ref sT_DATA_TRANFER);
				//Console.WriteLine("receive data success");
				StringBuilder sb = new StringBuilder();
				sb.AppendLine(sT_DATA_TRANFER.DataInt.ToString());
				sb.AppendLine(sT_DATA_TRANFER.DataUshort.ToString());
				sb.AppendLine(sT_DATA_TRANFER.DataBool.ToString());
				sb.AppendLine(sT_DATA_TRANFER.DataString.ToString());
				sb.AppendLine(sT_DATA_TRANFER.DataByteArr.Length.ToString());
				sb.AppendLine(sT_DATA_TRANFER.DataUshort.ToString());
				//sb.AppendLine(sT_DATA_TRANFER.DataByteArr.Count().ToString());
				Debug.Log($"message from server {sb.ToString()}");
				OnReceiveSuccess?.Invoke(sT_DATA_TRANFER);
			}
			request.BeginReceive(dataReceiveBuffer, 0, byteRead, SocketFlags.None, OnReceiveCallBack, null);
		}
		catch (SocketException ex) { 
			Debug.Log($"cant receive msg from server due to {ex.ToString()}");
		}
			
	}
}

public enum ConnectionStatus
{
	None,
	Success,
	Error
}
