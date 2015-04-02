using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecodistrict.Messaging
{
    /// <summary>
    /// The <see cref="Ecodistrict.Messaging"/> namespace contains classes for messaging between the
    /// dashboard and modules in the <see href="http://ecodistr-ict.eu/">Ecodistr-ICT project</see>.
    /// </summary>
    /// <remarks>
    /// The <see cref="Ecodistrict.Messaging"/> namespace contains classes for messaging between the
    /// dashboard and modules in the Ecodistr-ICT project.
    /// 
    /// The classes are constructed in a way that if you have been able to create the class, you can be certain 
    /// that the message it produces when you <see cref="Serialize"/> it can be interpeted by the dashboard. 
    /// However, the user of the library must make sure that the dashboard is given valid information 
    /// (<see cref="Response"/>) and that the module request the information it needs (for a given kpi), see 
    /// <see cref="InputSpecification"/> in <see cref="SelectModuleResponse"/>.<br/>
    /// <br/>
    /// When designing you application it's good to know that for every <see cref="Request"/> you get from
    /// the dashboard there is atleast one <see cref="Response"/> you should send back. If you get a moduleId, variantId
    /// and/or a kpiId with a <see cref="Request"/> make sure this information is in you <see cref="Response"/> back
    /// to the dashboard<br/>
    /// <br/>
    /// A rough layout of the messaging structure:<br/>
    /// <br/>
    /// Recieved: <see cref="GetModulesRequest"/>  => Respond: <see cref="GetModulesResponse"/><br/>
    /// <br/>
    /// Recieved: <see cref="SelectModuleRequest"/>  and moduleId is my id => Respond:  <see cref="SelectModuleResponse"/> 
    /// (contains the module specific <see cref="InputSpecification"/> for the selected kpi)<br/>
    /// <br/>
    /// Recieved: <see cref="StartModuleRequest"/>  and moduleId is my id => Respond:  <see cref="StartModuleResponse"/> with 
    /// <see cref="ModuleStatus"/> 'Processing' and start calculate the selected kpi. 
    /// When finished Respond:  <see cref="StartModuleResponse"/> with <see cref="ModuleStatus"/> 
    /// 'Success' (if it is). Then send result with <see cref="ModuleResult"/>.<br/>
    /// <br/>
    /// <br/>
    /// This is the class diagram for the <see cref="Ecodistrict.Messaging"/> namespace:
    /// <img src="../ClassDiagram.png" alt="Class diagram"/>
    /// <img src="../../../../ClassDiagram.png" />
    /// </remarks>
    [System.Runtime.CompilerServices.CompilerGenerated]
    class NamespaceDoc
    {

    }
    
}
