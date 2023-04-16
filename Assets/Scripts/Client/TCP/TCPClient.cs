using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPClient 
{	
	private TcpClient socketConnection;
	private Thread clientReceiveThread;
	private string m_IP;
	private int m_port = 8156;

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
	public TCPClient(string IP)
	{
		m_IP = IP;
		messages = new Queue<byte[]>();
	}

	/// <summary> 	
	/// Setup socket connection. 	
	/// </summary> 	
	public void ConnectToTcpServer()
	{
		try
		{
			clientReceiveThread = new Thread(new ThreadStart(ListenForData));
			clientReceiveThread.IsBackground = true;
			clientReceiveThread.Start();
		}
		catch (Exception e)
		{
			Debug.Log("On client connect exception " + e);
		}
	}
	/// <summary> 	
	/// Runs in background clientReceiveThread; Listens for incomming data. 	
	/// </summary>     
	private void ListenForData()
	{
		try
		{
			socketConnection = new TcpClient(m_IP, m_port);
			OnOpen?.Invoke();

			Byte[] bytes = new Byte[1024];
			while (true)
			{
				// Get a stream object for reading 				
				//using (NetworkStream stream = socketConnection.GetStream())
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
					//		// Convert byte array to string message. 						
					//		//string serverMessage = Encoding.ASCII.GetString(incommingData);
					//		//Debug.Log("server message received as: " + serverMessage);
					//	}

					ReadHeader();
				//}
			}
		}
		catch (SocketException socketException)
		{
			OnError?.Invoke(socketException);
			//Debug.Log("Socket exception: " + socketException);
		}
	}

	public void Update()
	{
		lock (messages)
		{
			if (messages.Count > 0)
			{
				foreach (byte[] message in messages)
				{
					OnMessage?.Invoke(message);
				}
				messages.Clear();
			}
		}
	}

	public void Close()
    {
		if (socketConnection != null)
        {
			clientReceiveThread.Abort();
			if (socketConnection.GetStream() != null)
            {
				socketConnection.GetStream().Close();
			}
			socketConnection.Close();
			OnClose?.Invoke();
		}
    }

	/// <summary> 	
	/// Send message to server using socket connection. 	
	/// </summary> 	
	public void SendMessage(string message)
	{
		if (socketConnection == null)
		{
			return;
		}
		try
		{
			// Get a stream object for writing. 	
			NetworkStream stream = socketConnection.GetStream();
            
			if (stream.CanWrite)
			{
				// Convert string message to byte array.                 
				byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(message);
				byte[] header = BitConverter.GetBytes(clientMessageAsByteArray.Length);
				byte[] outgoingData = new byte[header.Length + clientMessageAsByteArray.Length];
				Array.Copy(header, 0, outgoingData, 0, header.Length);
				Array.Copy(clientMessageAsByteArray, 0, outgoingData, header.Length, clientMessageAsByteArray.Length);
				// Write byte array to socketConnection stream.                 
				stream.Write(outgoingData, 0, outgoingData.Length);
				Debug.Log("Client sent his message - should be received by server");
			}
		}
		catch (SocketException socketException)
		{
			OnError?.Invoke(socketException);
		}
	}

	public void ReadHeader()
    {
		NetworkStream stream = socketConnection.GetStream();
		
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
		NetworkStream stream = socketConnection.GetStream();
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