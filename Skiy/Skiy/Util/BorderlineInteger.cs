﻿using System;
using Skiy.Util.Interfaces;

namespace Skiy.Util
{
    public class BorderlineInteger : IBorderline<int>, IComparable<BorderlineInteger>
    {
        public int MinValue() => int.MinValue;

        public int MaxValue() => int.MaxValue;
        
        public int Value { get; set; }
        
        public int CompareTo(BorderlineInteger other)
        {
            return Value.CompareTo(other.Value);
        }

        public BorderlineInteger(int value)
        {
            Value = value;
        }

        public BorderlineInteger()
        {
            
        }
    }
}