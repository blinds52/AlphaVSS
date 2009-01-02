/* Copyright (c) 2008-2009 Peter Palotas
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */
using System;
using System.Reflection;
using System.Runtime.Remoting;

namespace Alphaleonis.Win32.Vss
{
    /// <summary>
    ///     The <see cref="VssUtils"/> class is a static utility class for accessing the platform specific
    ///     instances of the various VSS interfaces in a platform-independent manner.
    /// </summary>
    /// <remarks>
    ///     Use the <see cref="O:Alphaleonis.Win32.Vss.VssUtils.LoadImplementation"/> under normal circumstances to load
    ///     the correct assembly and create an instance of <see cref="IVssImplementation"/> from that assembly. If you have 
    ///     specific requirements on how the assembly should be loaded, or the instance created you are not required to use 
    ///     these methods but can use the methods in this class for accessing the suggested assembly name to load, and load it manually.
    ///     In this case you need to create an instance of the class called <c>Alphaleonis.Win32.Vss.VssImplementation</c> from the platform specific
    ///     assembly. This class implements the <see cref="IVssImplementation"/> interface, and has a public parameterless constructor.
    /// </remarks>
    public static class VssUtils
    {
        /// <summary>
        /// Gets the short name of the platform specific assembly for the platform on which the assembly 
        /// is currently executing.
        /// </summary>
        /// <returns>the short name of the platform specific assembly for the platform on which the assembly 
        /// is currently executing.</returns>
        /// <exception cref="UnsupportedOperatingSystemException">The operating system could not be detected or is unsupported.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public static string GetPlatformSpecificAssemblyShortName()
        {
            string winVer;
            switch (OperatingSystemInfo.OSVersionName)
            {
                case OSVersionName.Windows2000:
                    throw new UnsupportedOperatingSystemException("Windows 2000 is not supported");
                case OSVersionName.WindowsXP:
                    winVer = "WinXP";
                    break;
                case OSVersionName.WindowsServer2003:
                    winVer = "Win2003";
                    break;
                case OSVersionName.WindowsVista:
                    winVer = "Win2008";
                    break;
                case OSVersionName.WindowsServer2008:
                    winVer = "Win2003";
                    break;
                case OSVersionName.Unknown:
                default:
                    throw new UnsupportedOperatingSystemException("Failed to detect running operating system, or current operating system not supported.");
            }

            string archName;
            switch (OperatingSystemInfo.ProcessorArchitecture)
            {
                case ProcessorArchitecture.X86:
                    archName = "x86";
                    break;
                case ProcessorArchitecture.IA64:
                    throw new UnsupportedOperatingSystemException("IA64 architecture is not supported.");
                case ProcessorArchitecture.X64:
                    archName = "x64";
                    break;
                case ProcessorArchitecture.Unknown:
                default:
                    throw new UnsupportedOperatingSystemException("Failed to detect architecture of running operating system.");
            }
#if DEBUG 
            return "AlphaVSS." + winVer + ".Debug." + archName;
#else
            return "AlphaVSS." + winVer + "." + archName;
#endif
        }

        /// <summary>
        /// Gets the full name of the platform specific assembly for the platform on which the assembly is currently executing.
        /// </summary>
        /// <returns>The full name of the platform specific assembly for the platform on which the assembly is currently executing.</returns>
        /// <exception cref="UnsupportedOperatingSystemException">The operating system could not be detected or is unsupported.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public static AssemblyName GetPlatformSpecificAssemblyName()
        {
            return new AssemblyName(
                GetPlatformSpecificAssemblyShortName() + 
                ", Version=" + 
                Assembly.GetExecutingAssembly().GetName().Version.ToString() + 
                ", PublicKeyToken=3033cf2dbd31cad3");
        }

        /// <summary>
        /// Loads the assembly containing the correct implementation of the <see cref="IVssImplementation"/> interface
        /// for the operating system on which the assembly is currently executing. 
        /// </summary>
        /// <overloads>
        /// Loads the assembly containing the correct implementation of the <see cref="IVssImplementation"/> interface
        /// for the operating system on which the assembly is currently executing, optionally allowing the specification
        /// of an <see cref="AppDomain"/> into which to load the assembly.
        /// </overloads>
        /// <remarks>
        ///     <para>
        ///         The assembly will be loaded into the same <see cref="AppDomain"/> as the calling assembly.
        ///     </para>
        ///     <para>
        ///         The assemblies are loaded using strong name lookup. They need to be present in the code base directory
        ///         of the executing assembly, or installed in the GAC for the lookup to succeed.
        ///     </para>
        /// </remarks>
        /// 
        /// <returns>An newly created instance of <see cref="IVssImplementation"/> suitable for the 
        /// operating system on which the assembly is currently executing.</returns>
        /// <exception cref="UnsupportedOperatingSystemException">The operating system could not be detected or is unsupported.</exception>
        public static IVssImplementation LoadImplementation()
        {
            Assembly assembly = Assembly.Load(GetPlatformSpecificAssemblyName());           
            return (IVssImplementation)assembly.CreateInstance("Alphaleonis.Win32.Vss.VssImplementation");
        }

        /// <summary>
        /// Loads the assembly containing the correct implementation of the <see cref="IVssImplementation"/> interface
        /// for the operating system on which the assembly is currently executing allowing the specification
        /// of an <see cref="AppDomain"/> into which to load the assembly.
        /// </summary>
        /// <param name="domain">The <see cref="AppDomain"/> into which to load the platform specific assembly.</param>
        /// <remarks>
        ///     <para>
        ///         The assemblies are loaded using strong name lookup. They need to be present in the code base directory
        ///         of the executing assembly, or installed in the GAC for the lookup to succeed.
        ///     </para>
        /// </remarks>
        /// <returns>
        ///     An newly created instance of <see cref="IVssImplementation"/> suitable for the
        ///     operating system on which the assembly is currently executing.
        /// </returns>
        /// <exception cref="UnsupportedOperatingSystemException">The operating system could not be detected or is unsupported.</exception>
        public static IVssImplementation LoadImplementation(AppDomain domain)
        {
            domain.Load(GetPlatformSpecificAssemblyName());            
            ObjectHandle handle = domain.CreateInstance(GetPlatformSpecificAssemblyName().ToString(), "Alphaleonis.Win32.Vss.VssImplementation");
            return (IVssImplementation)handle.Unwrap();
        }
    }
}
