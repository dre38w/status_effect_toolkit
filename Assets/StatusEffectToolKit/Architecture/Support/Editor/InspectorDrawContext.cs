/*
 * Description: Informs custom drawers that things are being rendered in a window rather than Unity's inspector
 */

#if UNITY_EDITOR
using System;

namespace Support.Editor
{
    internal static class InspectorDrawContext
    {
        internal static bool IsToolWindow { get; private set; }

        /// <summary>
        /// Create a temporary "scope" so we can track when the content is in/out of the tool window
        /// </summary>
        /// <returns></returns>
        internal static IDisposable ToolWindowScope()
        {
            return new Scope();
        }

        private sealed class Scope : IDisposable
        {
            //stores the previous content so we can restore it later
            private readonly bool isPrevious;

            public Scope()
            {
                //save the state to restore to
                isPrevious = IsToolWindow;
                //indicate we are now in the tool window
                IsToolWindow = true;
            }

            /// <summary>
            /// Clean up
            /// </summary>
            public void Dispose()
            {
                //we exited so restore the previous content
                IsToolWindow = isPrevious;
            }
        }
    }
}
#endif