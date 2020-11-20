﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvpMain.Util;

namespace TvpMain.CheckManagement
{
    class LocallyDevelopedChecksRepository : LocalRepository
    {
        private readonly string _folderPath = Path.Combine(Directory.GetCurrentDirectory(), MainConsts.LOCAL_CHECK_FOLDER_NAME);

        public override string GetFolderPath()
        {
            return _folderPath;
        }
    }
}
