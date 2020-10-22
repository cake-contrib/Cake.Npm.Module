using System.Linq;
using Cake.Core.Packaging;

namespace Cake.Npm.Module
{
    internal static class Extensions
    {
        internal static bool GetSwitch(this PackageReference package, string key, bool requireValue = false) {
            bool value = false;
            if (!requireValue) {
                return package.Parameters.ContainsKey(key);
            } else {
                if (package.Parameters.ContainsKey(key) && bool.TryParse(package.Parameters[key].First(), out value)) {
                    return value;
                }
            }
            return value;
        }

        private static bool HasValue(this PackageReference package, string key) {
            return package.Parameters.ContainsKey(key) && string.IsNullOrWhiteSpace(package.Parameters[key].First());
        }

        internal static bool HasValue(this PackageReference package, string key, out string value) {
            var hasValue = package.HasValue(key);
            value = package.GetValue(key) ?? string.Empty;
            return hasValue;
        }

        private static string GetValue(this PackageReference package, string key, params char[] trimChars) {
            var hasValue = package.HasValue(key);
            if (!hasValue) return string.Empty;
            var value = package.Parameters[key].FirstOrDefault();
            return value != null && trimChars.Any()
                ? value.Trim(trimChars)
                : value;

        }
    }
}
