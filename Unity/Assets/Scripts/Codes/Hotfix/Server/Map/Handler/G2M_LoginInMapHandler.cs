using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
    [ActorMessageHandler(SceneType.Map)]
    public class G2M_LoginInMapHandler : AMActorRpcHandler<Scene, G2M_LoginInMapARequest, G2M_LoginInMapAResponse>
    {
        protected override async ETTask Run(Scene scene, G2M_LoginInMapARequest request, G2M_LoginInMapAResponse response, Action reply)
        {
            var dbComponent = scene.GetComponent<DBComponent>();
            var unit = await dbComponent.Query<Unit>(request.Unit.UnitId);
            if (unit == null) // 新建
            {
                unit = UnitFactory.CreateNewUnit(scene, request.Unit);
                Log.Console($"[Map{scene.DomainScene()}]Create Unit {request.Unit.UnitId}");
            }
            else
            {
                scene.GetComponent<UnitComponent>().Add(unit);
                Log.Console($"Login Unit {request.Unit.UnitId}");
            }

            unit.GetComponent<UnitInfoComponent>().IsOnline = true;
            
            unit.AddComponent<MoveComponent>();
            unit.AddComponent<TeamComponent>();
            unit.AddComponent<MailBoxComponent, MailboxType>(MailboxType.UnitMessageDispatcher);
  
            unit.AddComponent<MeetMonsterComponent>();
            unit.AddComponent<UnitGateComponent>().GateSessionActorID = request.GateSessionActorID;
            
            unit.AddComponent<NumericComponent>();
            
            ET.UnitHelper.RefreshLevel(unit);
            ET.UnitHelper.SetUnitLevel(unit, unit.Level);
            
            unit.AddLocation().Coroutine();
            
            MessageHelper.SendToClient(unit, new StartSceneChangeAMessage(){MapId = unit.Map, MapActorId = unit.DomainScene().InstanceId});
            
            // 通知客户端创建My Unit
            MessageHelper.SendToClient(unit,  new CreateMyUnitAMessage
            {
                Unit = ET.UnitHelper.CreateUnitInfo(unit)
            });
            
            //推送背包
            var bagComponent = unit.GetComponent<BagComponent>();
            var bagList = new List<BagItemProto>();
            foreach (var kv in bagComponent.BagItems)
            {
                foreach (var child in kv.Value)
                {
                    bagList.Add(new BagItemProto()
                    {
                        ID = child.ItemID,
                        Num = child.Num,
                        UID = child.Id
                    });
                }
            }
            //推送背包
            bagComponent.SendBagItemChange(bagList);
            
            //推送技能
            // var skillComponent = unit.GetComponent<SkillComponent>();
            // var skillList = new List<SkillProto>();
            // foreach (var skill in skillComponent.AllSkill())
            // {
            //     //初始化被动技能词条
            //     if ((SkillType)skill.Config.Type == SkillType.Passive)
            //     {
            //         var words = skill.BaseConfig.EntryWord.ToIntArray(',');
            //         var wordVals = skill.BaseConfig.EntryWordArg.ToIntArray(',');
            //         for (int i = 0; i < words.Length; i++)
            //         {
            //             EntryWordHelper.Parse(unit, words[i], wordVals[i], OperatorType.Add);
            //         }
            //     }
            //
            //     skillList.Add(new SkillProto()
            //     {
            //         ID = skill.SkillId,
            //         Level = skill.Level
            //     });
            // }
         
            
            var professions = new List<ProfessionProto>();
            var equipUp = new List<EquipmentProto>();
            foreach (var profession in unit.GetChildren<Profession>())
            {  
                professions.Add(new ProfessionProto()
                {
                    UID = profession.Id,
                    Num = (int) profession.Num,
                });
                profession.GetComponent<ArmsComponent>().AfterDeserialize();
                foreach (var armsItem in profession.GetComponent<ArmsComponent>().GetChildren<ArmsItem>())
                {
                    equipUp.Add(new EquipmentProto()
                    {
                        EquipUp = 1,
                        Hole = (int)armsItem.Hole,
                        ProfessionNum = (int)profession.Num,
                        UID = armsItem.Id,
                    });
                }
            }
            MessageHelper.SendToClient(unit, new UnitProfessionAMessage()
            {
                Professions = professions
            });
            MessageHelper.SendToClient(unit, new UnitEquipmentUpdateAMessage()
            {
                Equips = equipUp
            });
            
            MessageHelper.SendToClient(unit, new UnitNumericUpdateAMessage()
            {
                Numeric = unit.GetComponent<NumericComponent>().NumericDic
            });
            
            unit.AddComponent<AOIEntity, int, float3>(9 * 1000, unit.Position);
            var newMap = unit.DomainScene().GetComponent<MapComponent>().GetMap(unit.Map);
            unit.AddComponent<PathfindingComponent, string>(newMap.MapConfig.SceneName);
            unit.StartSaveTimer();
            reply();
            await ETTask.CompletedTask;
        }
    }
}