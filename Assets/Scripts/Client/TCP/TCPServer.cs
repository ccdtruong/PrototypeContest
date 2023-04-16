using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPServer
{
	private TcpListener tcpListener;
	private Thread tcpListenerThread;
	private TcpClient connectedTcpClient;
	private string m_host;

	public delegate void OnOpenFunc();
	public event OnOpenFunc OnOpen;
	public delegate void OnErrorFunc(SocketException e);
	public event OnErrorFunc OnError;
	public delegate void OnCloseFunc();
	public event OnCloseFunc OnClose;
	public delegate void OnMessageFunc(byte[] bytes);
	public event OnMessageFunc OnMessage;
	public Queue<byte[]> messages;

	// Use this for initialization
	public TCPServer(string host)
	{
		m_host = host;
		messages = new Queue<byte[]>();
	}

	public void StartListening()
    {
		tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
		tcpListenerThread.IsBackground = true;
		tcpListenerThread.Start();
	}

	/// <summary> 	
	/// Runs in background TcpServerThread; Handles incomming TcpClient requests 	
	/// </summary> 	
	private void ListenForIncommingRequests()
	{
		try
		{
			// Create listener on localhost port 8052. 			
			tcpListener = new TcpListener(IPAddress.Parse(m_host), 8156);
			tcpListener.Start();
			Debug.Log("Server is listening");
			
			//Debug.Log("hello");
			//if (connectedTcpClient == null)
			//{
			//	Debug.Log("hello1");
			//	connectedTcpClient = tcpListener.AcceptTcpClient();
			//	OnOpen?.Invoke();
			//}

			Byte[] bytes = new Byte[1024];
			//// Get a stream object for reading 		
			//NetworkStream stream = connectedTcpClient.GetStream();
			//while (true)
			//{	
			//	int length;
			//	// Read incomming stream into byte arrary. 						
			//	while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
			//	{
			//		var incommingData = new byte[length];
			//		Array.Copy(bytes, 0, incommingData, 0, length);
			//		Debug.Log("hello2");
			//		// OnMessage?.Invoke(incommingData);
			//		// Convert byte array to string message. 							
			//		//string clientMessage = Encoding.ASCII.GetString(incommingData);
			//		lock (messages)
			//                 {
			//			messages.Enqueue(incommingData);
			//		}
			//		//Debug.Log("client message received as: " + clientMessage);
			//	}
			//}
			connectedTcpClient = tcpListener.AcceptTcpClient();
			//{
				OnOpen?.Invoke();
				// Get a stream object for reading 					
				//using (NetworkStream stream = connectedTcpClient.GetStream())
				//{
					//	int length;
					//	// Read incomming stream into byte arrary. 						
					//	while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
					//	{
					//		var incommingData = new byte[length];
					//		Array.Copy(bytes, 0, incommingData, 0, length);
					//		lock (messages)
					//		{
					//			messages.Enqueue(incommingData);
					//		}
					//	}
			while (true)
            {
				ReadHeader();
			}
				//}
			//}
		}
		catch (SocketException socketException)
		{
			OnError?.Invoke(socketException);
		}
	}

	public void Close()
    {
		if (tcpListener != null)
		{
			if (connectedTcpClient != null)
			{
				connectedTcpClient.GetStream().Close();
				connectedTcpClient.Close();
			}
			tcpListenerThread.Abort();
			tcpListener.Stop();
			OnClose?.Invoke();
		}
    }

	public void Update()
    {
		lock (messages)
        {
			if (messages.Count > 0)
			{
				foreach(byte[] message in messages)
                {
					OnMessage?.Invoke(message);
				}
				messages.Clear();
			}
		}
    }

	/// <summary> 	
	/// Send message to client using socket connection. 	
	/// </summary> 	
	public void SendMessage(string message)
	{
		if (connectedTcpClient == null)
		{
			return;
		}

		try
		{
			// Get a stream object for writing. 	
			NetworkStream stream = connectedTcpClient.GetStream();
          
			if (stream.CanWrite)
			{
				// Convert string message to byte array.                 
				byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(message);
				byte[] header = BitConverter.GetBytes(serverMessageAsByteArray.Length);
				byte[] outgoingData = new byte[header.Length + serverMessageAsByteArray.Length];
				Array.Copy(header, 0, outgoingData, 0, header.Length);
				Array.Copy(serverMessageAsByteArray, 0, outgoingData, header.Length, serverMessageAsByteArray.Length);
				// Write byte array to socketConnection stream.                 
				stream.Write(outgoingData, 0, outgoingData.Length);
				Debug.Log("Server sent his message - should be received by client");
			}
		}
		catch (SocketException socketException)
		{
			OnError?.Invoke(socketException);
		}
	}

	public void ReadHeader()
	{
		NetworkStream stream = connectedTcpClient.GetStream();
		
		Int32 header;
		byte[] incommingData = new byte[sizeof(Int32)];

		try
		{
			int numberOfBytesRead = 0;

			while (numberOfBytesRead < incommingData.Length)
			{
				numberOfBytesRead += stream.Read(incommingData, numberOfBytesRead, incommingData.Length - numberOfBytesRead);
			}

			header = BitConverter.ToInt32(incommingData);
			if (header > 0)
			{
				Debug.Log("Reading body with length " + header);
				ReadBody(header);
			}
		}
		catch (Exception e)
		{
			Debug.Log(e);
		}
	}

	public void ReadBody(Int32 length)
	{
		NetworkStream stream = connectedTcpClient.GetStream();
		
		byte[] incommingData = new byte[length];

		try
		{
			int numberOfBytesRead = 0;

			while (numberOfBytesRead < incommingData.Length)
			{
				numberOfBytesRead += stream.Read(incommingData, numberOfBytesRead, incommingData.Length - numberOfBytesRead);
			}

			lock (messages)
			{
				messages.Enqueue(incommingData);
			}
		}
		catch (Exception e)
		{
			Debug.Log(e);
		}	
	}
}