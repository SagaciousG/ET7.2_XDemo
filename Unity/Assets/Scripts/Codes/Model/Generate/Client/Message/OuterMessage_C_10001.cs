using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
//未整理协议
	[ResponseType(nameof(BenchmarkResponse))]
	[Message(OuterMessage.BenchmarkRequest)]
	[ProtoContract]
	public partial class BenchmarkRequest: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterMessage.BenchmarkResponse)]
	[ProtoContract]
	public partial class BenchmarkResponse: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_ReloadResponse))]
	[Message(OuterMessage.C2M_ReloadRequest)]
	[ProtoContract]
	public partial class C2M_ReloadRequest: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[ResponseType(nameof(M2C_TestALResponse))]
	[Message(OuterMessage.C2M_TestALRequest)]
	[ProtoContract]
	public partial class C2M_TestALRequest: ProtoObject, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string request { get; set; }

	}

	[ResponseType(nameof(M2C_TestRobotCaseALResponse))]
	[Message(OuterMessage.C2M_TestRobotCaseALRequest)]
	[ProtoContract]
	public partial class C2M_TestRobotCaseALRequest: ProtoObject, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int N { get; set; }

	}

	[ResponseType(nameof(GMALResponse))]
	[Message(OuterMessage.GMALRequest)]
	[ProtoContract]
	public partial class GMALRequest: ProtoObject, IUnitRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Code { get; set; }

		[ProtoMember(2)]
		public string P1 { get; set; }

		[ProtoMember(3)]
		public string P2 { get; set; }

		[ProtoMember(4)]
		public string P3 { get; set; }

	}

	[Message(OuterMessage.GMALResponse)]
	[ProtoContract]
	public partial class GMALResponse: ProtoObject, IUnitResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterMessage.HttpGetRouterResponse)]
	[ProtoContract]
	public partial class HttpGetRouterResponse: ProtoObject
	{
		[ProtoMember(1)]
		public List<string> Realms { get; set; }

		[ProtoMember(2)]
		public List<string> Routers { get; set; }

	}

	[Message(OuterMessage.M2C_ReloadResponse)]
	[ProtoContract]
	public partial class M2C_ReloadResponse: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterMessage.M2C_StopAMessage)]
	[ProtoContract]
	public partial class M2C_StopAMessage: ProtoObject, IActorMessage
	{
		[ProtoMember(1)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public long Id { get; set; }

		[ProtoMember(3)]
		public Unity.Mathematics.float3 Position { get; set; }

		[ProtoMember(4)]
		public Unity.Mathematics.quaternion Rotation { get; set; }

	}

	[Message(OuterMessage.M2C_TestALResponse)]
	[ProtoContract]
	public partial class M2C_TestALResponse: ProtoObject, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string response { get; set; }

	}

	[Message(OuterMessage.M2C_TestRobotCaseALResponse)]
	[ProtoContract]
	public partial class M2C_TestRobotCaseALResponse: ProtoObject, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int N { get; set; }

	}

	[Message(OuterMessage.MoveInfo)]
	[ProtoContract]
	public partial class MoveInfo: ProtoObject
	{
		[ProtoMember(1)]
		public List<Unity.Mathematics.float3> Points { get; set; }

		[ProtoMember(4)]
		public float A { get; set; }

		[ProtoMember(5)]
		public float B { get; set; }

		[ProtoMember(6)]
		public float C { get; set; }

		[ProtoMember(7)]
		public float W { get; set; }

		[ProtoMember(8)]
		public int TurnSpeed { get; set; }

	}

	[ResponseType(nameof(PingResponse))]
	[Message(OuterMessage.PingRequest)]
	[ProtoContract]
	public partial class PingRequest: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterMessage.PingResponse)]
	[ProtoContract]
	public partial class PingResponse: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long Time { get; set; }

	}

	[Message(OuterMessage.RouterSync)]
	[ProtoContract]
	public partial class RouterSync: ProtoObject
	{
		[ProtoMember(1)]
		public uint ConnectId { get; set; }

		[ProtoMember(2)]
		public string Address { get; set; }

	}

	[Message(OuterMessage.StopMoveALMessage)]
	[ProtoContract]
	public partial class StopMoveALMessage: ProtoObject, IUnitMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterMessage.UnitProto)]
	[ProtoContract]
	public partial class UnitProto: ProtoObject
	{
		[ProtoMember(1)]
		public Unity.Mathematics.float3 Position { get; set; }

		[ProtoMember(2)]
		public MoveInfo MoveInfo { get; set; }

		[ProtoMember(3)]
		public Unity.Mathematics.float3 Forward { get; set; }

		[ProtoMember(4)]
		public int GateWayId { get; set; }

		[ProtoMember(5)]
		public int Map { get; set; }

		[ProtoMember(6)]
		public SimpleUnit SimpleUnit { get; set; }

		[ProtoMember(7)]
		public int NPCID { get; set; }

		[ProtoMember(8)]
		public int MoveSpeed { get; set; }

	}

	public static partial class OuterMessage
	{
		 public const ushort BenchmarkRequest = 10002;
		 public const ushort BenchmarkResponse = 10003;
		 public const ushort C2M_ReloadRequest = 10004;
		 public const ushort C2M_TestALRequest = 10005;
		 public const ushort C2M_TestRobotCaseALRequest = 10006;
		 public const ushort GMALRequest = 10007;
		 public const ushort GMALResponse = 10008;
		 public const ushort HttpGetRouterResponse = 10009;
		 public const ushort M2C_ReloadResponse = 10010;
		 public const ushort M2C_StopAMessage = 10011;
		 public const ushort M2C_TestALResponse = 10012;
		 public const ushort M2C_TestRobotCaseALResponse = 10013;
		 public const ushort MoveInfo = 10014;
		 public const ushort PingRequest = 10015;
		 public const ushort PingResponse = 10016;
		 public const ushort RouterSync = 10017;
		 public const ushort StopMoveALMessage = 10018;
		 public const ushort UnitProto = 10019;
	}
}
