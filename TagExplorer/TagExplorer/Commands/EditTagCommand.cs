using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;
using EnvDTE;
using EnvDTE80;
using System.Windows.Forms;

namespace TagExplorer.Commands
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class EditTagCommand
    {
        public const int CommandId = 256;

        public static readonly Guid CommandSet = new Guid("b616aac5-6b80-4333-8cf3-b531d6974dfa");

        private readonly Package _package;

        private EditTagCommand( Package package )
        {
            _package = package;

            var commandService = (OleMenuCommandService) ServiceProvider.GetService( typeof( IMenuCommandService ) );

            if ( commandService != null )
            {
                var menuCommandID = new CommandID( CommandSet, CommandId );
                var menuItem = new MenuCommand( EditTag, menuCommandID );
                commandService.AddCommand( menuItem );
            }

        }

        public static EditTagCommand Instance { get; private set; }

        private IServiceProvider ServiceProvider
        {
            get { return _package; }
        }

        public static void Initialize( Package package )
        {
            Instance = new EditTagCommand( package );
        }


        private void EditTag( object sender, EventArgs e )
        {
            try
            {
                var dte = (DTE2) ServiceProvider.GetService( typeof( DTE ) );

                ProjectItem item = ProjectHelpers.GetSelectedItem( dte );
                //string path = ProjectHelpers.GetSelectedPath( dte, true );

                if ( item != null )
                {
                    Project proj = item.ContainingProject;
                    if ( proj != null )
                    {
                        MessageBox.Show( "ProjName:" + proj.Name );
                    }
                }
                else
                {
                    MessageBox.Show( "Couldn't resolve the file. Maybe multiselected files." );
                }
            }
            catch ( Exception ex )
            {
                Logger.Log( ex );
            }
        }

    }
}
