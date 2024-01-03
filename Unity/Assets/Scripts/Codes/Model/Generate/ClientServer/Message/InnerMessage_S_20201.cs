using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[ResponseType(nameof(G2M_LoginInMapAResponse))]
	[Message(InnerMessage.G2M_LoginInMapARequest)]
	[ProtoContract]
	public partial class G2M_LoginInMapARequest: ProtoObject, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public SimpleUnit Unit { get; set; }

		[ProtoMember(2)]
		public long GateSessionActorID { get; set; }

	}

	[Message(InnerMessage.G2M_LoginInMapAResponse)]
	[ProtoContract]
	public partial class G2M_LoginInMapAResponse: ProtoObject, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2M_LoginRobotToMapAResponse))]
	[Message(InnerMessage.G2M_LoginRobotToMapARequest)]
	[ProtoContract]
	public partial class G2M_LoginRobotToMapARequest: ProtoObject, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public SimpleUnit Robot { get; set; }

	}

	[Message(InnerMessage.G2M_LoginRobotToMapAResponse)]
	[ProtoContract]
	public partial class G2M_LoginRobotToMapAResponse: ProtoObject, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(InnerMessage.G2M_RemoveUnitAMessage)]
	[ProtoContract]
	public partial class G2M_RemoveUnitAMessage: ProtoObject, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

	}

	public static partial class InnerMessage
	{
		 public const ushort G2M_LoginInMapARequest = 20202;
		 public const ushort G2M_LoginInMapAResponse = 20203;
		 public const ushort G2M_LoginRobotToMapARequest = 20204;
		 public const ushort G2M_LoginRobotToMapAResponse = 20205;
		 public const ushort G2M_RemoveUnitAMessage = 20206;
	}
}
