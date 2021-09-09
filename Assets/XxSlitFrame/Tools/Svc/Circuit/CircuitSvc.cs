using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using XxSlitFrame.Tools.Svc.BaseSvc;

namespace XxSlitFrame.Tools.Svc
{
    public class CircuitSvc : SvcBase
    {
        public static CircuitSvc Instance;
        [TableList(AlwaysExpanded = true, DrawScrollView = false)]
        [LabelText("场景缓存流程数据")] public CircuitData circuitData;

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
        }

        private void OnHideEntity(string entityName)
        {
            for (int i = 0; i < circuitData.entity.Count; i++)
            {
                if (circuitData.entity[i] == entityName)
                {
                    circuitData.entity.RemoveAt(i);
                    return;
                }
            }
        }

        private void OnShowEntity(string entityName)
        {
            circuitData.entity.Add(entityName);
        }

        private void OnHideShow(Type type)
        {
            for (int i = 0; i < circuitData.activityViewType.Count; i++)
            {
                if (circuitData.activityViewType[i] == type)
                {
                    circuitData.activityViewType.RemoveAt(i);
                    return;
                }
            }
        }

        private void OnViewShow(Type type)
        {
            circuitData.activityViewType.Add(type);
        }

        private void OnTimeAddTimeTask(int tid, string timeName)
        {
            circuitData.timeTask.Add(tid);
        }

        private void OnTimeDeleteTimeTask(int tid)
        {
            for (int i = 0; i < circuitData.timeTask.Count; i++)
            {
                if (circuitData.timeTask[i] == tid)
                {
                    circuitData.timeTask.RemoveAt(i);
                    return;
                }
            }
        }

        private void OnTimeAddSwitchTimeTask(int tid, string timeName)
        {
            circuitData.switchTask.Add(tid);
        }

        private void OnTimeDeleteSwitchTimeTask(int tid)
        {
            for (int i = 0; i < circuitData.switchTask.Count; i++)
            {
                if (circuitData.switchTask[i] == tid)
                {
                    circuitData.switchTask.RemoveAt(i);
                    return;
                }
            }
        }

        public override void EndSvc()
        {
        }
    }
}