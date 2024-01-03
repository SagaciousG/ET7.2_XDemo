using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
//机器人
	[ResponseType(nameof(GetAllRobotResponse))]
	[Message(OuterMessage.GetAllRobotRequest)]
	[ProtoContract]
	public partial class GetAllRobotRequest: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterMessage.GetAllRobotResponse)]
	[ProtoContract]
	public partial class GetAllRobotResponse: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(2)]
		public List<RobotInfo> AllRobot { get; set; }

	}

	[ResponseType(nameof(LoginRobotResponse))]
	[Message(OuterMessage.LoginRobotRequest)]
	[ProtoContract]
	public partial class LoginRobotRequest: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long Account { get; set; }

	}

	[Message(OuterMessage.LoginRobotResponse)]
	[ProtoContract]
	public partial class LoginRobotResponse: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterMessage.RobotInfo)]
	[ProtoContract]
	public partial class RobotInfo: ProtoObject
	{
		[ProtoMember(1)]
		public int Online { get; set; }

		[ProtoMember(2)]
		public long Account { get; set; }

		[ProtoMember(3)]
		public string Name { get; set; }

	}

	public static partial class OuterMessage
	{
		 public const ushort GetAllRobotRequest = 10702;
		 public const ushort GetAllRobotResponse = 10703;
		 public const ushort LoginRobotRequest = 10704;
		 public const ushort LoginRobotResponse = 10705;
		 public const ushort RobotInfo = 10706;
	}
}
