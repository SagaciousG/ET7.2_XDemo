using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
//战斗测试
	[ResponseType(nameof(TestAddSkillALResponse))]
	[Message(OuterMessage.TestAddSkillALRequest)]
	[ProtoContract]
	public partial class TestAddSkillALRequest: ProtoObject, IUnitRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long UnitID { get; set; }

		[ProtoMember(3)]
		public SkillProto Skill { get; set; }

	}

	[Message(OuterMessage.TestAddSkillALResponse)]
	[ProtoContract]
	public partial class TestAddSkillALResponse: ProtoObject, IUnitResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(TestFightWithDummyALResponse))]
	[Message(OuterMessage.TestFightWithDummyALRequest)]
	[ProtoContract]
	public partial class TestFightWithDummyALRequest: ProtoObject, IUnitRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterMessage.TestFightWithDummyALResponse)]
	[ProtoContract]
	public partial class TestFightWithDummyALResponse: ProtoObject, IUnitResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(TestRemoveSkillALResponse))]
	[Message(OuterMessage.TestRemoveSkillALRequest)]
	[ProtoContract]
	public partial class TestRemoveSkillALRequest: ProtoObject, IUnitRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long UnitID { get; set; }

		[ProtoMember(3)]
		public int SkillID { get; set; }

	}

	[Message(OuterMessage.TestRemoveSkillALResponse)]
	[ProtoContract]
	public partial class TestRemoveSkillALResponse: ProtoObject, IUnitResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(TestRemoveUnitALResponse))]
	[Message(OuterMessage.TestRemoveUnitALRequest)]
	[ProtoContract]
	public partial class TestRemoveUnitALRequest: ProtoObject, IUnitRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long UnitID { get; set; }

	}

	[Message(OuterMessage.TestRemoveUnitALResponse)]
	[ProtoContract]
	public partial class TestRemoveUnitALResponse: ProtoObject, IUnitResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterMessage.TestUnitInfo)]
	[ProtoContract]
	public partial class TestUnitInfo: ProtoObject
	{
		[MongoDB.Bson.Serialization.Attributes.BsonDictionaryOptions(MongoDB.Bson.Serialization.Options.DictionaryRepresentation.ArrayOfArrays)]
		[ProtoMember(1)]
		public Dictionary<int, long> Numeric { get; set; }
		[ProtoMember(2)]
		public List<SkillProto> Skills { get; set; }

	}

	[ResponseType(nameof(TestUpdateNumericALResponse))]
	[Message(OuterMessage.TestUpdateNumericALRequest)]
	[ProtoContract]
	public partial class TestUpdateNumericALRequest: ProtoObject, IUnitRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long UnitID { get; set; }

		[MongoDB.Bson.Serialization.Attributes.BsonDictionaryOptions(MongoDB.Bson.Serialization.Options.DictionaryRepresentation.ArrayOfArrays)]
		[ProtoMember(3)]
		public Dictionary<int, long> Numeric { get; set; }
	}

	[ResponseType(nameof(TestUpdateOrCreateUnitALResponse))]
	[Message(OuterMessage.TestUpdateOrCreateUnitALRequest)]
	[ProtoContract]
	public partial class TestUpdateOrCreateUnitALRequest: ProtoObject, IUnitRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public BattleUnitProto Unit { get; set; }

	}

	[Message(OuterMessage.TestUpdateOrCreateUnitALResponse)]
	[ProtoContract]
	public partial class TestUpdateOrCreateUnitALResponse: ProtoObject, IUnitResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterMessage.TestUpdateNumericALResponse)]
	[ProtoContract]
	public partial class TestUpdateNumericALResponse: ProtoObject, IUnitResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	public static partial class OuterMessage
	{
		 public const ushort TestAddSkillALRequest = 10502;
		 public const ushort TestAddSkillALResponse = 10503;
		 public const ushort TestFightWithDummyALRequest = 10504;
		 public const ushort TestFightWithDummyALResponse = 10505;
		 public const ushort TestRemoveSkillALRequest = 10506;
		 public const ushort TestRemoveSkillALResponse = 10507;
		 public const ushort TestRemoveUnitALRequest = 10508;
		 public const ushort TestRemoveUnitALResponse = 10509;
		 public const ushort TestUnitInfo = 10510;
		 public const ushort TestUpdateNumericALRequest = 10511;
		 public const ushort TestUpdateOrCreateUnitALRequest = 10512;
		 public const ushort TestUpdateOrCreateUnitALResponse = 10513;
		 public const ushort TestUpdateNumericALResponse = 10514;
	}
}
