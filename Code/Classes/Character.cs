using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Inhuman
{
    public class Character
    {
        public string Value = ""; // Char or "first" "last" //
        public float Similarity = 1;

        public Dictionary<string, int> Before = new Dictionary<string, int>();
        public Dictionary<string, int> After = new Dictionary<string, int>();

        
        //================================================================================================================================================================//
        public Character() { }
        public Character(string value)
        {
            Value = value;
        }

        //================================================================================================================================================================//
        public static bool operator == (Character a, Character b)
        {
            if (System.Object.ReferenceEquals(a, b))
                return true;

            if (((object)a == null) || ((object)b == null))
                return false;

            return a.Value == b.Value;
        }

        //================================================================================================================================================================//
        public static bool operator != (Character a, Character b)
        {
            return !(a == b);
        }

        //================================================================================================================================================================//
        public override bool Equals(object obj)
        {
            Character a = obj as Character;
            if ((object)a == null)
                return false;

            return Value == a.Value;
        }

        //================================================================================================================================================================//
        public bool Equals(Character a)
        {
            return Value == a.Value;
        }

        //================================================================================================================================================================//
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


    }
}
