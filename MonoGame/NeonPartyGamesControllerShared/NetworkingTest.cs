using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoEngine;
using NeonPartyGamesController.Enums;

namespace NeonPartyGamesController
{
	public class NetworkingTest : Entity, ITouchable
	{
		private readonly Socket Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		private readonly IPAddress IP = new IPAddress(new byte[] {192, 168, 1, 245});
		private byte[] Bytes = new byte[50];

		public NetworkingTest() {
			Debug.WriteLine("This is a test!");
			var sprite = new Sprite(SpriteSheetHolder.SpriteSheet.GetRegion("faces/character_0_0"));
			this.AddSprite("main", sprite);
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
			int x = MathHelper.Clamp((int)this.Position.X, 0, 1280);
			int y = MathHelper.Clamp((int)this.Position.Y, 0, 720);
			bool destroy = false;

			SetPlayerBytes(this.IP, 0, (ushort)x, (ushort)y, destroy, Settings.PlayerFace, Settings.PlayerColor, Settings.PlayerName, ref this.Bytes);
			this.SendUDP("192.168.1.111", 54321, this.Bytes);
		}

		public void SendUDP(string ip, int port, byte[] data) {
			IPEndPoint end_point = new IPEndPoint(IPAddress.Parse(ip), port);
			this.Socket.SendTo(data, end_point);
		}

		private void AddToBuffer(byte[] buffer, byte[] bytes, int index) {
			for (int i = 0; i < bytes.Length; i++) {
				buffer[index + i] = bytes[i];
			}
		}

		public void SetPlayerBytes(IPAddress ip, ushort id, ushort x, ushort y, bool destroy, Faces face, Colors color, string name, ref byte[] bytes) {
			if (bytes == null || bytes.Length != 50) {
				throw new Exception("Data array must be 50 bytes");
			}

			if (name.Length > Settings.MaxNameLength) {
				throw new Exception("Name must not be longer than MaxNameLength");
			}

			byte fill = 0;
			byte[] ip_bytes = ip.GetAddressBytes();
			byte[] id_bytes = BitConverter.GetBytes(id);
			byte[] x_bytes = BitConverter.GetBytes(x);
			byte[] y_bytes = BitConverter.GetBytes(y);
			byte destroy_byte = Convert.ToByte(destroy);
			byte face_byte = (byte)face;
			byte color_byte = (byte)color;
			byte name_length_byte = (byte)(uint)name.Length;
			byte[] name_bytes = Encoding.ASCII.GetBytes(name);

			bytes[0] = ip_bytes[0];
			bytes[1] = ip_bytes[1];
			bytes[2] = ip_bytes[2];
			bytes[3] = ip_bytes[3];
			bytes[4] = id_bytes[0];
			bytes[5] = id_bytes[1];
			bytes[6] = x_bytes[0];
			bytes[7] = x_bytes[1];
			bytes[8] = y_bytes[0];
			bytes[9] = y_bytes[1];
			bytes[10] = destroy_byte;
			bytes[11] = face_byte;
			bytes[12] = color_byte;
			bytes[13] = name_length_byte;
			for (int i = 0; i < bytes.Length - 14; i++)
				bytes[14 + i] = (i < name_bytes.Length) ? name_bytes[i] : fill;
		}
	}
}
