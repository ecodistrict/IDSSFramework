using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecodistrict.Messaging
{
    /// <summary>
    /// The <see cref="Ecodistrict.Messaging"/> namespace contains classes for messaging between the
    /// dashboard and modules in the Ecodistr-ICT project.
    /// </summary>
    /// <remarks>
    /// The classes are constructed in a way that if you have been able to create the class, you can be certain 
    /// that the message it produces when you <see cref="Serialize"/> it is valid.<br/>
    /// <br/>
    /// ...<br/>
    /// <br/>
    /// Recieved: <see cref="GetModulesRequest"/>  => Respond: <see cref="GetModulesResponse"/><br/>
    /// Recieved: <see cref="SelectModuleRequest"/>  and moduleId is my id => Respond:  <see cref="SelectModuleResponse"/> 
    /// (contains the <see cref="InputSpecification"/>)<br/>
    /// Recieved: <see cref="StartModuleRequest"/>  and moduleId is my id => Respond:  <see cref="StartModuleResponse"/> with 
    /// <see cref="ModuleStatus"/> Processing and start calculate kpi. When finished Respond:  <see cref="StartModuleResponse"/> 
    /// with <see cref="ModuleStatus"/> Success (if it is).<br/>
    /// Send result with <see cref="ModuleResult"/>.<br/>
    /// <br/>
    /// This is the class diagram for the Ecodistrict.Messaging namespace.
    /// <img src="../ClassDiagram.png" alt="Class diagram"/>
    /// <img src="../../../../ClassDiagram.png" />
    /// </remarks>
    [System.Runtime.CompilerServices.CompilerGenerated]
    class NamespaceDoc
    {

    }
    
}
