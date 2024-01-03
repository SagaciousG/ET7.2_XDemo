namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class EffectComponent : Entity, IAwake
    {
        public static EffectComponent Instance { get; set; }
    }
}