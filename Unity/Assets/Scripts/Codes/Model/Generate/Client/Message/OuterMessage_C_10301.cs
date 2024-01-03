using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
//角色在地图中操作
	[Message(OuterMessage.BagItemArmsProto)]
	[ProtoContract]
	public partial class BagItemArmsProto: ProtoObject
	{
		[ProtoMember(1)]
		public int Equipped { get; set; }

		[ProtoMember(2)]
		public long UID { get; set; }

	}

	[Message(OuterMessage.BagItemProto)]
	[ProtoContract]
	public partial class BagItemProto: ProtoObject
	{
		[ProtoMember(1)]
		public int ID { get; set; }

		[ProtoMember(2)]
		public long Num { get; set; }

		[ProtoMember(3)]
		public long UID { get; set; }

	}

	[Message(OuterMessage.CreateMyUnitAMessage)]
	[ProtoContract]
	public partial class CreateMyUnitAMessage: ProtoObject, IActorMessage
	{
		[ProtoMember(1)]
		public UnitProto Unit { get; set; }

	}

	[Message(OuterMessage.CreateUnitsAMessage)]
	[ProtoContract]
	public partial class CreateUnitsAMessage: ProtoObject, IActorMessage
	{
		[ProtoMember(2)]
		public List<UnitProto> Units { get; set; }

	}

	[Message(OuterMessage.EnterBattleAMessage)]
	[ProtoContract]
	public partial class EnterBattleAMessage: ProtoObject, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int Seed { get; set; }

	}

	[ResponseType(nameof(EquipDownArmsALResponse))]
	[Message(OuterMessage.EquipDownArmsALRequest)]
	[ProtoContract]
	public partial class EquipDownArmsALRequest: ProtoObject, IUnitRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long UID { get; set; }

		[ProtoMember(3)]
		public int Profession { get; set; }

	}

	[Message(OuterMessage.FindPathALMessage)]
	[ProtoContract]
	public partial class FindPathALMessage: ProtoObject, IUnitMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public Unity.Mathematics.float3 Position { get; set; }

	}

	[Message(OuterMessage.EquipDownArmsALResponse)]
	[ProtoContract]
	public partial class EquipDownArmsALResponse: ProtoObject, IUnitResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(EquipUpArmsALResponse))]
	[Message(OuterMessage.EquipUpArmsALRequest)]
	[ProtoContract]
	public partial class EquipUpArmsALRequest: ProtoObject, IUnitRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long UID { get; set; }

		[ProtoMember(3)]
		public int Profession { get; set; }

		[ProtoMember(4)]
		public int Hole { get; set; }

	}

	[Message(OuterMessage.EquipmentProto)]
	[ProtoContract]
	public partial class EquipmentProto: ProtoObject
	{
		[ProtoMember(1)]
		public long UID { get; set; }

		[ProtoMember(2)]
		public int EquipUp { get; set; }

		[ProtoMember(3)]
		public int ProfessionNum { get; set; }

		[ProtoMember(4)]
		public int Hole { get; set; }

	}

	[Message(OuterMessage.FindPathResultAMessage)]
	[ProtoContract]
	public partial class FindPathResultAMessage: ProtoObject, IActorMessage
	{
		[ProtoMember(1)]
		public Unity.Mathematics.float3 Position { get; set; }

		[ProtoMember(2)]
		public List<Unity.Mathematics.float3> Points { get; set; }

		[ProtoMember(3)]
		public long unitId { get; set; }

	}

	[Message(OuterMessage.EquipUpArmsALResponse)]
	[ProtoContract]
	public partial class EquipUpArmsALResponse: ProtoObject, IUnitResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(GetInteractCodesALResponse))]
	[Message(OuterMessage.GetInteractCodesALRequest)]
	[ProtoContract]
	public partial class GetInteractCodesALRequest: ProtoObject, IUnitRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long Unit { get; set; }

	}

	[Message(OuterMessage.GetInteractCodesALResponse)]
	[ProtoContract]
	public partial class GetInteractCodesALResponse: ProtoObject, IUnitResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(2)]
		public List<int> Options { get; set; }

	}

	[ResponseType(nameof(InteractToResponse))]
	[Message(OuterMessage.InteractToRequest)]
	[ProtoContract]
	public partial class InteractToRequest: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long From { get; set; }

		[ProtoMember(3)]
		public long To { get; set; }

		[ProtoMember(4)]
		public int Option { get; set; }

	}

	[Message(OuterMessage.InteractToResponse)]
	[ProtoContract]
	public partial class InteractToResponse: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(LotteryALResponse))]
	[Message(OuterMessage.LotteryALRequest)]
	[ProtoContract]
	public partial class LotteryALRequest: ProtoObject, IUnitRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Count { get; set; }

		[ProtoMember(3)]
		public int Type { get; set; }

	}

	[Message(OuterMessage.MeetMonsterAMessage)]
	[ProtoContract]
	public partial class MeetMonsterAMessage: ProtoObject, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int MonsterGroupId { get; set; }

	}

	[Message(OuterMessage.LotteryALResponse)]
	[ProtoContract]
	public partial class LotteryALResponse: ProtoObject, IUnitResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(2)]
		public List<int> Result { get; set; }

	}

	[Message(OuterMessage.ProfessionProto)]
	[ProtoContract]
	public partial class ProfessionProto: ProtoObject
	{
		[ProtoMember(1)]
		public int Num { get; set; }

		[ProtoMember(2)]
		public long UID { get; set; }

	}

	[Message(OuterMessage.RemoveUnitsAMessage)]
	[ProtoContract]
	public partial class RemoveUnitsAMessage: ProtoObject, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public List<long> Units { get; set; }

	}

	[Message(OuterMessage.SkillUpdateAMessage)]
	[ProtoContract]
	public partial class SkillUpdateAMessage: ProtoObject, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public List<SkillProto> Skills { get; set; }

	}

	[Message(OuterMessage.TransferMapALMessage)]
	[ProtoContract]
	public partial class TransferMapALMessage: ProtoObject, IUnitMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int FromGateWay { get; set; }

	}

	[Message(OuterMessage.UnitBagAMessage)]
	[ProtoContract]
	public partial class UnitBagAMessage: ProtoObject, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public List<BagItemProto> Items { get; set; }

	}

	[Message(OuterMessage.UnitBagArmsAMessage)]
	[ProtoContract]
	public partial class UnitBagArmsAMessage: ProtoObject, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public List<BagItemArmsProto> Arms { get; set; }

	}

	[Message(OuterMessage.UnitDisconnectAMessage)]
	[ProtoContract]
	public partial class UnitDisconnectAMessage: ProtoObject, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long UnitID { get; set; }

	}

	[Message(OuterMessage.UnitEquipmentUpdateAMessage)]
	[ProtoContract]
	public partial class UnitEquipmentUpdateAMessage: ProtoObject, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public List<EquipmentProto> Equips { get; set; }

	}

	[Message(OuterMessage.UnitNumericUpdateAMessage)]
	[ProtoContract]
	public partial class UnitNumericUpdateAMessage: ProtoObject, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[MongoDB.Bson.Serialization.Attributes.BsonDictionaryOptions(MongoDB.Bson.Serialization.Options.DictionaryRepresentation.ArrayOfArrays)]
		[ProtoMember(2)]
		public Dictionary<int, long> Numeric { get; set; }
	}

	[Message(OuterMessage.UnitProfessionAMessage)]
	[ProtoContract]
	public partial class UnitProfessionAMessage: ProtoObject, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public List<ProfessionProto> Professions { get; set; }

	}

	[ResponseType(nameof(UseItemALResponse))]
	[Message(OuterMessage.UseItemALRequest)]
	[ProtoContract]
	public partial class UseItemALRequest: ProtoObject, IUnitRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long UID { get; set; }

		[ProtoMember(3)]
		public long Num { get; set; }

	}

	[Message(OuterMessage.UseItemALResponse)]
	[ProtoContract]
	public partial class UseItemALResponse: ProtoObject, IUnitResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterMessage.MapBuildTemplateAMessage)]
	[ProtoContract]
	public partial class MapBuildTemplateAMessage: ProtoObject, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public string TemplateJson { get; set; }

	}

	public static partial class OuterMessage
	{
		 public const ushort BagItemArmsProto = 10302;
		 public const ushort BagItemProto = 10303;
		 public const ushort CreateMyUnitAMessage = 10304;
		 public const ushort CreateUnitsAMessage = 10305;
		 public const ushort EnterBattleAMessage = 10306;
		 public const ushort EquipDownArmsALRequest = 10307;
		 public const ushort FindPathALMessage = 10308;
		 public const ushort EquipDownArmsALResponse = 10309;
		 public const ushort EquipUpArmsALRequest = 10310;
		 public const ushort EquipmentProto = 10311;
		 public const ushort FindPathResultAMessage = 10312;
		 public const ushort EquipUpArmsALResponse = 10313;
		 public const ushort GetInteractCodesALRequest = 10314;
		 public const ushort GetInteractCodesALResponse = 10315;
		 public const ushort InteractToRequest = 10316;
		 public const ushort InteractToResponse = 10317;
		 public const ushort LotteryALRequest = 10318;
		 public const ushort MeetMonsterAMessage = 10319;
		 public const ushort LotteryALResponse = 10320;
		 public const ushort ProfessionProto = 10321;
		 public const ushort RemoveUnitsAMessage = 10322;
		 public const ushort SkillUpdateAMessage = 10323;
		 public const ushort TransferMapALMessage = 10324;
		 public const ushort UnitBagAMessage = 10325;
		 public const ushort UnitBagArmsAMessage = 10326;
		 public const ushort UnitDisconnectAMessage = 10327;
		 public const ushort UnitEquipmentUpdateAMessage = 10328;
		 public const ushort UnitNumericUpdateAMessage = 10329;
		 public const ushort UnitProfessionAMessage = 10330;
		 public const ushort UseItemALRequest = 10331;
		 public const ushort UseItemALResponse = 10332;
		 public const ushort MapBuildTemplateAMessage = 10333;
	}
}
