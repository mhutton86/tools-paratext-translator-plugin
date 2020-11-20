﻿using PtxUtils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TvpMain.Check;
using TvpMain.Util;

namespace TvpMain.CheckManagement
{
    public class CheckManager : ICheckManager
    {
        private readonly LocalRepository installedChecksRepository;
        private readonly LocalRepository locallyDevelopedChecksRepository;

        private readonly S3Repository s3Repository;

        public CheckManager()
        {
            installedChecksRepository = new InstalledChecksRepository();
            locallyDevelopedChecksRepository = new LocallyDevelopedChecksRepository();
            s3Repository = new S3Repository();
        }

        public List<CheckAndFixItem> GetRemoteCheckAndFixItems()
        {
            return s3Repository.GetCheckAndFixItems();
        }

        public virtual void InstallCheckAndFixItem(CheckAndFixItem item)
        {
            string filename = GetCheckAndFixItemFilename(item);
            installedChecksRepository.AddCheckAndFixItem(filename, item);
        }

        public Task InstallCheckAndFixItemAsync(CheckAndFixItem item)
        {
            string filename = GetCheckAndFixItemFilename(item);
            return installedChecksRepository.AddCheckAndFixItemAsync(filename, item);
        }
        
        public virtual void SaveCheckAndFixItem(CheckAndFixItem item)
        {
            string filename = GetCheckAndFixItemFilename(item);
            locallyDevelopedChecksRepository.AddCheckAndFixItem(filename, item);
        }

        public Task SaveCheckAndFixItemAsync(CheckAndFixItem item)
        {
            string filename = GetCheckAndFixItemFilename(item);
            return locallyDevelopedChecksRepository.AddCheckAndFixItemAsync(filename, item);
        }

        public virtual void UninstallCheckAndFixItem(CheckAndFixItem item)
        {
            string filename = GetCheckAndFixItemFilename(item);
            installedChecksRepository.RemoveCheckAndFixItem(filename);
        }
        public virtual void DeleteCheckAndFixItem(CheckAndFixItem item)
        {
            string filename = GetCheckAndFixItemFilename(item);
            locallyDevelopedChecksRepository.RemoveCheckAndFixItem(filename);
        }

        public void PublishCheckAndFixItem(CheckAndFixItem item)
        {
            string filename = GetCheckAndFixItemFilename(item);
            s3Repository.AddCheckAndFixItem(filename, item);
        }

        public Task PublishCheckAndFixItemAsync(CheckAndFixItem item)
        {
            string filename = GetCheckAndFixItemFilename(item);
            return s3Repository.AddCheckAndFixItemAsync(filename, item);
        }

        public virtual List<CheckAndFixItem> GetAvailableCheckAndFixItems()
        {
            return s3Repository.GetCheckAndFixItems();
        }
        public virtual List<CheckAndFixItem> GetInstalledCheckAndFixItems()
        {
            return installedChecksRepository.GetCheckAndFixItems();
        }
        
        public virtual List<CheckAndFixItem> GetSavedCheckAndFixItems()
        {
            return locallyDevelopedChecksRepository.GetCheckAndFixItems();
        }

        public virtual Dictionary<CheckAndFixItem, CheckAndFixItem> GetOutdatedCheckAndFixItems()
        {
            List<CheckAndFixItem> availableCheckAndFixItems = GetAvailableCheckAndFixItems();
            availableCheckAndFixItems.Sort((x, y) => new Version(y.Version).CompareTo(new Version(x.Version)));
            List<CheckAndFixItem> installedCheckAndFixItems = GetInstalledCheckAndFixItems();
            Dictionary<CheckAndFixItem, CheckAndFixItem> outdatedCheckAndFixItems = new Dictionary<CheckAndFixItem, CheckAndFixItem>();

            installedCheckAndFixItems.ForEach(installedCheck =>
            {
                CheckAndFixItem availableUpdate = availableCheckAndFixItems.Find(availableCheck => IsNewVersion(installedCheck, availableCheck));
                if (availableUpdate != default(CheckAndFixItem)) outdatedCheckAndFixItems.Add(installedCheck, availableUpdate);
            });

            return outdatedCheckAndFixItems;
        }

        /// <summary>
        /// This method creates a filename for the provided <c>CheckAndFixItem</c>.
        /// </summary>
        /// <param name="item">The <c>CheckAndFixItem</c> for which to produce a filename.</param>
        /// <returns>The filename produced for the provided <c>CheckAndFixItem</c>.</returns>
        private string GetCheckAndFixItemFilename(CheckAndFixItem item)
        {
            return $"{item.Name.ConvertToTitleCase().Replace(" ", String.Empty)}-{item.Version.Trim()}.{MainConsts.CHECK_FILE_EXTENSION}";
        }

        /// <summary>
        /// This method compares two checks and determines whether the candidate is an updated version of the original.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="candidate"></param>
        /// <returns></returns>
        public virtual Boolean IsNewVersion(CheckAndFixItem original, CheckAndFixItem candidate)
        {
            return String.Equals(candidate.Name, original.Name) &&
                    Version.Parse(candidate.Version) > Version.Parse(original.Version);
        }
    }
}
