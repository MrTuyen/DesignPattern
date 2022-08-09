using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBackupMySQL
{
    public enum GetTotalRowsMethod
    {
        Skip = 1,
        InformationSchema = 2,
        SelectCount = 3
    }
}
