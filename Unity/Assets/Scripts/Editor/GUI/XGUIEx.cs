using UnityEditor;

namespace ET
{
	public partial class XGUI
	{
		[MenuItem("GameObject/XGUI/XButton", priority = 0)]
		public static void XButton(MenuCommand command)
		{
			Create(command, "XButton");
		}
		[MenuItem("GameObject/XGUI/XDropdown", priority = 0)]
		public static void XDropdown(MenuCommand command)
		{
			Create(command, "XDropdown");
		}
		[MenuItem("GameObject/XGUI/XImage", priority = 0)]
		public static void XImage(MenuCommand command)
		{
			Create(command, "XImage");
		}
		[MenuItem("GameObject/XGUI/XInputField", priority = 0)]
		public static void XInputField(MenuCommand command)
		{
			Create(command, "XInputField");
		}
		[MenuItem("GameObject/XGUI/XMarqueue", priority = 0)]
		public static void XMarqueue(MenuCommand command)
		{
			Create(command, "XMarqueue");
		}
		[MenuItem("GameObject/XGUI/XRawImage", priority = 0)]
		public static void XRawImage(MenuCommand command)
		{
			Create(command, "XRawImage");
		}
		[MenuItem("GameObject/XGUI/XScroll View", priority = 0)]
		public static void XScrollView(MenuCommand command)
		{
			Create(command, "XScroll View");
		}
		[MenuItem("GameObject/XGUI/XSlider", priority = 0)]
		public static void XSlider(MenuCommand command)
		{
			Create(command, "XSlider");
		}
		[MenuItem("GameObject/XGUI/XText", priority = 0)]
		public static void XText(MenuCommand command)
		{
			Create(command, "XText");
		}
		[MenuItem("GameObject/XGUI/XToggle", priority = 0)]
		public static void XToggle(MenuCommand command)
		{
			Create(command, "XToggle");
		}
		[MenuItem("GameObject/XGUI/XUIList", priority = 0)]
		public static void XUIList(MenuCommand command)
		{
			Create(command, "XUIList");
		}
		[MenuItem("GameObject/XGUI/XUIRoot", priority = 0)]
		public static void XUIRoot(MenuCommand command)
		{
			Create(command, "XUIRoot");
		}
	}
}
