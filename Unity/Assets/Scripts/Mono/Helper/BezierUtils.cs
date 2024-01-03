using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public static class BezierUtils
{
 
    /// <summary>
    /// 线性贝塞尔曲线，根据T值，计算贝塞尔曲线上面相对应的点
    /// </summary>
    /// <param name="t"></param>T值
    /// <param name="p0"></param>起始点
    /// <param name="p1"></param>控制点
    /// <returns></returns>根据T值计算出来的贝赛尔曲线点
    private static Vector3 CalculateLineBezierPoint(float t, Vector3 p0, Vector3 p1)
    {
        float u = 1 - t;
         
        Vector3 p = u * p0;
        p +=  t * p1;
    
 
        return p;
    }
 
    /// <summary>
    /// 二次贝塞尔曲线，根据T值，计算贝塞尔曲线上面相对应的点
    /// </summary>
    /// <param name="t"></param>T值
    /// <param name="p0"></param>起始点
    /// <param name="p1"></param>控制点
    /// <param name="p2"></param>目标点
    /// <returns></returns>根据T值计算出来的贝赛尔曲线点
    private static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
 
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
 
        return p;
    }
 
    /// <summary>
    /// 三次贝塞尔曲线，根据T值，计算贝塞尔曲线上面相对应的点
    /// </summary>
    /// <param name="t">插量值</param>
    /// <param name="startPoint">起点</param>
    /// <param name="controlPoint1">控制点1</param>
    /// <param name="controlPoint2">控制点2</param>
    /// <param name="endPoint">尾点</param>
    /// <returns></returns>
    public static Vector3 CalculateThreePowerBezierPoint(float t, Vector3 startPoint, Vector3 controlPoint1, Vector3 controlPoint2, Vector3 endPoint)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float ttt = tt * t;
        float uuu = uu * u;
 
        Vector3 p = uuu * startPoint;
        p += 3 * t * uu * controlPoint1;
        p += 3 * tt * u * controlPoint2;
        p += ttt * endPoint;
 
        return p;
    }
    
    /// <summary>
    /// 在贝塞尔曲线上取n个点，计算点之间的直线长度，进行加和，从而取得一个曲线的近似长度。取点越多这个长度越趋向于精确。
    /// 形参中的p0, p1, p2, p3 分别对应公式中的 p0~p3。pointCount代表取点个数，默认30。
    /// </summary>
    /// <param name="startPoint"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <param name="pointCount"></param>
    /// <returns></returns>
    public static float BezierLength(Vector3 startPoint, Vector3 controlPoint1, Vector3 controlPoint2, Vector3 endPoint, int pointCount = 30)
    {
        if (pointCount < 2)
        {
            return 0;
        }

        //取点 默认 30个
        float length = 0.0f;
        Vector3 lastPoint = CalculateThreePowerBezierPoint(0, startPoint, controlPoint1, controlPoint2, endPoint);
        for (int i = 1; i <= pointCount; i++)
        {
            Vector3 point = CalculateThreePowerBezierPoint((float)i/(float)pointCount, startPoint, controlPoint1, controlPoint2, endPoint);
            length += Vector3.Distance(point, lastPoint);
            lastPoint = point;
        }
        return length;
    }
 
 
    /// <summary>
    /// 获取存储贝塞尔曲线点的数组
    /// </summary>
    /// <param name="startPoint"></param>起始点
    /// <param name="controlPoint"></param>控制点
    /// <param name="endPoint"></param>目标点
    /// <param name="segmentNum"></param>采样点的数量
    /// <returns></returns>存储贝塞尔曲线点的数组
    public static Vector3[] GetLineBeizerList(Vector3 startPoint,  Vector3 endPoint, int segmentNum)
    {
        Vector3[] path = new Vector3[segmentNum];
        for (int i = 1; i <= segmentNum; i++)
        {
            float t = i / (float)segmentNum;
            Vector3 pixel = CalculateLineBezierPoint(t, startPoint, endPoint);
            path[i - 1] = pixel;
            Debug.Log(path[i - 1]);
        }
        return path;
 
    }
 
    /// <summary>
    /// 获取存储的二次贝塞尔曲线点的数组
    /// </summary>
    /// <param name="startPoint"></param>起始点
    /// <param name="controlPoint"></param>控制点
    /// <param name="endPoint"></param>目标点
    /// <param name="segmentNum"></param>采样点的数量
    /// <returns></returns>存储贝塞尔曲线点的数组
    public static Vector3[] GetCubicBeizerList(Vector3 startPoint, Vector3 controlPoint, Vector3 endPoint, int segmentNum)
    {
        Vector3[] path = new Vector3[segmentNum];
        for (int i = 1; i <= segmentNum; i++)
        {
            float t = i / (float)segmentNum;
            Vector3 pixel = CalculateCubicBezierPoint(t, startPoint,
                controlPoint, endPoint);
            path[i - 1] = pixel;
            Debug.Log(path[i - 1]);
        }
        return path;
 
    }
 
    /// <summary>
    /// 获取存储的三次贝塞尔曲线点的数组
    /// </summary>
    /// <param name="startPoint"></param>起始点
    /// <param name="controlPoint1"></param>控制点1
    /// <param name="controlPoint2"></param>控制点2
    /// <param name="endPoint"></param>目标点
    /// <param name="segmentNum"></param>采样点的数量
    /// <returns></returns>存储贝塞尔曲线点的数组
    public static Vector3[] GetThreePowerBeizerList(Vector3 startPoint, Vector3 controlPoint1, Vector3 controlPoint2 , Vector3 endPoint, int segmentNum)
    {
        Vector3[] path = new Vector3[segmentNum];
        for (int i = 1; i <= segmentNum; i++)
        {
            float t = i / (float)segmentNum;
            Vector3 pixel = CalculateThreePowerBezierPoint(t, startPoint,
                controlPoint1, controlPoint2, endPoint);
            path[i - 1] = pixel;
            Debug.Log(path[i - 1]);
        }
        return path;
 
    }
}
