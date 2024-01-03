using System.IO;
using System.Linq;

namespace ET.Server
{
    [FriendOf(typeof(UnitNameComponent))]
    public static class UnitNameComponentSystem
    {
        public class UnitNameComponentAwakeSystem : AwakeSystem<UnitNameComponent>
        {
            protected override async void Awake(UnitNameComponent self)
            {
                var names = File.ReadAllLines("./Config/Document/Names.txt");
                var dbComponent = self.DomainScene().GetComponent<DBComponent>();
                var res = await dbComponent.Query<UnitNameComponent>(a => true);
                if (res.Count > 0)
                {
                    foreach (var name in res[0].UsingNames)
                    {
                        self.UsingNamesSet.Add(name);
                    }
                }
                foreach (string name in names)
                {
                    self.DefinedNames.Add(name);
                    if (self.UsingNamesSet.Contains(name))
                    {
                        continue;
                    }

                    self.UnusedNames.Add(name);
                }
            }
        }

        public static bool IsUsing(this UnitNameComponent self, string name)
        {
            return self.UsingNamesSet.Contains(name);
        }

        public static void Use(this UnitNameComponent self, string name)
        {
            self.UnusedNames.Remove(name);
            self.UsingNamesSet.Add(name);
        }

        public static void Rename(this UnitNameComponent self, string oldName, string newName)
        {
            self.UsingNamesSet.Remove(newName);
            self.UnusedNames.Remove(oldName);
            if (self.DefinedNames.Contains(oldName))
                self.UnusedNames.Add(oldName);
            self.UsingNames.Add(newName);
        }

        public static string RandomGet(this UnitNameComponent self)
        {
            return RandomGenerator.RandomArray(self.UnusedNames.ToList());
        }

        public static ETTask Save(this UnitNameComponent self)
        {
            self.UsingNames.Clear();
            foreach (string s in self.UsingNamesSet)
            {
                self.UsingNames.Add(s);    
            }
            var dbComponent = self.DomainScene().GetComponent<DBComponent>();
            return dbComponent.Save(self);
        }
    }
}