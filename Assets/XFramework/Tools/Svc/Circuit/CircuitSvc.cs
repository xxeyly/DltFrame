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

        [TableList(AlwaysExpanded = true, DrawScrollView = false)] [LabelText("场景缓存流程数据")]
        public CircuitTempData circuitTempData;

        [LabelText("场景流程")] public List<CircuitBaseData> sceneCircuitBaseData;
        private Dictionary<Type, CircuitBaseData> _allCircuitBaseDataDic;

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
            sceneCircuitBaseData = DataSvc.GetAllObjectsInScene<CircuitBaseData>();
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

        public void StartCircuit(Type circuitType)
        {
            ClearTempCircuit();
            _allCircuitBaseDataDic[circuitType].StartCircuit();
        }

        private void ClearTempCircuit()
        {
            Type[] viewType = DataSvc.DataValueClone(circuitTempData.activityViewType).ToArray();
            ViewSvc.Instance.HideView(viewType);

            List<int> timeTask = DataSvc.DataValueClone(circuitTempData.timeTask);
            TimeSvc.Instance.DeleteTimeTask(timeTask);
            circuitTempData.timeTask.Clear();

            List<int> switchTimeTask = DataSvc.DataValueClone(circuitTempData.switchTask);
            for (int i = 0; i < switchTimeTask.Count; i++)
            {
                TimeSvc.Instance.DeleteSwitchTask(switchTimeTask[i]);
            }

            circuitTempData.switchTask.Clear();

            EntitySvc.Instance.DisplayEntityByEntityName(false, DataSvc.DataValueClone(circuitTempData.entity).ToArray());
            circuitTempData.entity.Clear();
        }

        /// <summary>
        /// 注册流程
        /// </summary>
        /// <param name="circuitBaseData"></param>
        public void RegisterCircuit(CircuitBaseData circuitBaseData)
        {
            if (_allCircuitBaseDataDic.ContainsKey(circuitBaseData.GetType()))
            {
                Debug.LogError(circuitBaseData.GetType() + "已经注册过");
            }
            else
            {
                _allCircuitBaseDataDic.Add(circuitBaseData.GetType(), circuitBaseData);
            }
        }

        private void OnHideEntity(string entityName)
        {
            for (int i = 0; i < circuitTempData.entity.Count; i++)
            {
                if (circuitTempData.entity[i] == entityName)
                {
                    circuitTempData.entity.RemoveAt(i);
                    return;
                }
            }
        }

        private void OnShowEntity(string entityName)
        {
            circuitTempData.entity.Add(entityName);
        }

        private void OnHideShow(Type type)
        {
            for (int i = 0; i < circuitTempData.activityViewType.Count; i++)
            {
                if (circuitTempData.activityViewType[i] == type)
                {
                    circuitTempData.activityViewType.Remove(circuitTempData.activityViewType[i]);
                    return;
                }
            }
        }

        private void OnViewShow(Type type)
        {
            circuitTempData.activityViewType.Add(type);
        }

        private void OnTimeAddTimeTask(int tid, string timeName)
        {
            circuitTempData.timeTask.Add(tid);
        }

        private void OnTimeDeleteTimeTask(int tid)
        {
            for (int i = 0; i < circuitTempData.timeTask.Count; i++)
            {
                if (circuitTempData.timeTask[i] == tid)
                {
                    circuitTempData.timeTask.Remove(circuitTempData.timeTask[i]);
                    return;
                }
            }
        }

        private void OnTimeAddSwitchTimeTask(int tid, string timeName)
        {
            circuitTempData.switchTask.Add(tid);
        }

        private void OnTimeDeleteSwitchTimeTask(int tid)
        {
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