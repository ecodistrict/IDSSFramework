using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Ecodistrict.Messaging
{
    /// <summary>
    /// The collection of inputs from which user can select one in the dashboard.
    /// </summary>
    /// <remarks>
    /// Used by the <see cref="Input"/>-type <see cref="Select"/>.
    /// </remarks>
    [DataContract]
    public class Options : List<Option>
    {
        public new bool Contains(Option option)
        {
            foreach(Option opt in this)
            {
                if (opt.value == option.value)
                    return true;
            }

            return false;
        }

        public int GetIndex(String value)
        {
            if (value != null)
            {
                for (int i = 0; i < this.Count; ++i)
                {
                    if (this[i].value == value)
                        return i;
                }
            }

            return -1;
        }
    }

    /// <summary>
    /// Defines an option that can be selected in the <see cref="Select"/> input type.
    /// </summary>
    [DataContract]
    public class Option
    {
        /// <summary>
        /// The value of the option.
        /// </summary>
        [DataMember]
        public string value { get; protected set; }

        /// <summary>
        /// Description of the value.
        /// </summary>
        [DataMember]
        protected string label;

        /// <summary>
        /// Option constructor.
        /// </summary>
        /// <param name="value">The value of the option.</param>
        /// <param name="label">Description of the value.</param>
        public Option(string value, string label)
        {
            this.value = value;
            this.label = label;
        }

        //public static bool operator ==(Option a, Option b)
        //{
        //    if(a!=null)
        //    return a.value == b.value;
        //}

        //public static bool operator !=(Option a, Option b)
        //{
        //    return !(a.value == b.value);
        //}
    }
}
