using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
//技能相关
	[Message(OuterMessage.SkillProto)]
	[ProtoContract]
	public partial class SkillProto: ProtoObject
	{
		[ProtoMember(1)]
		public int ID { get; set; }

		[ProtoMember(2)]
		public int Level { get; set; }

	}

	[ResponseType(nameof(UseSkillAResponse))]
	[Message(OuterMessage.UseSkillARequest)]
	[ProtoContract]
	public partial class UseSkillARequest: ProtoObject, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public SkillProto Skill { get; set; }

		[ProtoMember(2)]
		public long Target { get; set; }

		[ProtoMember(3)]
		public long Caster { get; set; }

	}

	[Message(OuterMessage.UseSkillAResponse)]
	[ProtoContract]
	public partial class UseSkillAResponse: ProtoObject, IActorResponse
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
		 public const ushort SkillProto = 10602;
		 public const ushort UseSkillARequest = 10603;
		 public const ushort UseSkillAResponse = 10604;
	}
}
