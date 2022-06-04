using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleFileManager
{
    internal class ProcessCommandLine : IFileIO
    {
        FileManagerClass fm;
        string[] args;
        public ProcessCommandLine(string[] args)
        {
            fm = new FileManagerClass();
            this.args = args;
            Start();
        }

        public void CopyDirectory(string sourceDir, string destinationDir)
        {
            fm.CopyDirectory(sourceDir, destinationDir);
        }

        public void CopyFile(string sourcePath, string destinationPath)
        {
            fm.CopyFile(sourcePath, destinationPath);
        }

        public void CreateNewDirectory(string filename, string destinationPath)
        {
            fm.CreateNewDirectory(filename, destinationPath);
        }

        public void CreateNewDirectory(string filename)
        {
            fm.CreateNewDirectory(filename);
        }

        public void CreateNewFile(string filename, string destinationPath)
        {
            fm.CreateNewFile(filename, destinationPath);
        }

        public void CreateNewFile(string filename)
        {
            fm.CreateNewFile(filename);
        }

        public void RemoveDir(string sourcePath)
        {
            fm.RemoveDir(sourcePath);
        }

        public void RemoveFile(string sourcePath)
        {
            fm.RemoveFile(sourcePath);
        }

        public void Start()
        {
            switch(args[0])
            {
                //touch myfile1.txt C:\Users\sined - win\Desktop
                case "touch":
                    CreateNewFile(args[1], args[2]);
                    break;
                //mkdir myDir1 "C:\Users\sined-win\Desktop"
                case "mkdir":
                    CreateNewDirectory(args[1], args[2]);
                    break;
                case "cp":
                    FileAttributes attrCopy = File.GetAttributes(args[1]);
                    if(attrCopy.HasFlag(FileAttributes.Directory))
                    {
                        // sourcePath - "C:\Users\sined-win\Desktop\fl"
                        // destinationPath - "C:\Users\sined-win\Desktop\fl-bak"
                        CopyDirectory(args[1], args[2]);
                    }
                    else
                    { 
                        // sourcePath - "C:\Users\sined-win\Desktop\warmane.txt"
                        // destinationPath - "C:\Users\sined-win\Desktop\warmane-bak.txt"
                        CopyFile(args[1], args[2]);
                    }
                    break;
                case "mv":
                    FileAttributes attrMove = File.GetAttributes(args[1]);
                    if (attrMove.HasFlag(FileAttributes.Directory))
                    {
                        // sourcePath - "C:\Users\sined-win\Desktop\fl"
                        // destinationPath - "C:\Users\sined-win\Desktop\fl-bak"
                        CopyDirectory(args[1], args[2]);
                        RemoveDir(args[1]);
                    }
                    else
                    {
                        // sourcePath - "C:\Users\sined-win\Desktop\warmane.txt"
                        // destinationPath - "C:\Users\sined-win\Desktop\warmane-bak.txt"
                        CopyFile(args[1], args[2]);
                        RemoveFile(args[1]);
                    }
                    break;
                case "rm":
                    FileAttributes attrRem = File.GetAttributes(args[1]);
                    if (attrRem.HasFlag(FileAttributes.Directory))
                    {
                        // sourcePath - "C:\Users\sined-win\Desktop\fl"
                        RemoveDir(args[1]);
                    }
                    else
                    {
                        // sourcePath - "C:\Users\sined-win\Desktop\warmane.txt"
                        RemoveFile(args[1]);
                    }
                    break;
            }
        }
    }
}
