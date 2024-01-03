using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ET;
using UnityEngine;
using ET;

namespace Model
{
    /// <summary>
    /// 将Rect分割成x*y个格子
    /// </summary>
    public class GridSearcher
    {
        public int UnitCount => _unitCount;

        private int _unitCount;
        private int _xCount;
        private int _yCount;
        private float _areaHeight;
        private float _areaWidth;
        private Rect _rectArea;
        private Node[] _nodes;
        private MultiMap<IGridSearcher, int> _itemCoords = new MultiMap<IGridSearcher, int>();
        
        
        public GridSearcher(Rect area, float gridWidth)
        {
            _xCount = Mathf.CeilToInt(area.width / gridWidth);
            _yCount = Mathf.CeilToInt(area.height / gridWidth);
            _rectArea = area;
            var count = _xCount * _yCount;
            _nodes = new Node[count];
            for (int i = 0; i < _xCount; i++)
            {
                for (int j = 0; j < _yCount; j++)
                {
                    var item = new Node()
                    {
                        x = i,
                        y = j,
                    };
                    _areaHeight = gridWidth;
                    _areaWidth = gridWidth;
                    item.area = new Rect(_rectArea.xMin + item.x * _areaWidth, _rectArea.yMin + item.y * _areaHeight, _areaWidth, _areaHeight);
                    _nodes[j * _xCount + i] = item;
                }
            }
        }
        
        public void Update(IGridSearcher item)
        {
            var rect = item.GetRect;
            if (!_rectArea.Contains(rect))
                return;

            if (_itemCoords.ContainsKey(item))
            {
                foreach (var index in _itemCoords[item])
                {
                    _nodes[index].Remove(item);
                }
                _itemCoords[item].Clear();
                _unitCount--;
            }

            _unitCount++;

            var p = ConvertPoint(rect.center);
            var x = p.x;
            var y = p.y;
            _itemCoords.Add(item, y * _xCount + x);
            _nodes[y * _xCount + x].Add(item);

            foreach (var i in MoveY(1, int.MaxValue, rect, x, y, AddCondition, BreakCondition))
            {
                _itemCoords.Add(item, i);
                _nodes[i].Add(item);
            }

            foreach (var i in MoveY(-1, int.MaxValue, rect, x, y, AddCondition, BreakCondition))
            {
                _itemCoords.Add(item, i);
                _nodes[i].Add(item);
            }

            foreach (var i in MoveX(1, int.MaxValue, rect, x, y, AddCondition, BreakCondition))
            {
                _itemCoords.Add(item, i);
                _nodes[i].Add(item);
            }

            foreach (var i in MoveX(-1, int.MaxValue, rect, x, y, AddCondition, BreakCondition))
            {
                _itemCoords.Add(item, i);
                _nodes[i].Add(item);
            }
        }

        public void Remove(IGridSearcher item)
        {
            if (_itemCoords.ContainsKey(item))
            {
                foreach (var index in _itemCoords[item])
                {
                    _nodes[index].Remove(item);
                }
                _itemCoords[item].Clear();
                _unitCount--;
            }
        }
        
        /// <summary>
        /// 返回Point所在格子中的第一个节点
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public IGridSearcher FindFirst(Vector2 point)
        {
            var p = ConvertPoint(point);
            var x = p.x;
            var y = p.y;
            var res = _nodes[y * _xCount + x].Contain(point);
            return res;
        }

        /// <summary>
        /// 返回半径内所有节点，不包含自己
        /// </summary>
        /// <param name="target"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public IGridSearcher[] FindAround(IGridSearcher target, float radius)
        {
            var res = new HashSet<IGridSearcher>();
            var rect = target.GetRect;
            if (_unitCount < 10)
            {
                foreach (var key in _itemCoords.Keys)
                {
                    if (key.Equals(target))
                        continue;
                    var dis = Vector2.Distance(key.GetRect.center, rect.center);
                    if (dis <= radius)
                    {
                        res.Add(key);
                    }
                }

                var arr = res.ToArray();
                return arr;
            }
            var p = ConvertPoint(rect.center);
            var x = p.x;
            var y = p.y;

            bool breakCond(int i, Rect r)
            {
                var node = _nodes[i];
                var dis = Vector2.Distance(rect.center, node.area.center);
                return dis > radius;
            }

            foreach (var i in MoveY(1, Int32.MaxValue, rect, x, y, Condition, breakCond))
            {
                var node = _nodes[i];
                foreach (var item in node._items)
                {
                    if (!res.Contains(item))
                    {
                        res.Add(item);
                    }
                }
            }

            foreach (var i in MoveY(-1, Int32.MaxValue, rect, x, y, Condition, breakCond))
            {
                var node = _nodes[i];
                foreach (var item in node._items)
                {
                    if (!res.Contains(item))
                    {
                        res.Add(item);
                    }
                }
            }

            foreach (var i in MoveX(1, Int32.MaxValue, rect, x, y, Condition, breakCond))
            {
                var node = _nodes[i];
                foreach (var item in node._items)
                {
                    if (!res.Contains(item))
                    {
                        res.Add(item);
                    }
                }
            }

            foreach (var i in MoveX(-1, Int32.MaxValue, rect, x, y, Condition, breakCond))
            {
                var node = _nodes[i];
                foreach (var item in node._items)
                {
                    if (!res.Contains(item))
                    {
                        res.Add(item);
                    }
                }
            }

            return res.ToArray();
        }
        
