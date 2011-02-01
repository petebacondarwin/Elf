using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elf.Persistence.Entities {
    /// <summary>
    /// Classes that implement this interface will be persisted in the database
    /// </summary>
    public interface IPersistent {
        /// <summary>
        /// The identifier used in the database.
        /// </summary>
        /// <remarks>
        /// Implementing classes should also implement a private set method.
        /// </remarks>
        int Id { get; }
    }
}
