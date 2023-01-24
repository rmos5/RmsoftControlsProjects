using System;

namespace RmsoftControls.Dialogs
{
    [Flags]
    public enum DialogButtons : int
    {
        None = 0,
        Ok = 1,
        Cancel = 2,
        OkCancel = Ok | Cancel
    }
}
