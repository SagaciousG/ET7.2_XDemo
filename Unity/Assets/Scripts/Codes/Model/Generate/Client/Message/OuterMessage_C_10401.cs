using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
//战斗
	[Message(OuterMessage.BattleBroadcastStateAMessage)]
	[ProtoContract]
	public partial class BattleBroadcastStateAMessage: ProtoObject, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[MongoDB.Bson.Serialization.Attributes.BsonDictionaryOptions(MongoDB.Bson.Serialization.Options.DictionaryRepresentation.ArrayOfArrays)]
		[ProtoMember(1)]
		public Dictionary<long, BattleUnitState> Units { get; set; }
	}

	[Message(OuterMessage.BattleCommandInput)]
	[ProtoContract]
	public partial class BattleCommandInput: ProtoObject
	{
		[ProtoMember(1)]
		public long Target { get; set; }

		[ProtoMember(2)]
		public int CommandType { get; set; }

		[ProtoMember(3)]
		public List<int> Params { get; set; }

	}

	[Message(OuterMessage.BattleCommandResultAMessage)]
	[ProtoContract]
	public partial class BattleCommandResultAMessage: ProtoObject, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterMessage.BattleCommandResult)]
	[ProtoContract]
	public partial class BattleCommandResult: ProtoObject
	{
		[ProtoMember(1)]
		public BattleCommandInput Input { get; set; }

		[ProtoMember(2)]
		public long Director { get; set; }

	}

	[Message(OuterMessage.BattleCreateMonsterAMessage)]
	[ProtoContract]
	public partial class BattleCreateMonsterAMessage: ProtoObject, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public List<BattleMonsterInfo> Monsters { get; set; }

	}

	[Message(OuterMessage.BattleCreateUnitAMessage)]
	[ProtoContract]
	public partial class BattleCreateUnitAMessage: ProtoObject, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public List<BattleUnitProto> Units { get; set; }

	}

	[Message(OuterMessage.BattleMonsterInfo)]
	[ProtoContract]
	public partial class BattleMonsterInfo: ProtoObject
	{
		[ProtoMember(1)]
		public long ID { get; set; }

		[ProtoMember(2)]
		public int Pos { get; set; }

		[ProtoMember(3)]
		public int CfgID { get; set; }

	}

	[Message(OuterMessage.BattleReadyALMessage)]
	[ProtoContract]
	public partial class BattleReadyALMessage: ProtoObject, IBattleMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

	}

	[Message(OuterMessage.BattleRoundResultAMessage)]
	[ProtoContract]
	public partial class BattleRoundResultAMessage: ProtoObject, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public List<BattleCommandResult> Results { get; set; }

		[ProtoMember(3)]
		public int Round { get; set; }

	}

	[Message(OuterMessage.BattleRoundStartAMessage)]
	[ProtoContract]
	public partial class BattleRoundStartAMessage: ProtoObject, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int Round { get; set; }

	}

	[Message(OuterMessage.BattleRoundTimerOutAMessage)]
	[ProtoContract]
	public partial class BattleRoundTimerOutAMessage: ProtoObject, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[ResponseType(nameof(BattleUnitCommandALResponse))]
	[Message(OuterMessage.BattleUnitCommandALRequest)]
	[ProtoContract]
	public partial class BattleUnitCommandALRequest: ProtoObject, IBattleRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public List<BattleCommandInput> Input { get; set; }

		[ProtoMember(3)]
		public long Director { get; set; }

	}

	[Message(OuterMessage.BattleUnitCommandALResponse)]
	[ProtoContract]
	public partial class BattleUnitCommandALResponse: ProtoObject, IBattleResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterMessage.BattleUnitProto)]
	[ProtoContract]
	public partial class BattleUnitProto: ProtoObject
	{
		[ProtoMember(90)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public int Pos { get; set; }

		[ProtoMember(3)]
		public string Name { get; set; }

		[ProtoMember(4)]
		public int UnitShow { get; set; }

		[ProtoMember(5)]
		public int Level { get; set; }

		[MongoDB.Bson.Serialization.Attributes.BsonDictionaryOptions(MongoDB.Bson.Serialization.Options.DictionaryRepresentation.ArrayOfArrays)]
		[ProtoMember(6)]
		public Dictionary<int, long> Numeric { get; set; }
		[ProtoMember(7)]
		public List<SkillProto> Skill { get; set; }

		[ProtoMember(8)]
		public int UnitType { get; set; }

		[ProtoMember(9)]
		public int Map { get; set; }

	}

	[Message(OuterMessage.BattleUnitState)]
	[ProtoContract]
	public partial class BattleUnitState: ProtoObject
	{
		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public int Ready { get; set; }

	}

	public static partial class OuterMessage
	{
		 public const ushort BattleBroadcastStateAMessage = 10402;
		 public const ushort BattleCommandInput = 10403;
		 public const ushort BattleCommandResultAMessage = 10404;
		 public const ushort BattleCommandResult = 10405;
		 public const ushort BattleCreateMonsterAMessage = 10406;
		 public const ushort BattleCreateUnitAMessage = 10407;
		 public const ushort BattleMonsterInfo = 10408;
		 public const ushort BattleReadyALMessage = 10409;
		 public const ushort BattleRoundResultAMessage = 10410;
		 public const ushort BattleRoundStartAMessage = 10411;
		 public const ushort BattleRoundTimerOutAMessage = 10412;
		 public const ushort BattleUnitCommandALRequest = 10413;
		 public const ushort BattleUnitCommandALResponse = 10414;
		 public const ushort BattleUnitProto = 10415;
		 public const ushort BattleUnitState = 10416;
	}
}
