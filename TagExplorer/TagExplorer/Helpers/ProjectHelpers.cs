using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;

namespace TagExplorer
{
    internal static class ProjectHelpers
    {
        public static ProjectItem GetSelectedItem( DTE2 dte )
        {
            var items = (Array) dte.ToolWindows.SolutionExplorer.SelectedItems;

            if ( items.Length == 1 )
            {
                UIHierarchyItem selItem = items.GetValue( 0 ) as UIHierarchyItem;
                return selItem.Object as ProjectItem;
            }

            return null;
        }

        public static string GetSelectedPath( DTE2 dte, bool openSolutionProjectAsRegularFile )
        {
            var items = (Array) dte.ToolWindows.SolutionExplorer.SelectedItems;
            string file = "";

            if ( items.Length == 1 )
            {

                foreach ( UIHierarchyItem selItem in items )
                {
                    ProjectItem item = selItem.Object as ProjectItem;

                    if ( item != null )
                    {
                        file = item.GetFilePath();
                    }
                }
            }

            return file;
        }

        public static string GetFilePath( this ProjectItem item )
        {
            return $"\"{item.FileNames[1]}\""; // Indexing starts from 1
        }

        public static string GetRootFolder( this Project project )
        {
            if ( string.IsNullOrEmpty( project.FullName ) )
                return null;

            string fullPath;

            try
            {
                fullPath = project.Properties.Item( "FullPath" ).Value as string;
            }
            catch ( ArgumentException )
            {
                try
                {
                    // MFC projects don't have FullPath, and there seems to be no way to query existence
                    fullPath = project.Properties.Item( "ProjectDirectory" ).Value as string;
                }
                catch ( ArgumentException )
                {
                    // Installer projects have a ProjectPath.
                    fullPath = project.Properties.Item( "ProjectPath" ).Value as string;
                }
            }

            if ( string.IsNullOrEmpty( fullPath ) )
                return File.Exists( project.FullName ) ? Path.GetDirectoryName( project.FullName ) : null;

            if ( Directory.Exists( fullPath ) )
                return fullPath;

            if ( File.Exists( fullPath ) )
                return Path.GetDirectoryName( fullPath );

            return null;
        }
    }
}
