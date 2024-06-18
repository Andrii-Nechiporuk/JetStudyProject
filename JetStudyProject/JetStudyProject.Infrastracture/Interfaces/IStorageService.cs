using Amazon.Runtime;
using JetStudyProject.Helpers;
using JetStudyProject.Infrastracture.DTOs.S3BucketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetStudyProject.Core.Entities.S3;


namespace JetStudyProject.Infrastracture.Interfaces
{
    public interface IStorageService
    {
        Task<S3ResponseDto> UploadFileAsync(S3Object s3obj, AwsCredentials awsCredentials);
    }
}
