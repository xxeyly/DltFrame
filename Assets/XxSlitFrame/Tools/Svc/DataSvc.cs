using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace XxSlitFrame.Tools.Svc
{
    public static class DataSvc
    {
        /// <summary>
        /// 随机排序
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> RandomSort<T>(List<T> list)
        {
            var random = new Random();
            var newList = new List<T>();
            foreach (var item in list)
            {
                newList.Insert(random.Next(newList.Count), item);
            }

            return newList;
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                return input;
            string str = input.First().ToString().ToUpper() + input.Substring(1);
            return str;
        }

        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FirstCharToLower(string input)
        {
            if (String.IsNullOrEmpty(input))
                return input;
            string str = input.First().ToString().ToLower() + input.Substring(1);
            return str;
        }

        /// <summary>
        /// 获得两点之间的角度360
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float Angle(Vector3 origin, Vector3 target)
        {
            //两点的x、y值
            float x = origin.x - target.x;
            float y = origin.y - target.y;

            //斜边长度
            float hypotenuse = Mathf.Sqrt(Mathf.Pow(x, 2f) + Mathf.Pow(y, 2f));

            //求出弧度
            float cos = x / hypotenuse;
            float radian = Mathf.Acos(cos);

            //用弧度算出角度    
            float angle = 180 / (Mathf.PI / radian);
            if (y > 0)
            {
                angle = 180 + (180 - angle);
            }

            return angle;
        }
    }
}