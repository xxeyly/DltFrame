# -----项目介绍-----

Dlt 是一个基于Unity引擎的插件,对常用的功能进行的封装,可以更方便快捷并且稳定的开发,适合小型游戏或虚拟仿真的开发  
B站地址:【DltFrame-简介】 https://www.bilibili.com/video/BV1WrefeSELQ/share_source=copy_web&vd_source=c2ca7ea3abdba1820ca9747d640c7872

## 环境依赖

Odin - Inspector and Serializer

插件地址:https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041

## 插件功能:

### 1.UI框架

### 2.实体

### 3.事件监听

### 4.音频管理

### 5.计时器

### 6.场景管理

### 7.运行时数据

### 8.热更新(结合Hybridclr)

# 更新日志:

当前版本: 1.1.5

### V1.1.5

- **Hierarchy 新增**
    - R!黄色: 可以忽略，只是名称不一样，点击可自动更改名称。
    - R!红色: 不可忽略，继续使用会导致报错，名称不规范，需要手动更改名称。
    - H!红色: 不可忽略，继续使用会导致报错，子级中包含了热更脚本，点击H!会自动移除不规范内容。
- **Hierarchy 旧版**
    - H: 表示是热更。
    - E: 实体。
    - U: 表示要生成绑定脚本。
    - S: 场景组件。
    - 眼睛开: 当前实体显示状态。
    - 眼睛闭: 当前视图隐藏状态。
    - 开锁: 当前视图全局操作时会忽略。
    - 解锁: 当前视图全局操作时不会忽略。

### V1.1.4

- Hierarchy 备注名称增加划入划出效果，变更颜色统一处理。
- FrameComponent 执行顺序改为配置表内顺序。
- SceneComponentInit 修复添加热更文件问题。

### V1.1.3

- BaseWindow Hierarchy 图标位置调换。

### V1.1.2

- Hierarchy 图标变更。

### V1.1.1

- 绑定类型格式优化，去除多余命名空间名称。

### v1.1.0

- 调用方式改变。
