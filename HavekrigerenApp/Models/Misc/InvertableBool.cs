using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavekrigerenApp.Models.Misc
{
    public class InvertableBool
    {
        private bool value = false;

        public bool Value { get { return value; } }
        public bool Invert { get { return !value; } }

        public InvertableBool(bool b)
        {
            value = b;
        }

        public static implicit operator InvertableBool(bool b)
        {
            return new InvertableBool(b);
        }

        public static implicit operator bool(InvertableBool b)
        {
            return b.value;
        }
    }
}
