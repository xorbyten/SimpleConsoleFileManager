using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleFileManager
{
    internal interface IFileIO
    {
        /// <summary>
        /// Метод, который инициирует всю работу.
        /// </summary>
        void Start();
        /// <summary>
        /// Метод, позволяющий удалить файл.
        /// </summary>
        /// <param name="sourcePath"></param>
        void RemoveFile(string sourcePath);
        /// <summary>
        /// Метод, позволяющий удалить директорию.
        /// </summary>
        /// <param name="sourcePath"></param>
        void RemoveDir(string sourcePath);
        /// <summary>
        /// Метод, позволяющий создать новую директорию.
        /// </summary>
        /// <param name="filename"></param>
        void CreateNewDirectory(string filename);
        /// <summary>
        ///  Перегруженый метод позволяющий создать новую директорию.
        /// </summary>
        /// /// <param name="filename"></param>
        /// /// <param name="destinationPath"></param>
        void CreateNewDirectory(string filename, string destinationPath);
        /// <summary>
        /// Метод, позволяющий создать новый текстовый документ.
        /// </summary>
        /// <param name="filename"></param>
        void CreateNewFile(string filename);
        /// <summary>
        /// Метод, позволяющий создать новый текстовый документ.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="destinationPath"></param>
        void CreateNewFile(string filename, string destinationPath);
        /// <summary>
        /// Метод, позволяющий скопировать каталог в новое местоположение.
        /// </summary>
        /// <param name="dirname"></param>
        /// <param name="destination"></param>
        void CopyDirectory(string sourceDir, string destinationDir);
        /// <summary>
        /// Метод, позволяющий скопировать файл в новое местоположение.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="destination"></param>
        void CopyFile(string sourcePath, string destinationPath);
    }
}
