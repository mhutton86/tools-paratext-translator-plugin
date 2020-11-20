﻿using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using TVPMain.Util;

namespace TvpMain.CheckManagement
{
    public class S3Service : IRemoteService
    {
        // Read-only PPM repository and CLI AWS configuration parameters.
        const string accessKey = AWSCredentials.AWS_TVP_ACCESS_KEY_ID;
        const string secretKey = AWSCredentials.AWS_TVP_ACCESS_KEY_SECRET;
        private string BucketName = AWSCredentials.AWS_TVP_BUCKET_NAME;

        public virtual string GetBucketName()
        {
            return BucketName;
        }

        private void SetBucketName(string value)
        {
            BucketName = value;
        }

        private AmazonS3Client s3Client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.USEast1);

        /// <summary>
        /// The S3 client we use for S3 requests.
        /// </summary>
        public virtual AmazonS3Client GetS3Client()
        {
            return s3Client;
        }

        /// <summary>
        /// The S3 client we use for S3 requests.
        /// </summary>
        private void SetS3Client(AmazonS3Client value)
        {
            s3Client = value;
        }

        public List<string> ListAllFiles()
        {
            List<string> checkFileNames = new List<string>();
            ListObjectsV2Request request = new ListObjectsV2Request
            {
                BucketName = GetBucketName(),
                MaxKeys = 10
            };
            ListObjectsV2Response response;
            do
            {
                response = GetS3Client().ListObjectsV2(request);

                // Process the response.
                foreach (S3Object entry in response.S3Objects)
                {
                    checkFileNames.Add(entry.Key);
                }
                request.ContinuationToken = response.NextContinuationToken;
            } while (response.IsTruncated);

            return checkFileNames;
        }

        public async Task<List<string>> ListAllFilesAsync()
        {
            List<string> checkFileNames = new List<string>();
            ListObjectsV2Request request = new ListObjectsV2Request
            {
                BucketName = GetBucketName(),
                MaxKeys = 10
            };
            ListObjectsV2Response response;
            do
            {
                response = await GetS3Client().ListObjectsV2Async(request);

                // Process the response.
                foreach (S3Object entry in response.S3Objects)
                {
                    checkFileNames.Add(entry.Key);
                }
                request.ContinuationToken = response.NextContinuationToken;
            } while (response.IsTruncated);

            return checkFileNames;
        }

        public Stream GetFileStream(string file)
        {
            GetObjectRequest getObjectRequest = new GetObjectRequest
            {
                BucketName = GetBucketName(),
                Key = file
            };

            GetObjectResponse getObjectResponse = GetS3Client().GetObject(getObjectRequest);

            return getObjectResponse.ResponseStream;
        }

        public async Task<Stream> GetFileStreamAsync(string file)
        {
            GetObjectRequest getObjectRequest = new GetObjectRequest
            {
                BucketName = GetBucketName(),
                Key = file
            };

            GetObjectResponse getObjectResponse = await GetS3Client().GetObjectAsync(getObjectRequest);

            return getObjectResponse.ResponseStream;
        }

        public HttpStatusCode PutFileStream(string filename, Stream file)
        {
            PutObjectRequest putObjectRequest = new PutObjectRequest
            {
                BucketName = GetBucketName(),
                Key = filename,
                InputStream = file
            };

            PutObjectResponse putObjectResponse = GetS3Client().PutObject(putObjectRequest);

            return putObjectResponse.HttpStatusCode;
        }

        public async Task<HttpStatusCode> PutFileStreamAsync(string filename, Stream file)
        {
            PutObjectRequest putObjectRequest = new PutObjectRequest
            {
                BucketName = GetBucketName(),
                Key = filename,
                InputStream = file
            };

            PutObjectResponse putObjectResponse = await GetS3Client().PutObjectAsync(putObjectRequest);

            return putObjectResponse.HttpStatusCode;
        }

        public HttpStatusCode DeleteFile(string filename)
        {
            DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = GetBucketName(),
                Key = filename
            };

            DeleteObjectResponse deleteObjectResponse = GetS3Client().DeleteObject(deleteObjectRequest);

            return deleteObjectResponse.HttpStatusCode;
        }

        public async Task<HttpStatusCode> DeleteFileAsync(string filename)
        {
            DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = GetBucketName(),
                Key = filename
            };

            DeleteObjectResponse delectObjectResponse = await GetS3Client().DeleteObjectAsync(deleteObjectRequest);

            return delectObjectResponse.HttpStatusCode;
        }
    }
}
