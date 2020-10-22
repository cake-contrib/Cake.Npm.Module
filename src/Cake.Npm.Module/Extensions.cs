using System.Linq;
using Cake.Core.Packaging;

namespace Cake.Npm.Module
{
    /// <summary>
    /// Internal Extensions.
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// parses switches from a <see cref="PackageReference"/>.
        /// </summary>
        /// <param name="package">The <see cref="PackageReference"/> to parse.</param>
        /// <param name="key">The key to parse for.</param>
        /// <param name="requireValue"><c>true</c>, if the value (i.e. "true" or "false") should be parsed.
        /// <c>false</c> if only the existence of the switch should be parsed.</param>
        /// <returns><c>true</c> if the switch was set.</returns>
        internal static bool GetSwitch(this PackageReference package, string key, bool requireValue = false)
        {
            bool value = false;
            if (!requireValue)
            {
                return package.Parameters.ContainsKey(key);
            }
            else
            {
                if (package.Parameters.ContainsKey(key) && bool.TryParse(package.Parameters[key].First(), out value))
                {
                    return value;
                }
            }

            return value;
        }

        /// <summary>
        /// Checks the existence of a given key on <see cref="PackageReference"/> and supplies the value in <paramref name="value"/>.
        /// </summary>
        /// <param name="package">The <see cref="PackageReference"/> to check.</param>
        /// <param name="key">The key to search for.</param>
        /// <param name="value">The value of the key, if it was set. <c>string.empty</c> otherwise.</param>
        /// <returns><c>true</c> if the key was set.</returns>
        internal static bool HasValue(this PackageReference package, string key, out string value)
        {
            var hasValue = package.HasValue(key);
            value = package.GetValue(key) ?? string.Empty;
            return hasValue;
        }

        private static bool HasValue(this PackageReference package, string key)
        {
            return package.Parameters.ContainsKey(key) && string.IsNullOrWhiteSpace(package.Parameters[key].First());
        }

        private static string GetValue(this PackageReference package, string key, params char[] trimChars)
        {
            var hasValue = package.HasValue(key);
            if (!hasValue)
            {
                return string.Empty;
            }

            var value = package.Parameters[key].FirstOrDefault();
            return value != null && trimChars.Any()
                ? value.Trim(trimChars)
                : value;
        }
    }
}
