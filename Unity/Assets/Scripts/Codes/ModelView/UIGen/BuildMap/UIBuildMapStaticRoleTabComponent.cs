//自动生成文件，请勿在此编辑
using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
	[ComponentOf(typeof(UI))]
	public partial class UIBuildMapStaticRoleTabComponent : Entity, IAwake, IUIComponent
	{
		public UIList modelList;
		public XInputField name;
		public XPopup faction;
		public XPopup passable;
		public RectTransform storeLine;
		public RectTransform towerLine;
		public RectTransform spawnLine;
		public XPopup spawnPoint;
		public XPopup spawnNPC;
		public XInputField spawnInterval;
		public XInputField spawnNum;
		public RectTransform transferLine;
		public RectTransform dropContent;
		public RectTransform content;
		public XImage property;
		public XText propertyLabel;
		public XImage function;
		public XText functionLabel;
		public XSubUI tools;
		public UI toolsUI;

	}
}
