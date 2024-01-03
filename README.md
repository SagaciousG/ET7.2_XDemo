# 基于ET版本 7.2开发的个人项目框架核心
## 1.集成YooAsset，并拆分Art（未提交）和Main两个工程
## 2.修改了Excel的导出规则。每个xlsx为一组，每个Sheet为一个Config。sheet命名规则：描述文本@Config名称。可以用‘_’拆分同类型sheet，用法详见13_物品表。
## 3.提供PSD2UGUI工具和UI框架。UI框架完全遵守Entity-System规范，配套有各种复杂的UI组件实现。
## 4.Demo实现登录注册、创角选角、背包、装备、抽卡、GM等功能。

## 插件
### 1.ProtoBuf编辑器（Editor/ProtoBufEditor）
![img_1.png](img_1.png)
1.规定命名规范：采用(name) + (Request、Response、Message)的命名组合规则，对Actor类型消息
使用（name）+（ARequest、AResponse、AMessage），对ActorLocation则是（ALRequest）如此。  
2.选择消息类型，如果类型为Request类型，则点击类型旁边按钮一键创建（选择）关联的Response。  
3.当消息类型为None时，则该结构被是作为结构体，就会出现在字段类型选择中。如果要使用Unity中的类型
请勾选【UnityStruct】并填入类型名称，如果是非ET命名空间下的类型，请带上包名。  
4.支持迁移（M）克隆（C）删除（X）
### 2.Debug工具栏（ThirdParty/Console）
![img_2.png](img_2.png)
1.支持Runtime和Editor（快捷键F6）两种  
2.Log支持点击跳转到行  
3.协议工具使用OpcodeHelper.LogMsg自行接入。样例工程已经接好了，可以参考  
4.属性栏可以在Runtime中查看场景中的GameObject结构，能设置Active  
5.工具栏为一些操作扩展  
### 3.PSD2UGUI（Editor/PSD）
![img_3.png](img_3.png)
介绍请看[PSD2UGUI](https://blog.csdn.net/Sagacious_G/article/details/124145809)
### 4.UI
![img_4.png](img_4.png)

### 5.UGUI
1.提供一些UI的基础套件（UIList、Popup、PageScroll等）
### 6.ComponetView
![image](https://user-images.githubusercontent.com/49907344/195511113-64a0261b-d8f7-4777-9de8-8b42e07c73d8.png)

1.改进了ComponentView可以查看所有属性和字段  
2.实现TypeDraw扩展更多类型  
3.支持Dictionary、list、stack
### 7.NumericTypeEditor
![image](https://user-images.githubusercontent.com/49907344/195549206-973e480b-8659-4be6-a1c7-2a055460089b.png)
### 8.ETTreeViewr
![image](https://github.com/SagaciousG/ETPlugins/assets/49907344/adf35d26-af6d-4949-aec6-8cc4e28df0d6)


