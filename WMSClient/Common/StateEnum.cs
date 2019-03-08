using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSClient.Common
{
    public enum WMSClientDisPatchAGV
    {
        Idle,
        Busy
    }
    public enum WMSClientDisPatchWarehousePLC
    {
        Idle,
        Busy
    }
    public enum WMSClientDisPatchDataBase
    {
        Idle,
        Busy
    }

    public enum WMSClientInput
    {
        Idle,
        InputLoadStart,
        InputLoadComplete,
        ReadRFIDStart,
        ReadRFIDComplete,
        CreateRFIDStart,
        CreateRFIDComplete,
        WriteRFIDStart,
        WriteRFIDComplete,
        InputExecuteStart,
        InputExecyteComplete,
    }
}
