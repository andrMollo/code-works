using CADBooster.SolidDna;

namespace CodeWorksLibrary
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public class AddIn : Xarial.XCad.SolidWorks.SwAddInEx
    {
        public override void OnConnect()
        {
            AddInIntegration.ConnectToActiveSolidWorks(this.Application.Sw.RevisionNumber(), this.AddInId);

            Application.ShowMessageBox("Hello xCAD");
        }
    }
}
