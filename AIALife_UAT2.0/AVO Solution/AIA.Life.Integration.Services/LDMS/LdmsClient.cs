using AIA.Life.Models.Common;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Integration.Services.LDMS
{
    public static class LdmsClient
    {
        public static void UploadsFTP(LdmsDocuments ldmsDocuments)
        {
            try
            {
                //string filePath = "C:\\users\\rajiv.n\\desktop\\50129915.png";
                var host = ConfigurationManager.AppSettings["SFTPHOST"].ToString();
                var port = 22;
                var username = ConfigurationManager.AppSettings["SFTPUSER"].ToString();
                var password = ConfigurationManager.AppSettings["SFTPPWD"].ToString();
                SftpClient client = new SftpClient(host, port, username, password);
                client.Connect();
                string directoryPath = ConfigurationManager.AppSettings["SFTPPATH"].ToString();
                directoryPath = directoryPath + "/" + ldmsDocuments.AgentCode + "/" + ldmsDocuments.ProposalNo + "/";
                CreateDirectoryRecursively(client, directoryPath);
                client.ChangeDirectory(directoryPath);
                FileStream fs = new FileStream(ldmsDocuments.SourcePath, FileMode.Open);
                client.BufferSize = 1024;
                client.UploadFile(fs,ldmsDocuments.DocCode+Path.GetExtension(ldmsDocuments.SourcePath));
                client.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private static void CreateDirectoryRecursively(SftpClient client, string path)
        {
            string current = "";

            if (path[0] == '/')
            {
                path = path.Substring(1);
            }

            while (!string.IsNullOrEmpty(path))
            {
                int p = path.IndexOf('/');
                current += '/';
                if (p >= 0)
                {
                    current += path.Substring(0, p);
                    path = path.Substring(p + 1);
                }
                else
                {
                    current += path;
                    path = "";
                }

                if (client.Exists(current))
                {
                    SftpFileAttributes attrs = client.GetAttributes(current);
                    if (!attrs.IsDirectory)
                    {
                        throw new Exception("not directory");
                    }
                }
                else
                {
                    client.CreateDirectory(current);
                }
            }
        }
    }
}
