namespace BrokeredServer
{
    public sealed class HostProcessManager
    {
        #region dllhost process
        public int ProcessId { get { return System.Diagnostics.Process.GetCurrentProcess().Id; } }

        public void KillProcess()
        {
            System.Diagnostics.Process.GetProcessById(ProcessId).Kill();
        }
        #endregion
    }
}
