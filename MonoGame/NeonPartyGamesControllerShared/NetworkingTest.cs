using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Xna.Framework.Input.Touch;
using MonoEngine;

namespace NeonPartyGamesController
{
	public class NetworkingTest : Entity, ITouchable
	{
		public NetworkingTest() {
			Debug.WriteLine("This is a test!");
		}

		public override void onUpdate(float dt) {
			base.onUpdate(dt);
			this.SendPlayerData();
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
			string name = "test";

			ushort id = 0;
			ushort x = (ushort)this.Position.X;
			ushort y = (ushort)this.Position.Y;
			byte is_new = 0;
			byte character = 0;
			byte color = 0;
			byte name_length = (byte)(uint)name.Length;
			byte[] name_bytes = Encoding.ASCII.GetBytes(name);

			List<byte> send_buffer = new List<byte>();
			send_buffer.Add(192);
			send_buffer.Add(168);
			send_buffer.Add(1);
			send_buffer.Add(245);
			send_buffer.AddRange(BitConverter.GetBytes(id));
			send_buffer.AddRange(BitConverter.GetBytes(x));
			send_buffer.AddRange(BitConverter.GetBytes(y));
			send_buffer.Add(is_new);
			send_buffer.Add(character);
			send_buffer.Add(color);
			send_buffer.Add(name_length);
			send_buffer.AddRange(name_bytes);
			while (send_buffer.Count < 50) {
				send_buffer.Add(0);
			}

			this.SendUDP("192.168.1.111", 54321, send_buffer.ToArray());
		}

		public void SendUDP(string ip, int port, byte[] data) {
			Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Udp);
			IPAddress server_addr = IPAddress.Parse(ip);
			IPEndPoint end_point = new IPEndPoint(server_addr, port);
			sock.SendTo(data , end_point);
		}

		private void AddToBuffer(byte[] buffer, byte[] bytes, int index) {
			for (int i = 0; i < bytes.Length; i++) {
				buffer[index + i] = bytes[i];
			}
		}
	}
}
