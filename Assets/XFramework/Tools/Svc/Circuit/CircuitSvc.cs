using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    public class CircuitSvc : SvcBase
    {
        public static CircuitSvc Instance;

        [BoxGroup] [TableList(AlwaysExpanded = true, DrawScrollView = false)] [LabelText("场景缓存流程数据")]
        public CircuitTempData circuitTempData = new CircuitTempData();

        [LabelText("上一个流程")] private Type _lastCircuitBaseData;
        [LabelText("场景流程")] public List<CircuitBaseData> sceneCircuitBaseData;
        private Dictionary<Type, CircuitBaseData> _allCircuitBaseDataDic;
        [LabelText("流程执行中")] private bool _inExecution;

        public override void StartSvc()
        {
            Instance = GetComponent<CircuitSvc>();
        }

        public override void InitSvc()
        {
            ViewSvc.Instance.onShowView += OnViewShow;
            ViewSvc.Instance.onHideView += OnHideShow;
            TimeSvc.Instance.onAddTimeTask += OnTimeAddTimeTask;
            TimeSvc.Instance.onDeleteTimeTask += OnTimeDeleteTimeTask;
            TimeSvc.Instance.onAddSwitchTask += OnTimeAddSwitchTimeTask;
            TimeSvc.Instance.onDeleteSwitchTask += OnTimeDeleteSwitchTimeTask;
            EntitySvc.Instance.onShowEntity += OnShowEntity;
            EntitySvc.Instance.onHideEntity += OnHideEntity;
            _allCircuitBaseDataDic = new Dictionary<Type, CircuitBaseData>();
            sceneCircuitBaseData = DataSvc.GetInheritAllSubclass<CircuitBaseData>();
            foreach (CircuitBaseData circuitBaseData in sceneCircuitBaseData)
            {
                _allCircuitBaseDataDic.Add(circuitBaseData.GetType(), circuitBaseData);
            }
        }

        public override void EndSvc()
        {
            ViewSvc.Instance.onShowView -= OnViewShow;
            ViewSvc.Instance.onHideView -= OnHideShow;
            TimeSvc.Instance.onAddTimeTask -= OnTimeAddTimeTask;
            TimeSvc.Instance.onDeleteTimeTask -= OnTimeDeleteTimeTask;
            TimeSvc.Instance.onAddSwitchTask -= OnTimeAddSwitchTimeTask;
            TimeSvc.Instance.onDeleteSwitchTask -= OnTimeDeleteSwitchTimeTask;
            EntitySvc.Instance.onShowEntity -= OnShowEntity;
            EntitySvc.Instance.onHideEntity -= OnHideEntity;
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
            Type[] viewType = DataSvc.DataValueClone(circuitTempData.activityViewType).ToArray();
            ViewSvc.Instance.HideView(viewType);
            //结束时间监听
            List<int> timeTask = DataSvc.DataValueClone(circuitTempData.timeTask);
            TimeSvc.Instance.DeleteTimeTask(timeTask);
            circuitTempData.timeTask.Clear();
            List<int> switchTimeTask = DataSvc.DataValueClone(circuitTempData.switchTask);
            for (int i = 0; i < switchTimeTask.Count; i++)
            {
                TimeSvc.Instance.DeleteSwitchTask(switchTimeTask[i]);
            }

            circuitTempData.switchTask.Clear();
            //隐藏实体
            EntitySvc.Instance.DisplayEntityByEntityName(false, DataSvc.DataValueClone(circuitTempData.entity).ToArray());
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