using System.Collections.Generic;
using UnityEngine;

namespace ThirdParty.Scripts
{
    public struct CustomGradientKey {
     
      public float T { get; set; }
      public Color Color { get; set; } // comes with r, g, b, alpha
     
      public CustomGradientKey(Color color, float t) {
        Color = color;
        T = t;
      }
     
    }
     
    public class CustomGradient {
     
      private List<CustomGradientKey> _keys = new();

      public int Count => _keys.Count;
     
      public CustomGradientKey this[int index] {
        get => _keys[index];
        set { _keys[index] = value; SortKeys(); }
      }
     
      public void AddKey(Color color, float t)
        => AddKey(new CustomGradientKey(color, t));
     
      public void AddKey(CustomGradientKey key) {
        _keys.Add(key);
        SortKeys();
      }
     
      public void InsertKey(int index, Color color,  float t)
        => InsertKey(index, new CustomGradientKey(color, t));
     
      public void InsertKey(int index, CustomGradientKey key) {
        _keys.Insert(index, key);
        SortKeys();
      }
     
      public void RemoveKey(int index) {
        _keys.RemoveAt(index);
        SortKeys();
      }
     
      public void RemoveInRange(float min, float max) {
        for(int i = _keys.Count - 1; i >= 0; i--)
          if(_keys[i].T >= min && _keys[i].T <= max) _keys.RemoveAt(i);
        SortKeys();
      }
     
      public void Clear() => _keys.Clear();
     
      private void SortKeys() => _keys.Sort( (a, b) => a.T.CompareTo(b.T) );
     
      private (int l, int r) GetNeighborKeys(float t) {
        var l = Count - 1;
     
        for(int i = 0; i <= l; i++) {
          if(_keys[i].T >= t) {
            if(i == 0) return ( -1, i );
            return ( i - 1, i );
          }
        }
     
        return ( l, -1 );
      }
     
      public Color Evaluate(float t) {
        if(Count == 0) return new Color(0f, 0f, 0f, 0f);
     
        var n = GetNeighborKeys(t);
     
        if(n.l < 0) return _keys[n.r].Color;
          else if(n.r < 0) return _keys[n.l].Color;
     
        return Color.Lerp(
          _keys[n.l].Color,
          _keys[n.r].Color,
          Mathf.InverseLerp(_keys[n.l].T, _keys[n.r].T, t)
        );
      }
     
    }

}