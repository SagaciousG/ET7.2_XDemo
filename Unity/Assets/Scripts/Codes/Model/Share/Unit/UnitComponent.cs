using System.Collections.Generic;

namespace ET
{
	
	[ComponentOf(typeof(Scene))]
	public class UnitComponent: Entity, IAwake, IDestroy
	{
		public List<Unit> Units { get; set; } = new();
		public MultiMap<UnitType, Unit> TypeUnits { get; set; } = new();
	}
}