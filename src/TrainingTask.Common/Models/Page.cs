using System;
using System.Collections.Generic;
using System.Text;

namespace TrainingTask.Common.Models
{
    /// <summary>
    /// Contains result information after request.
    /// </summary>

    public class Page<T>
    {
        /// <summary>
        /// Gets or sets the items after request for 1 page.
        /// </summary>
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// Gets or sets the total count of items after request.
        /// </summary>
        public int Total { get; set; }

    }
}
