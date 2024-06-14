using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DltFramework
{
    public class EntityProcess
    {
        public string entityName;
        public bool display;
    }

    public class ProcessPoolFrameComponent : FrameComponent
    {
        public static ProcessPoolFrameComponent Instance;

        /// <summary>框架初始化</summary>
        public override void FrameInitComponent()
        {
            Instance = this;
        }

        /// <summary>框架场景初始化</summary>
        public override void FrameSceneInitComponent()
        {
        }

        public override void FrameSceneEndComponent()
        {
        }

        public override void FrameEndComponent()
        {
        }
    }
}