using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecodistrict.Messaging.MessageTypes
{
    /// <summary>
    /// Containing different enum message indicators used by the <see cref="Ecodistrict.Messaging"/> namespace.
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGenerated]
    class NamespaceDoc
    {

    }

    /// <summary>
    /// Used to flag for different types of method commands described in a <see cref="IMessage"/> message.
    /// </summary>
    public enum MessageMethod
    {
        /// <summary>
        /// Indicator whether the <see cref="IMessage"/> belongs to the group GetModules.
        /// </summary>
        GetModules,

        /// <summary>
        /// Indicator whether the <see cref="IMessage"/> belongs to the group SelectModule.
        /// </summary>
        SelectModule,

        /// <summary>
        /// Indicator whether the <see cref="IMessage"/> belongs to the group StartModule.
        /// </summary>
        StartModule,

        /// <summary>
        /// Indicator whether the <see cref="IMessage"/> belongs to the group ModuleResult.
        /// </summary>
        ModuleResult,

        /// <summary>
        /// No valid method exist.
        /// </summary>
        NoMethod
    }

    /// <summary>
    /// Used to flag for which sub-message-type the <see cref="IMessage"/> belongs to (<see cref="Ecodistrict.Messaging.Request"/>, 
    /// <see cref="Ecodistrict.Messaging.Response"/> or <see cref="Ecodistrict.Messaging.Result"/>). 
    /// <seealso cref="Ecodistrict.Messaging.Request"/><seealso cref="Ecodistrict.Messaging.Response"/>
    /// <seealso cref="Ecodistrict.Messaging.Result"/>
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Indicator of which subclass of <see cref="IMessage"/> message belongs to, in this case <see cref="Ecodistrict.Messaging.Request"/>.
        /// </summary>
        Request,
        
        /// <summary>
        /// Indicator of which subclass of <see cref="IMessage"/> message belongs to, in this case <see cref="Ecodistrict.Messaging.Response"/>.
        /// </summary>
        Response,

        /// <summary>
        /// Indicator of which subclass of <see cref="IMessage"/> message belongs to, in this case <see cref="Ecodistrict.Messaging.Result"/>.
        /// </summary>
        Result,

        /// <summary>
        /// Indicator signaling that the <see cref="IMessage"/> type does not exist, and therefore
        /// isn't valid.
        /// </summary>
        NoType
    }

    /// <summary>
    /// Indicator used by <see cref="StartModuleResponse"/> in order signal the 
    /// state of the module.
    /// </summary>
    public enum ModuleStatus
    {
        /// <summary>
        /// The module is calculating. 
        /// </summary>
        Processing,

        /// <summary>
        /// The module has finished succesfully, the dashboard may expect a result soon.
        /// </summary>
        Success
    }


}
