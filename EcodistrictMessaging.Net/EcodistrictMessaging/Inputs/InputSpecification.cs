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
    /// The collection of <see cref="Input"/>s the module need in order to calculate a given kpi.
    /// 
    /// Deprecated, the dashboard doesn't need this anymore.
    /// </summary>
    /// <example>
    /// A construction of a simple input specification containing 2 components.
    /// <code>
    /// //Create a input specification demanding 2 values from the user of the dashboard.
    /// InputSpecification iSpec = new InputSpecification();
    ///
    /// //A user specified age
    /// Number numberAge = new Number(
    ///    label: "Age",
    ///    min: 0,
    ///    unit: "years");
    ///
    /// Options opt = new Options();
    /// opt.Add(new Option(value: "alp-cheese", label: "Alpk\u00e4se")); //Note the web-friendly string
    /// opt.Add(new Option(value: "edam-cheese", label: "Edammer"));
    /// opt.Add(new Option(value: "brie-cheese", label: "Brie"));
    ///
    /// //And one of the above options of cheese-types. 
    /// //(The preselected value, "brie-cheese", is optional)
    /// Select selectCheseType = new Select(
    ///    label: "Cheese type",
    ///    options: opt,
    ///    value: "brie-cheese");
    ///
    /// //Add these components to the input specification.
    /// //(Note the choosed keys, its the keys that will be attached to the
    /// //data when the dashboard returns with the user specified data in
    /// //a StartModuleRequest.)
    /// iSpec.Add(
    ///    key: "age",
    ///    value: numberAge);
    ///
    /// iSpec.Add(
    ///    key: "cheese-type",
    ///    value: selectCheseType);
    /// </code>
    /// </example>
    /// <seealso cref="Atomic"/> <seealso cref="NonAtomic"/>
    [DataContract]
    public class InputSpecification : Dictionary<string, Input> //TODO Is not used anymore, remove when the messaging format has been finalized.
    {

    }
}
