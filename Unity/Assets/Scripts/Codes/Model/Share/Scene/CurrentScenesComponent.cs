using System.Collections.Generic;
using ET.Client;

namespace ET
{
    // 可以用来管理多个客户端场景，比如大世界会加载多块场景
    [ComponentOf(typeof(Scene))]
    public class CurrentScenesComponent: Entity, IAwake
    {
        public Scene Current { get; set; }
        public Scene BattleScene { get; set; }
        public Scene BuildScene { get; set; }
        public Scene GameScene { get; set; }
        public int MapId { get; set; }

    }
}