﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetStudyProject.Core.Entities.S3
{
    public class S3Object
    {
        public string Name { get; set; } = null!;
        public MemoryStream InputStream { get; set; } = null!;
        public string BucketName { get; set; } = null!;
    }
}
