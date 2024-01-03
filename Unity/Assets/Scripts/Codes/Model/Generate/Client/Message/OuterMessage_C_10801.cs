using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
//地图商店
	[ResponseType(nameof(AddOrUpdateTemplateALResponse))]
	[Message(OuterMessage.AddOrUpdateTemplateALRequest)]
	[ProtoContract]
	public partial class AddOrUpdateTemplateALRequest: ProtoObject, IUnitRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int TemplateType { get; set; }

		[ProtoMember(3)]
		public string Json { get; set; }

	}

	[Message(OuterMessage.AddOrUpdateTemplateALResponse)]
	[ProtoContract]
	public partial class AddOrUpdateTemplateALResponse: ProtoObject, IUnitResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(GetGridMapInfoALResponse))]
	[Message(OuterMessage.GetGridMapInfoALRequest)]
	[ProtoContract]
	public partial class GetGridMapInfoALRequest: ProtoObject, IUnitRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Page { get; set; }

	}

	[Message(OuterMessage.GetGridMapInfoALResponse)]
	[ProtoContract]
	public partial class GetGridMapInfoALResponse: ProtoObject, IUnitResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(2)]
		public int StartIndex { get; set; }

		[ProtoMember(3)]
		public List<GridMapProto> Infos { get; set; }

		[ProtoMember(4)]
		public int MaxPage { get; set; }

	}

	[ResponseType(nameof(GetMyMapsALResponse))]
	[Message(OuterMessage.GetMyMapsALRequest)]
	[ProtoContract]
	public partial class GetMyMapsALRequest: ProtoObject, IUnitRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Page { get; set; }

	}

	[Message(OuterMessage.GetMyMapsALResponse)]
	[ProtoContract]
	public partial class GetMyMapsALResponse: ProtoObject, IUnitResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(2)]
		public int StartIndex { get; set; }

		[ProtoMember(3)]
		public List<GridMapProto> Infos { get; set; }

		[ProtoMember(4)]
		public int MaxPage { get; set; }

	}

	[Message(OuterMessage.GridMapProto)]
	[ProtoContract]
	public partial class GridMapProto: ProtoObject
	{
		[ProtoMember(90)]
		public int Size { get; set; }

		[ProtoMember(2)]
		public long UnitID { get; set; }

		[ProtoMember(3)]
		public string DataURL { get; set; }

		[ProtoMember(4)]
		public string Name { get; set; }

		[ProtoMember(5)]
		public string Desc { get; set; }

		[ProtoMember(6)]
		public string MD5 { get; set; }

		[ProtoMember(7)]
		public long MapID { get; set; }

	}

	[ResponseType(nameof(RemoveTemplateALResponse))]
	[Message(OuterMessage.RemoveTemplateALRequest)]
	[ProtoContract]
	public partial class RemoveTemplateALRequest: ProtoObject, IUnitRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long TempID { get; set; }

	}

	[ResponseType(nameof(SaveMyMapALResponse))]
	[Message(OuterMessage.SaveMyMapALRequest)]
	[ProtoContract]
	public partial class SaveMyMapALRequest: ProtoObject, IUnitRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public GridMapProto Map { get; set; }

	}

	[Message(OuterMessage.SaveMyMapALResponse)]
	[ProtoContract]
	public partial class SaveMyMapALResponse: ProtoObject, IUnitResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(2)]
		public string Url { get; set; }

	}

	[Message(OuterMessage.SaveTemplateALResponse)]
	[ProtoContract]
	public partial class SaveTemplateALResponse: ProtoObject, IUnitResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterMessage.RemoveTemplateALResponse)]
	[ProtoContract]
	public partial class RemoveTemplateALResponse: ProtoObject, IUnitResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(UpdateTemplateDataALResponse))]
	[Message(OuterMessage.UpdateTemplateDataALRequest)]
	[ProtoContract]
	public partial class UpdateTemplateDataALRequest: ProtoObject, IUnitRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public List<string> Groups { get; set; }

	}

	[Message(OuterMessage.UpdateTemplateDataALResponse)]
	[ProtoContract]
	public partial class UpdateTemplateDataALResponse: ProtoObject, IUnitResponse
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
		 public const ushort AddOrUpdateTemplateALRequest = 10802;
		 public const ushort AddOrUpdateTemplateALResponse = 10803;
		 public const ushort GetGridMapInfoALRequest = 10804;
		 public const ushort GetGridMapInfoALResponse = 10805;
		 public const ushort GetMyMapsALRequest = 10806;
		 public const ushort GetMyMapsALResponse = 10807;
		 public const ushort GridMapProto = 10808;
		 public const ushort RemoveTemplateALRequest = 10809;
		 public const ushort SaveMyMapALRequest = 10810;
		 public const ushort SaveMyMapALResponse = 10811;
		 public const ushort SaveTemplateALResponse = 10812;
		 public const ushort RemoveTemplateALResponse = 10813;
		 public const ushort UpdateTemplateDataALRequest = 10814;
		 public const ushort UpdateTemplateDataALResponse = 10815;
	}
}
