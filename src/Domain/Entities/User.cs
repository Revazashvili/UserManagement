using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        /// <summary>
        /// Gets or sets the Personal Number for this user.
        /// </summary>
        public string? Pin { get; set; }
        /// <summary>
        /// Gets or sets a flag indicating user is married or not.
        /// </summary>
        /// <value>True if user is married, otherwise false.</value>
        public bool IsMarried { get; set; }
        /// <summary>
        /// Gets or sets a flag indicating user is employed or not.
        /// </summary>
        /// <value>True if user is employed, otherwise false.</value>
        public bool Employed { get; set; }
        /// <summary>
        /// Gets or sets the compensation for this user.
        /// </summary>
        /// <value>Has value if user is employed.</value>
        public double? Compensation { get; set; }
        /// <summary>
        /// Gets or sets the physical address for this user.
        /// </summary>
        public string? Address { get; set; }
    }
}