namespace TagExplorer
{
    using System;
    using System.Runtime.InteropServices;
    using EnvDTE;
    using EnvDTE80;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid( "2b3d1874-daba-40f7-98b7-86a1544e46a7" )]
    public class TagToolWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagToolWindow"/> class.
        /// </summary>
        public TagToolWindow() : base( null )
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            this.Caption = "TagToolWindow";
            

            var dte = (DTE2) ServiceProvider.GlobalProvider.GetService( typeof( DTE ) );
            var solution = (IVsSolution) ServiceProvider.GlobalProvider.GetService( typeof( SVsSolution ) );
            this.Content = new TagToolWindowControl( solution, dte );
        }
        
    }
}
