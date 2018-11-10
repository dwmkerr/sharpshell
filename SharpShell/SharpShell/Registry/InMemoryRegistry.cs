using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace SharpShell.Registry
{
    /// <summary>
    /// The In-Memory registry implements <see cref="IRegistry"/> with a simple in-memory structure.
    /// It is designed to support testing scenarios, for example, asserting the ServerRegistrationManager
    /// can correctly register differnt types of servers.
    /// </summary>
    /// <seealso cref="SharpShell.Registry.IRegistry" />
    public class InMemoryRegistry : IRegistry
    {
        private readonly Dictionary<Tuple<RegistryView, RegistryHive>, InMemoryRegistryKey> _rootKeys = new Dictionary<Tuple<RegistryView, RegistryHive>, InMemoryRegistryKey>();

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryRegistry"/> class.
        /// </summary>
        public InMemoryRegistry()
        {
            var hivesAndNames = new[]
            {
                Tuple.Create(RegistryHive.CurrentUser, "HKEY_CURRENT_USER"),
                Tuple.Create(RegistryHive.LocalMachine, "HKEY_LOCAL_MACHINE"),
                Tuple.Create(RegistryHive.ClassesRoot, "HKEY_CLASSES_ROOT"),
                Tuple.Create(RegistryHive.Users, "HKEY_USERS"),
                Tuple.Create(RegistryHive.PerformanceData, "HKEY_PERFORMANCE_DATA"),
                Tuple.Create(RegistryHive.CurrentConfig, "HKEY_CURRENT_CONFIG")
            };
            foreach (var hn in hivesAndNames)
            {
                foreach (var view in Enum.GetValues(typeof(RegistryView)).OfType<RegistryView>())
                {
                    _rootKeys.Add(Tuple.Create(view, hn.Item1), new InMemoryRegistryKey(view, hn.Item2));
                }
            }
        }

        /// <inheritdoc />
        public IRegistryKey OpenBaseKey(RegistryHive hKey, RegistryView view)
        {
            //  Find and return the root key for the given view.
            var rootKey = _rootKeys.Where(kv => kv.Key.Item1 == view && kv.Key.Item2 == hKey).Select(kvp => kvp.Value).FirstOrDefault();
            if(rootKey == null) throw new InvalidOperationException($"Cannot find {view} root key for hive '{hKey}'");
            return rootKey;
        }

        /// <summary>
        /// Adds the given structure to the registry view. Generally used for testing only.
        /// </summary>
        /// <param name="registryView">The registry view.</param>
        /// <param name="structure">The structure, which matches the MSDN documentation for shell extensions.</param>
        /// <exception cref="InvalidOperationException">Thrown if the structure is malformed.</exception>
        public void AddStructure(RegistryView registryView, string structure)
        {
            //  Helper function to get the number of spaces which start a line.
            var initialSpaceRex = new Regex(@"^([ ]+)[^ ]");
            int Spaces(string line)
            {
                var m = initialSpaceRex.Match(line);
                return m.Success ? m.Groups[1].Length : 0;
            }

            //  Loop through the lines, building a stack of keys which we set.
            var keyStack = new Stack<IRegistryKey>();
            var lineNum = 0;
            foreach (var line in structure.Split(new [] { Environment.NewLine }, StringSplitOptions.None))
            {
                lineNum++;

                //  Skip empty lines.
                if (string.IsNullOrEmpty(line)) continue;
                
                //  Get the depth of the line, which is indented by sets of three spaces.
                var spaces = Spaces(line);
                if(spaces%3 != 0) throw new InvalidOperationException($@"Line {lineNum+1}: Line starts with {spaces} spaces. Lines should start with spaces which are a multiple of three.");
                var depth = spaces / 3;

                //  Pop the stack if we need to.
                while (depth < keyStack.Count) keyStack.Pop();

                //  If we have zero spaces, we're loading a hive.
                if (depth == 0)
                {
                    //  Load the hive.
                    var hive = _rootKeys.Where(kv => kv.Key.Item1 == registryView && kv.Value.Name == line).Select(kvp => kvp.Value).FirstOrDefault();
                    if(hive == null) throw new InvalidOperationException($@"Line ${lineNum + 1}: {line} is not a known registry hive key.");
                    keyStack.Push(hive);
                    continue;
                }

                //  If we are at the current depth, we're either moving to a child key or setting a value.
                if (depth == keyStack.Count)
                {
                    //  Are we setting a value?
                    var rexVal = new Regex(@"^(.*) = (.*)$");
                    var match = rexVal.Match(line.TrimStart());
                    if (match.Success)
                    {
                        var name = match.Groups[1].Value;
                        var value = match.Groups[2].Value;

                        //  Don't forget - '(Default)' is a magic string for empty (i.e. the default value)...
                        keyStack.Peek().SetValue(name == "(Default)" ? string.Empty : name, value);
                    }
                    else
                    {
                        //  We're opening or creating a subkey.
                        var subkeyName = line.TrimStart();
                        keyStack.Push(keyStack.Peek().CreateSubKey(subkeyName));
                    }
                    continue;
                }
                
                //  If we get here, we've got a malformed file.
                throw new InvalidOperationException($@"Line {lineNum + 1}: This line it at an invalid depth.");
            }
        }

        /// <summary>
        /// Prints a registry key with the given depth.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="depth">The depth.</param>
        /// <returns>The key, printed at the given depth.</returns>
        private static string PrintKey(IRegistryKey key, int depth)
        {
            var indent = new string(' ', depth*3);

            //  Get the value strings.
            var values = key.GetValueNames()
                .Select(v => $"{indent}{(string.IsNullOrEmpty(v) ? "(Default)" : v)} = {key.GetValue(v)}")
                .OrderBy(s => s);

            //  Get the subkey strings.
            var subKeys = key.GetSubKeyNames()
                .OrderBy(sk => sk)
                .Select(sk =>
                $"{indent}{sk}{Environment.NewLine}{PrintKey(key.OpenSubKey(sk), depth + 1)}");

            return string.Join(Environment.NewLine, values.Concat(subKeys));
        }

        /// <summary>
        /// Prints the specified registry view. Used for functional testing.
        /// </summary>
        /// <param name="registryView">The registry view.</param>
        /// <returns>The registry view as a string.</returns>
        public string Print(RegistryView registryView)
        {
            string v = string.Empty;
            //  Go through the hives. We'll only print them if they have keys.
            foreach (var rootKey in _rootKeys.Where(rk => rk.Key.Item1 == registryView).OrderBy(k => k.Value.Name))
            {
                string print = rootKey.Value.Name;
                string val = PrintKey(rootKey.Value, 1);
                if (!string.IsNullOrEmpty(val))
                {
                    v += print + Environment.NewLine;
                    v += val + Environment.NewLine;
                    v += Environment.NewLine;
                }
            }

            return v.Trim();
        }
    }
}