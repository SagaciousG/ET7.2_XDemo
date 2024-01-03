using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class XToggleGroup : MonoBehaviour
    {
        private List<XToggle> _toggles = new();

        public void NotifyToggleOn(XToggle toggle)
        {
            foreach (var xToggle in _toggles)
            {
                if (toggle != xToggle)
                    xToggle.isOn = false;
            }
        }
        
        public void Add(XToggle toggle)
        {
            if (!_toggles.Contains(toggle))
                _toggles.Add(toggle);
        }

        public void Remove(XToggle toggle)
        {
            _toggles.Remove(toggle);
        }
    }
}