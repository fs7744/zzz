using Microsoft.Extensions.Configuration;

namespace Zzz.Hosting
{
    public class HostBuilderContext
    {
        /// <summary>
        /// Initializes a new instance of <see cref="HostBuilderContext"/>.
        /// </summary>
        /// <param name="properties">A non-null <see cref="IDictionary{TKey, TValue}"/> for sharing state between components during the host building process.</param>
        public HostBuilderContext(IDictionary<object, object> properties)
        {
            ArgumentNullException.ThrowIfNull(nameof(properties));

            Properties = properties;
        }

        /// <summary>
        /// The <see cref="IHostEnvironment" /> initialized by the <see cref="IHost" />.
        /// </summary>
        public IHostEnvironment HostingEnvironment { get; set; } = null!;

        /// <summary>
        /// The <see cref="IConfiguration" /> containing the merged configuration of the application and the <see cref="IHost" />.
        /// </summary>
        public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// A central location for sharing state between components during the host building process.
        /// </summary>
        public IDictionary<object, object> Properties { get; }
    }
}