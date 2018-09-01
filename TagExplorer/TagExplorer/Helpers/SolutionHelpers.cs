using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;

namespace TagExplorer
{
    class SolutionHelpers
    {
        private static string sTagIdentifier = "Tags";

        public static void SetTag( IVsSolution cSolution, ProjectItem cProjectItem, string sTag )
        {
            IVsHierarchy hierarchy;
            cSolution.GetProjectOfUniqueName( cProjectItem.ContainingProject.UniqueName, out hierarchy );
            IVsBuildPropertyStorage buildPropertyStorage = hierarchy as IVsBuildPropertyStorage;
            if ( buildPropertyStorage != null )
            {
                uint itemId;
                hierarchy.ParseCanonicalName( (string) cProjectItem.Properties.Item( "FullPath" ).Value, out itemId );
                buildPropertyStorage.SetItemAttribute( itemId, sTagIdentifier, sTag );
            }
        }

        public static string GetTag( IVsSolution cSolution, ProjectItem cProjectItem )
        {
            string sReturn = "";
            IVsHierarchy hierarchy;
            cSolution.GetProjectOfUniqueName( cProjectItem.ContainingProject.UniqueName, out hierarchy );
            IVsBuildPropertyStorage buildPropertyStorage = hierarchy as IVsBuildPropertyStorage;
            if ( buildPropertyStorage != null )
            {
                uint itemId;
                hierarchy.ParseCanonicalName( (string) cProjectItem.Properties.Item( "FullPath" ).Value, out itemId );
                buildPropertyStorage.GetItemAttribute( itemId, sTagIdentifier, out sReturn );
            }
            return sReturn;
        }
    }
}
