using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    public class CircuitComponent : ComponentBase
    {
        public static CircuitComponent Instance;

        [BoxGroup] [TableList(AlwaysExpanded = true, DrawScrollView = false)] [LabelText("场景缓存流程数据")]
        public CircuitTempData circuitTempData = new CircuitTempData();

        [LabelText("上一个流程")] private Type _lastCircuitBaseData;
        [LabelText("场景流程")] public List<CircuitBaseData> sceneCircuitBaseData;
        private Dictionary<Type, CircuitBaseData> _allCircuitBaseDataDic;
        [LabelText("流程执行中")] private bool _inExecution;

        public override void StartComponent()
        {
            Instance = GetComponent<CircuitComponent>();
        }

        public override void InitComponent()
        {
            ViewComponent.Instance.onShowView += OnViewShow;
            ViewComponent.Instance.onHideView += OnHideShow;
            TimeComponent.Instance.onAddTimeTask += OnTimeAddTimeTask;
            TimeComponent.Instance.onDeleteTimeTask += OnTimeDeleteTimeTask;
            TimeComponent.Instance.onAddSwitchTask += OnTimeAddSwitchTimeTask;
            TimeComponent.Instance.onDeleteSwitchTask += OnTimeDeleteSwitchTimeTask;
            EntityComponent.Instance.onShowEntity += OnShowEntity;
            EntityComponent.Instance.onHideEntity += OnHideEntity;
            _allCircuitBaseDataDic = new Dictionary<Type, CircuitBaseData>();
            sceneCircuitBaseData = DataComponent.GetInheritAllSubclass<CircuitBaseData>();
            foreach (CircuitBaseData circuitBaseData in sceneCircuitBaseData)
            {
                _allCircuitBaseDataDic.Add(circuitBaseData.GetType(), circuitBaseData);
            }
        }

        public override void EndComponent()
        {
            ViewComponent.Instance.onShowView -= OnViewShow;
            ViewComponent.Instance.onHideView -= OnHideShow;
            TimeComponent.Instance.onAddTimeTask -= OnTimeAddTimeTask;
            TimeComponent.Instance.onDeleteTimeTask -= OnTimeDeleteTimeTask;
            TimeComponent.Instance.onAddSwitchTask -= OnTimeAddSwitchTimeTask;
            TimeComponent.Instance.onDeleteSwitchTask -= OnTimeDeleteSwitchTimeTask;
            EntityComponent.Instance.onShowEntity -= OnShowEntity;
            EntityComponent.Instance.onHideEntity -= OnHideEntity;
        }

        /// <summary>
        /// 执行流程
        /// </summary>
        /// <param name="circuitType"></param>
        public void StartCircuit(Type circuitType)
        {
            EndCircuit();
            _inExecution = true;
            _lastCircuitBaseData = circuitType;
            _allCircuitBaseDataDic[circuitType].StartCircuit();
        }

        /// <summary>
        /// 停止流程
        /// </summary>
        public void EndCircuit()
        {
            if (_lastCircuitBaseData != null)
            {
                _allCircuitBaseDataDic[_lastCircuitBaseData].EndCircuit();
            }

            //隐藏视图
            Type[] viewType = DataComponent.DataValueClone(circuitTempData.activityViewType).ToArray();
            ViewComponent.Instance.HideView(viewType);
            //结束时间监听
            List<int> timeTask = DataComponent.DataValueClone(circuitTempData.timeTask);
            TimeComponent.Instance.DeleteTimeTask(timeTask);
            circuitTempData.timeTask.Clear();
            List<int> switchTimeTask = DataComponent.DataValueClone(circuitTempData.switchTask);
            for (int i = 0; i < switchTimeTask.Count; i++)
            {
                TimeComponent.Instance.DeleteSwitchTask(switchTimeTask[i]);
            }

            circuitTempData.switchTask.Clear();
            //隐藏实体
            EntityComponent.Instance.DisplayEntityByEntityName(false, DataComponent.DataValueClone(circuitTempData.entity).ToArray());
            circuitTempData.entity.Clear();
            //结束流程控制
            _inExecution = false;
        }

        /// <summary>
        /// 隐藏实体
        /// </summary>
        /// <param name="entityName"></param>
        private void OnHideEntity(string entityName)
        {
            if (!_inExecution)
            {
                return;
            }

            for (int i = 0; i < circuitTempData.entity.Count; i++)
            {
                if (circuitTempData.entity[i] == entityName)
                {
                    circuitTempData.entity.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// 显示实体
        /// </summary>
        /// <param name="entityName"></param>
        private void OnShowEntity(string entityName)
        {
            if (!_inExecution)
            {
                return;
            }

            circuitTempData.entity.Add(entityName);
        }

        /// <summary>
        /// 隐藏实体
        /// </summary>
        /// <param name="type"></param>
        private void OnHideShow(Type type)
        {
            if (!_inExecution)
            {
                return;
            }

            for (int i = 0; i < circuitTempData.activityViewType.Count; i++)
            {
                if (circuitTempData.activityViewType[i] == type)
                {
                    circuitTempData.activityViewType.Remove(circuitTempData.activityViewType[i]);
                    return;
                }
            }
        }

        /// <summary>
        /// 显示视图
        /// </summary>
        /// <param name="type"></param>
        private void OnViewShow(Type type)
        {
            if (!_inExecution)
            {
                return;
            }

            circuitTempData.activityViewType.Add(type);
        }

        /// <summary>
        /// 增加监听
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="timeName"></param>
        private void OnTimeAddTimeTask(int tid, string timeName)
        {
            if (!_inExecution)
            {
                return;
            }

            circuitTempData.timeTask.Add(tid);
        }

        /// <summary>
        /// 删除监听
        /// </summary>
        /// <param name="tid"></param>
        private void OnTimeDeleteTimeTask(int tid)
        {
            if (!_inExecution)
            {
                return;
            }

            for (int i = 0; i < circuitTempData.timeTask.Count; i++)
            {
                if (circuitTempData.timeTask[i] == tid)
                {
                    circuitTempData.timeTask.Remove(circuitTempData.timeTask[i]);
                    return;
                }
            }
        }

        /// <summary>
        /// 增加监听   
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="timeName"></param>
        private void OnTimeAddSwitchTimeTask(int tid, string timeName)
        {
            if (!_inExecution)
            {
                return;
            }

            circuitTempData.switchTask.Add(tid);
        }

        /// <summary>
        /// 删除监听
        /// </summary>
        /// <param name="tid"></param>
        private void OnTimeDeleteSwitchTimeTask(int tid)
        {
            if (!_inExecution)
            {
                return;
            }

            for (int i = 0; i < circuitTempData.switchTask.Count; i++)
            {
                if (circuitTempData.switchTask[i] == tid)
                {
                    circuitTempData.switchTask.Remove(circuitTempData.switchTask[i]);
                    return;
                }
            }
        }
    }
}