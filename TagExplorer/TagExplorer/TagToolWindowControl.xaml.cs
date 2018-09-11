namespace TagExplorer
{
    using EnvDTE;
    using EnvDTE80;
    using Microsoft.VisualStudio.Shell.Interop;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for TagToolWindowControl.
    /// </summary>
    public partial class TagToolWindowControl : UserControl
    {
        private readonly IVsSolution m_vsSolution;
        private readonly DTE2 m_dte;

        public TagToolWindowControl( IVsSolution vsSolution, DTE2 dte )
        {
            m_vsSolution = vsSolution;
            m_dte = dte;
            this.InitializeComponent();
        }

        private void btnRefresh_Click( object sender, RoutedEventArgs e )
        {
            treeTags.Items.Clear();

            if ( m_dte.Solution != null &&
                m_dte.Solution.IsOpen )
            {
                Solution sol = m_dte.Solution;
                foreach ( var project in sol.Projects )
                {
                    NavigateProject( (Project) project );
                }
            }
        }

        private void NavigateProject( Project project )
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            IVsHierarchy hierarchy;
            m_vsSolution.GetProjectOfUniqueName( project.UniqueName, out hierarchy );

            var buildPropertyStorage = hierarchy as IVsBuildPropertyStorage;

            foreach ( ProjectItem item in project.ProjectItems )
            {
                if ( buildPropertyStorage != null )
                {
                    NavigateProjectItem( hierarchy, buildPropertyStorage, item );
                }
                else
                {
                    if ( item.Object is Project )
                        NavigateProject( item.Object as Project );
                }

            }
        }

        private void NavigateProjectItem( IVsHierarchy hierarchy, IVsBuildPropertyStorage buildPropertyStorage,
                                         ProjectItem rootItem )
        {
            if ( rootItem == null || rootItem.Properties == null )
                return;

            if ( rootItem.ProjectItems != null )
            {
                foreach ( object item in rootItem.ProjectItems )
                {
                    NavigateProjectItem( hierarchy, buildPropertyStorage, item as ProjectItem );
                }
            }

            string fullPath = rootItem.Properties
                .Cast<Property>()
                .Where( p => p.Name == "FullPath" )
                .Select( p => p.Value.ToString() )
                .FirstOrDefault();

            if ( string.IsNullOrEmpty( fullPath ) )
                return;


            uint itemId = 0;
            hierarchy.ParseCanonicalName( fullPath, out itemId );
            string tags;
            buildPropertyStorage.GetItemAttribute( itemId, "Tags", out tags );

            if ( string.IsNullOrEmpty( tags ) )
                return;

            foreach ( string tag in tags.Split( ';' ) )
            {
                AddFileToTag( rootItem, tag );
            }

        }

        private void AddFileToTag( ProjectItem projectItem, string tag )
        {
            foreach ( object item in treeTags.Items )
            {
                var node = item as TreeViewItem;
                if ( node == null )
                    continue;

                if ( (string) node.Header == tag )
                {
                    var newNode = new TreeViewItem { Header = projectItem.Name, DataContext = projectItem };
                    //newNode.MouseDoubleClick += TagsNode_MouseDoubleClick;
                    //newNode.KeyUp += TagsNode_KeyUp;
                    node.Items.Add( newNode );
                    return;
                }
            }

            var tagNode = new TreeViewItem { Header = tag };
            var childNode = new TreeViewItem { Header = projectItem.Name, DataContext = projectItem };
            //childNode.MouseDoubleClick += TagsNode_MouseDoubleClick;
            //childNode.KeyUp += TagsNode_KeyUp;
            tagNode.Items.Add( childNode );
            treeTags.Items.Add( tagNode );
        }
    }
}