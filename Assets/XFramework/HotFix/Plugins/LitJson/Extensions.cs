﻿using UnityEngine;
using System.Collections;
using System;

namespace LitJson.Extensions {
#pragma warning disable CS0436

    /// <summary>
    /// 拓展方法
    /// </summary>
	public static class Extensions {

		public static void WriteProperty(this JsonWriter w,string name,long value){
			w.WritePropertyName(name);
			w.Write(value);
		}
		
		public static void WriteProperty(this JsonWriter w,string name,string value){
			w.WritePropertyName(name);
			w.Write(value);
		}
		
		public static void WriteProperty(this JsonWriter w,string name,bool value){
			w.WritePropertyName(name);
			w.Write(value);
		}
		
		public static void WriteProperty(this JsonWriter w,string name,double value){
			w.WritePropertyName(name);
			w.Write(value);
		}

	}

    /// <summary>
    /// 跳过序列化的标签
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class JsonIgnore : Attribute
    {

    }
#pragma warning able CS0436

}