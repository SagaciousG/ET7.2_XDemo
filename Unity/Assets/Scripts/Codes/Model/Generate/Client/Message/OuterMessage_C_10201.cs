using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
//登录到
	[Message(OuterMessage.StartSceneChangeAMessage)]
	[ProtoContract]
	public partial class StartSceneChangeAMessage: ProtoObject, IActorMessage
	{
		[ProtoMember(1)]
		public int MapId { get; set; }

		[ProtoMember(2)]
		public long MapActorId { get; set; }

	}

	public static partial class OuterMessage
	{
		 public const ushort StartSceneChangeAMessage = 10202;
	}
}
