using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoEngine;

namespace NeonPartyGamesController
{
	public class NetworkingTest : Entity, ITouchable
	{
		public const uint MaxNameLength = 25;

		public NetworkingTest() {
			Debug.WriteLine("This is a test!");
		}

		public override void onUpdate(float dt) {
			base.onUpdate(dt);
			this.SendPlayerData();
		}

		public override void onMouse(MouseState state) {
			base.onMouse(state);
			var pos = state.Position;
			if (pos.X >= 0 && pos.X <= 1280 && pos.Y >= 0 && pos.Y <= 720)
				this.Position = state.Position.ToVector2();
		}

		public void onTouchPressed(TouchLocation touch) {
		}

		public void onTouch(TouchCollection touch) {
			if (touch.Count > 0) {
				this.Position = touch[0].Position;
			}
		}

		public void onTouchReleased(TouchLocation touch) {
		}

		public void SendPlayerData() {
			string name = "Austin";

			ushort id = 0;
			ushort x = (ushort)this.Position.X;
			ushort y = (ushort)this.Position.Y;
			byte destroy = 0;
			byte character = 0;
			byte color = 0;
			uint name_length = (uint)name.Length;
			if (name_length > NetworkingTest.MaxNameLength)
				name_length = NetworkingTest.MaxNameLength;
			byte[] name_bytes = Encoding.ASCII.GetBytes(name);

			List<byte> send_buffer = new List<byte>();
			send_buffer.Add(192);
			send_buffer.Add(168);
			send_buffer.Add(1);
			send_buffer.Add(245);
			send_buffer.AddRange(BitConverter.GetBytes(id));
			send_buffer.AddRange(BitConverter.GetBytes(x));
			send_buffer.AddRange(BitConverter.GetBytes(y));
			send_buffer.Add(destroy);
			send_buffer.Add(character);
			send_buffer.Add(color);
			send_buffer.Add((byte)name_length);
			send_buffer.AddRange(name_bytes);
			for (int i = 0; i < NetworkingTest.MaxNameLength - name_length; i++) {
				send_buffer.Add(0);
			}

			byte[] bytes = new byte[50];
			int index = 0;
			while (index < bytes.Length && index < send_buffer.Count) {
				bytes[index] = send_buffer[index];
				index++;
			}

//			for (int i = 0; i < bytes.Length; i++) {
//				Debug.WriteLine((uint)bytes[i]);
//			}
//			Debug.WriteLine("############################");

			this.SendUDP("192.168.1.111", 54321, bytes);
		}

		public void SendUDP(string ip, int port, byte[] data) {
			Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			IPAddress server_addr = IPAddress.Parse(ip);
			IPEndPoint end_point = new IPEndPoint(server_addr, port);
			sock.SendTo(data, end_point);
		}

		private void AddToBuffer(byte[] buffer, byte[] bytes, int index) {
			for (int i = 0; i < bytes.Length; i++) {
				buffer[index + i] = bytes[i];
			}
		}
	}
}