        public bool IsOverlaps(IGridSearcher searcher, int maxDepth, Func<IGridSearcher, bool> condition)
        {
            return IsOverlaps(searcher.GetRect, maxDepth, condition);
        }
        
        
        public bool IsOverlaps(Rect rect, int maxDepth, Func<IGridSearcher, bool> condition)
        {
            if (!_rectArea.Contains(rect))
                return false;
            var p = ConvertPoint(rect.center);
            var x = p.x;
            var y = p.y;
            var node = _nodes[y * _xCount + x];
            if (node.Overlaps(rect, condition))
                return true;
            foreach (var i in MoveY(1, maxDepth, rect, x, y, Condition))
            {
                if (_nodes[i].Overlaps(rect, condition))
                    return true;
            }

            foreach (var i in MoveY(-1, maxDepth, rect, x, y, Condition))
            {
                if (_nodes[i].Overlaps(rect, condition))
                    return true;
            }

            foreach (var i in MoveX(1, maxDepth, rect, x, y, Condition))
            {
                if (_nodes[i].Overlaps(rect, condition))
                    return true;
            }

            foreach (var i in MoveX(-1, maxDepth, rect, x, y, Condition))
            {
                if (_nodes[i].Overlaps(rect, condition))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 查找与Target最近的节点
        /// </summary>
        /// <param name="target">参考对象</param>
        /// <param name="maxDepth">最大查找深度</param>
        /// <param name="condition">当找到最近的节点时触发函数，且当Condition返回True时，该节点为有效最近节点</param>
        /// <returns></returns>
        public IGridSearcher FindNearestTo(IGridSearcher target, int maxDepth, Func<IGridSearcher, bool> condition)
        {
            var rect = target.GetRect;
            if (_unitCount < 10)
            {
                var min = float.MaxValue;
                IGridSearcher res = null;
                foreach (var key in _itemCoords.Keys)
                {
                    if (key.Equals(target))
                        continue;
                    var dis = Vector2.Distance(key.GetRect.center, rect.center);
                    if (dis < min && condition.Invoke(key))
                    {
                        min = dis;
                        res = key;
                    }
                }

                return res;
            }
            var p = ConvertPoint(rect.center);
            var x = p.x;
            var y = p.y;
            var minNum = float.MaxValue;
            IGridSearcher item = null;
            FindDirect(target, ref item, ref minNum, condition, MoveY(1, maxDepth, rect, x, y, NearestCondition));
            FindDirect(target, ref item, ref minNum, condition, MoveY(-1, maxDepth, rect, x, y, NearestCondition));
            FindDirect(target, ref item, ref minNum, condition, MoveX(1, maxDepth, rect, x, y, NearestCondition));
            FindDirect(target, ref item, ref minNum, condition, MoveX(-1, maxDepth, rect, x, y, NearestCondition));
            return item;
        }

        private void FindDirect(IGridSearcher target, ref IGridSearcher result, ref float minNum, Func<IGridSearcher, bool> condition, IEnumerable<int> enumerable)
        {
            foreach (var i in enumerable)
            {
                var res = FindNearest(i, target, ref minNum, condition);
                if (res != null)
                {
                    result = res;
                    return;
                }
            }
        }
        
        private Vector2Int ConvertPoint(Vector2 center)
        {
            var x = Mathf.FloorToInt((center.x - _rectArea.xMin) / _areaWidth);
            var y = Mathf.FloorToInt((center.y - _rectArea.yMin) / _areaHeight);
            return new Vector2Int(x, y);
        }

        private IGridSearcher FindNearest(int index, IGridSearcher target, ref float minNum, Func<IGridSearcher, bool> condition)
        {
            IGridSearcher item = null;
            var rect = target.GetRect;
            foreach (var searcher in _nodes[index]._items)
            {
                if (searcher.Equals(target))
                    continue;
                var dis = Vector2.Distance(searcher.GetRect.center, rect.center);
                if (dis < minNum && condition.Invoke(searcher))
                {
                    minNum = dis;
                    item = searcher;
                }

            }

            return item;
        }
        
        private bool NearestCondition(int arg, Rect rect)
        {
            return _nodes[arg]._items.Count > 0;
        }
        
        private bool Condition(int arg, Rect rect)
        {
            return _nodes[arg]._items.Count > 0;
        }

        private bool AddCondition(int index, Rect rect)
        {
            return _nodes[index].area.Overlaps(rect);
        }
        
        private bool BreakCondition(int index, Rect rect)
        {
            return !_nodes[index].area.Overlaps(rect);
        }

        

        private IEnumerable<int> MoveY(int depth, int maxDepth, Rect rect, int x, int y, Func<int, Rect, bool> condition, Func<int, Rect,bool> breakCondition = null)
        {
            var curY = y + depth;
            if (curY >= _yCount || curY < 0)
                yield break;
            var symbol = depth.Symbol();
            var depthVal = Mathf.Abs(depth);
            if (depthVal > maxDepth)
                yield break;
            
            if (breakCondition?.Invoke(curY * _xCount + x, rect) ?? false)
                yield break;
            
            //从中心到两边
            if (condition.Invoke(curY * _xCount + x, rect))
            {
                yield return curY * _xCount + x;
            }

            for (int i = 1; i <= depthVal; i++)
            {
                if (x + i < _xCount)
                {
                    var index = curY * _xCount + x + i;
                    if (condition.Invoke(index, rect))
                    {
                        yield return index;
                    }
                }
                if (x - i > 0)
                {
                    var index = curY * _xCount + x - i;
                    if (condition.Invoke(index, rect))
                    {
                        yield return index;
                    }
                }
            }

            foreach (var res in MoveY((depthVal + 1) * symbol, maxDepth, rect, x, y, condition, breakCondition))
            {
                yield return res;
            }
        }
        
        private IEnumerable<int> MoveX(int depth, int maxDepth, Rect rect, int x, int y, Func<int, Rect, bool> condition, Func<int, Rect, bool> breakCondition = null)
        {
            var curX = x + depth;
            if (curX >= _xCount || curX < 0)
                yield break;
            var symbol = depth.Symbol();
            var depthVal = Mathf.Abs(depth);
            if (depthVal > maxDepth)
                yield break;
            
            if (breakCondition?.Invoke(y * _xCount + curX, rect) ?? false)
                yield break;
            
            //x轴向的，不遍历四角，防止重复遍历
            if (condition.Invoke(y * _xCount + curX, rect))
            {
                yield return y * _xCount + curX;
            }

            for (int i = 1; i < depthVal; i++)
            {
                if (y + i < _yCount)
                {
                    var index = (y + i) * _xCount + curX;
                    if (condition.Invoke(index, rect))
                    {
                        yield return index;
                    }
                }
                if (y - i > 0)
                {
                    var index = (y - i) * _xCount + curX;
                    if (condition.Invoke(index, rect))
                    {
                        yield return index;
                    }
                }
            }
            

            foreach (var i in MoveX((depthVal + 1) * symbol, maxDepth, rect, x, y, condition))
            {
                yield return i;
            }
        }
        
        private class Node
        {
            public int x;
            public int y;
            public Rect area;
            public List<IGridSearcher> _items = new List<IGridSearcher>();

            public void Remove(IGridSearcher item)
            {
                _items.Remove(item);
            }
            
            public void Add(IGridSearcher item)
            {
                _items.Add(item);
            }

            public bool Overlaps(IGridSearcher target, Func<IGridSearcher, bool> condition)
            {
                var rect = target.GetRect;
                if (!area.Overlaps(rect))
                    return false;
                foreach (var item in _items)
                {
                    if (item.Equals(target))
                        continue;
                    if (item.GetRect.Overlaps(rect) && condition.Invoke(item))
                    {
                        return true;
                    }
                }

                return false;
            }
            
            public bool Overlaps(Rect rect, Func<IGridSearcher, bool> condition)
            {
                if (!area.Overlaps(rect))
                    return false;
                foreach (var item in _items)
                {
                    if (item.GetRect.Overlaps(rect) && (condition?.Invoke(item) ?? true))
                    {
                        return true;
                    }
                }

                return false;
            }

            public IGridSearcher Contain(Vector2 point)
            {
                foreach (var item in _items)
                {
                    if (item.GetRect.Contains(point))
                        return item;
                }

                return null;
            }
        }

        public void OnDrawGizmos()
        {
            for (int i = 0; i < _xCount + 1; i++)
            {
                Gizmos.DrawLine(new Vector3(_rectArea.xMin + i * _areaWidth, 0, _rectArea.yMin), new Vector3(_rectArea.xMin + i * _areaWidth, 0, _rectArea.yMax));
            }

            for (int i = 0; i < _yCount + 1; i++)
            {
                Gizmos.DrawLine(new Vector3(_rectArea.xMin, 0, _rectArea.yMin + i * _areaHeight), new Vector3(_rectArea.xMax, 0, _rectArea.yMin + i * _areaHeight));
            }

            foreach (var node in _nodes)
            {
                if (node._items.Count > 0)
                    GizmosHelper.Draw2DCube(node.area, new Color(0, 1, 0, 0.2f));
            }
        }

    }

    public interface IGridSearcher
    {
        Rect GetRect { get; }
    }
}