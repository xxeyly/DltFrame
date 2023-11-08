using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class FindMaxSubSum : SerializedMonoBehaviour
{
    // Start is called before the first frame update

    public List<int> number = new List<int>();
    [SerializeField] public List<List<int>> temp = new List<List<int>>();

    [Button]
    //求出最大的和
    public void StartContinuousSubarray()
    {
        temp = GetContinuousSubarray(new List<int>() { 1, -1, 2, 3 });
        List<int> SubarraySum = new List<int>();

        for (int i = 0; i < temp.Count; i++)
        {
            int tempSum = 0;
            for (int j = 0; j < temp[i].Count; j++)
            {
                tempSum += temp[i][j];
            }

            SubarraySum.Add(tempSum);
        }

        Debug.Log(SubarraySum.Max());
    }

    //求出所有连续子数组
    private List<List<int>> GetContinuousSubarray(List<int> subarray)
    {
        List<List<int>> temp = new List<List<int>>();

        //从头开始遍历
        for (int i = 0; i < subarray.Count; i++)
        {
            //每次遍历都从第下一位开始
            //1,-1,2,3
            //-1,2,3
            //2,3
            //3
            for (int j = i; j < subarray.Count; j++)
            {
                //每次遍历的个数 从1个增加到最大数组个数
                //分解 1,-1,2,3
                // 1,-1
                // 1,-1.2
                // 1,-1,2,3
                List<int> t1 = new List<int>();
                for (int k = i; k <= j; k++)
                {
                    t1.Add(subarray[k]);
                }

                temp.Add(t1);
            }
        }

        return temp;
    }
}