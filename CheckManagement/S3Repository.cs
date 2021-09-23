﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TvpMain.Check;

namespace TvpMain.CheckManagement
{
    /// <summary>
    /// This class works with a remote, S3-based repository for checks and fixes.
    /// </summary>
    public class S3Repository : IRepository
    {
        /// <summary>
        /// The service which interacts with S3.
        /// </summary>
        protected virtual IRemoteService Service { get; } = S3ServiceProvider.Instance;

        public void AddCheckAndFixItem(string filename, CheckAndFixItem item)
        {
            if (String.IsNullOrEmpty(filename)) new ArgumentNullException(nameof(filename));

            Service.PutFileStream(filename, item.WriteToXmlStream());
        }

        public async Task AddCheckAndFixItemAsync(string filename, CheckAndFixItem item)
        {
            if (String.IsNullOrEmpty(filename)) new ArgumentNullException(nameof(filename));

            await Service.PutFileStreamAsync(filename, item.WriteToXmlStream());
        }

        public List<CheckAndFixItem> GetCheckAndFixItems()
        {
            var checkAndFixItems = new List<CheckAndFixItem>();

            // Whether the string represents an XML filename.
            // This is a simple way to guard against non-check files in the repository.
            static bool IsXmlFile(string filename) => filename.Trim().ToLowerInvariant().EndsWith(".xml");
            
            var filenames = Service.ListAllFiles().Where((Func<string,bool>) IsXmlFile).ToList();
            foreach (var file in filenames)
            {
                using var fileStream = Service.GetFileStream(file);
                var checkAndFixItem = ReadCheckAndFixItemFromStream(fileStream);
                if (checkAndFixItem != null) checkAndFixItems.Add(checkAndFixItem);
            }

            return checkAndFixItems;
        }

        /// <summary>
        /// This loads a <c>CheckAndFixItem</c> from a <c>Stream</c>, guarding against invalid files.
        /// </summary>
        /// <param name="stream">The <c>Stream</c> of a file representing a <c>CheckAndFixItem</c>.</param>
        /// <returns></returns>
        private CheckAndFixItem ReadCheckAndFixItemFromStream(Stream stream)
        {
            CheckAndFixItem checkAndFixItem = null;
            checkAndFixItem = CheckAndFixItem.LoadFromXmlContent(stream);

            return checkAndFixItem;
        }

        public void RemoveCheckAndFixItem(string filename)
        {
            Service.DeleteFile(filename);
        }
    }
}
