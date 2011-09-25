using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandController.Core
{
    /// <summary>
    /// Controller interface for front-ends, such as a command line interface, graphical UI, etc.
    /// </summary>
    public interface IFrontEndController
    {
        /// <summary>
        /// Initialises this instance.
        /// </summary>
        /// <param name="data">An <see cref="Object"/> containing initialisation data.</param>
        int Start(object data);
    }
}
