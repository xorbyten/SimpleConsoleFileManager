using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleFileManager
{
    internal class FileManagerClass : IFileIO
    {
        // Переменная содержащая путь
        string path;
        // Свойство для получения и установки пути по-умолчанию
        public string GetPath { get { return path; } set { path = value; } }
        // Переменная для хранения текущего пункта меню
        int index;
        // Объект, хранящий информацию о нажатых клавишах
        ConsoleKeyInfo keyInfo;
        // Список файлов и папок для вывода на консоль
        List<string> list;
        // Массив директорий
        string[] dirs;
        // Массив файлов
        string[] files;
        public FileManagerClass()
        {
            index = 0;
            keyInfo = new ConsoleKeyInfo();
            path = Directory.GetCurrentDirectory();
            GetDirsFiles();
            list = new List<string>();
            list = Merge();
        }
        /// <summary>
        /// Метод, получающий файлы и папки из пути и
        /// генерирующий новый список.
        /// </summary>
        public void GetDirsFiles()
        {
            // Получаем директории из пути
            dirs = Directory.GetDirectories(path);
            // Получаем файлы из пути
            files = Directory.GetFiles(path);
        }
        /// <summary>
        /// Метод, объединяющий два списка в один.
        /// </summary>
        public List<string> Merge()
        {
            List<string> result = new List<string>() { ".." };

            foreach (var d in dirs)
                result.Add(Path.GetFileName(d));

            foreach (var f in files)
                result.Add(Path.GetFileName(f));

            return result;
        }
        /// <summary>
        /// Метод, который инициирует всю работу.
        /// </summary>
        public void Start()
        {
            // Рисуем меню
            WriteMenu();

            // Выполняться пока не нажата клавиша X
            while (keyInfo.Key != ConsoleKey.X)
            {
                // Ждем нажатия клавиши от пользователя
                keyInfo = Console.ReadKey();
                // Если нажата стрелка вниз
                if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    // то если index меньще длины списка
                    if (index + 2 <= list.Count())
                    {
                        // то увеличиваем index на 1
                        index++;
                        // рисуем меню
                        WriteMenu();
                        // Производится переход по пунктам меню - вниз.
                    }
                }
                // Если нажата стрелка вверх
                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    // то если index больше или равно нулю
                    if (index - 1 >= 0)
                    {
                        // то уменьшаем индекс
                        index--;
                        // Рисуем меню
                        WriteMenu();
                        // Производится переход по пунктам меню - вверх
                    }
                }
                // Если нажата клавиша Enter
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    // Если текущий пункт содержит '..',
                    if (list[index] == "..")
                    {
                        // Получаем последний символ разделителя в пути
                        int lastSeparatorIndex = path.LastIndexOf('\\');
                        // Режем путь от начала и до разделителя
                        path = path.Substring(0, lastSeparatorIndex);
                        // Когда возращаемся до диска (например D:), то путь не содержит
                        // разделителя. Поэтому проверяем, если путь не содержит разделителя
                        // добавим его, чтобы можно получить директории и файлы из нее.
                        if (!path.Contains("\\"))
                        {
                            path += '\\';
                        }
                        // Получаем папки и файлы из пути
                        GetDirsFiles();
                        // Формируем новый список с папками и файлами
                        list = Merge();
                        // Обнуляем индекс чтобы курсор стоял в начале меню
                        index = 0;
                        // Рисуем меню
                        WriteMenu();
                    }
                    // иначе, если путь не содержит символ '..',
                    else
                    {
                        // то добавляем в путь текущий пункт меню (директорию или файл)
                        path += "\\" + list[index];
                        // Создаем объект атрибутов файлов и папок по текущему пути path
                        FileAttributes attr = File.GetAttributes(path);
                        // Проверяем, если текущий пункт меню является директорией,
                        if (attr.HasFlag(FileAttributes.Directory))
                        {
                            // то получаем файлы и папки из текущего пути
                            GetDirsFiles();
                            // формируем новый список с файлами и папками из пути
                            list = Merge();
                            // Обнуляем индекс чтобы курсор стоял в начале меню
                            index = 0;
                            // рисуем меню
                            WriteMenu();
                        }
                        // иначе, если текущий пункт не директория (значит файл скорей всего)
                        else
                        {
                            // то получаем индекс последнего разделителя в пути
                            int lastSeparatorIndex = path.LastIndexOf('\\');
                            // Режем путь от начала и до разделителя
                            path = path.Substring(0, lastSeparatorIndex);
                            // при этом, при выборе файла ничего не происходит.
                            // Так и задумано.
                        }
                    }
                }
                // Если нажата клавиша F
                if(keyInfo.Key == ConsoleKey.F)
                {
                    // то запрашиваем ввод у пользователя
                    Console.Write("Введите имя файла (вместе с расширением): ");
                    string filename = Console.ReadLine();
                    // создаем новый файл
                    CreateNewFile(filename);
                    // Обновляем списки с директориями и файлами
                    // чтобы новое меню прорисовалось уже с ними
                    GetDirsFiles();
                    list = Merge();
                }
                // Если нажата клавиша D
                if (keyInfo.Key == ConsoleKey.D)
                {
                    // то запрашиваем ввод у пользователя
                    Console.Write("Введите имя каталога: ");
                    string dirname = Console.ReadLine();
                    // создаем новую директорию
                    CreateNewDirectory(dirname);
                    // Обновляем списки с директориями и файлами
                    // чтобы новое меню прорисовалось уже с ними
                    GetDirsFiles();
                    list = Merge();
                }
                // Если нажата клавиша R
                if (keyInfo.Key == ConsoleKey.R)
                {
                    // то получаем атрибуты файла
                    FileAttributes attr = File.GetAttributes(Path.Combine(path, list[index]));
                    // и проверяем, если файл является директорией,
                    if(attr.HasFlag(FileAttributes.Directory))
                    {
                        // то удаляем директорию
                        RemoveDir(Path.Combine(path, list[index]));
                        // Обновляем списки с директориями и файлами
                        // чтобы новое меню прорисовалось уже с ними
                        GetDirsFiles();
                        list = Merge();
                    }
                    // иначе это какой-то файл
                    else
                    {
                        // Удаляем файл
                        RemoveFile(Path.Combine(path, list[index]));
                        // Обновляем списки с директориями и файлами
                        // чтобы новое меню прорисовалось уже с ними
                        GetDirsFiles();
                        list = Merge();
                    }
                }
                // Если нажата клавиша C
                if(keyInfo.Key == ConsoleKey.C)
                {
                    // то запрашивает ввод у пользователя
                    Console.WriteLine("Введите новый путь для копирования.");
                    Console.WriteLine("Новый путь не должен содержать конечного имени файла.");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"Новый путь -> ");
                    string copyPath = Console.ReadLine();
                    Console.ResetColor();                
                    // Получаем атрибуты файла
                    FileAttributes attr = File.GetAttributes(Path.Combine(path, list[index]));
                    // Если файл является директорией
                    if (attr.HasFlag(FileAttributes.Directory))
                    {
                        // То копируем
                        CopyDirectory(Path.Combine(path, list[index]), Path.Combine(copyPath, list[index]));
                    }
                    else
                    {
                        // Если файл, то копируем файл
                        CopyFile(Path.Combine(path, list[index]), Path.Combine(copyPath, list[index]));
                    }
                }
                // Если нажата клавиша M
                if(keyInfo.Key == ConsoleKey.M)
                {
                    // Запрашиваем ввод у пользователя
                    Console.WriteLine("Введите новый путь для перемещения.");
                    Console.WriteLine("Новый путь не должен содержать конечного имени файла.");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"Новый путь -> ");
                    string copyPath = Console.ReadLine();
                    Console.ResetColor();
                    // Получаем атрибуты файла
                    FileAttributes attr = File.GetAttributes(Path.Combine(path, list[index]));
                    // Если файл является директорией
                    if (attr.HasFlag(FileAttributes.Directory))
                    {
                        // то копируем директорию
                        CopyDirectory(Path.Combine(path, list[index]), Path.Combine(copyPath, list[index]));
                        // и удаляем старую директорию
                        RemoveDir(Path.Combine(path, list[index]));
                        // Обновляем списки с директориями и файлами
                        // чтобы новое меню прорисовалось уже с ними
                        GetDirsFiles();
                        list = Merge();
                    }
                    else
                    {
                        // копируем файл
                        CopyFile(Path.Combine(path, list[index]), Path.Combine(copyPath, list[index]));
                        // удаляем старый файл
                        RemoveFile(Path.Combine(path, list[index]));
                        // Обновляем списки с директориями и файлами
                        // чтобы новое меню прорисовалось уже с ними
                        GetDirsFiles();
                        list = Merge();
                    }
                }
            }
        }
        /// <summary>
        /// Метод, позволяющий скопировать файл в новое местоположение.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="destination"></param>
        public void CopyFile(string sourcePath, string destinationPath)
        {
            // Получаем информацию о файле
            FileInfo fileInfo = new FileInfo(sourcePath);
            try
            {
                // Копируем файл с перезаписью
                fileInfo.CopyTo(destinationPath, true);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Файл {fileInfo.Name} скопирован по пути {destinationPath}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Такого пути не существует.");
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }
        /// <summary>
        /// Метод, позволяющий скопировать каталог в новое местоположение.
        /// </summary>
        /// <param name="dirname"></param>
        /// <param name="destination"></param>
        public void CopyDirectory(string sourceDir, string destinationDir)
        {
            // Получаем информацию о директории
            DirectoryInfo dir = new DirectoryInfo(sourceDir);
            // Получаем массив директорий
            DirectoryInfo[] d = dir.GetDirectories();
            // Создаем директорию
            Directory.CreateDirectory(destinationDir);
            // Получаем файлы из директорий и удаляем сначала их
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }
            // Рекурсивно удаляем директории
            foreach (DirectoryInfo subDir in d)
            {
                string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                CopyDirectory(subDir.FullName, newDestinationDir);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Каталог {dir.Name} скопирован по пути {destinationDir}");
            Console.ResetColor();
        }
        /// <summary>
        /// Метод, позволяющий создать новый текстовый документ.
        /// </summary>
        /// <param name="filename"></param>
        public void CreateNewFile(string filename)
        {            
            // Получаем информацию о файле для удаления
            FileInfo newFile = new FileInfo(Path.Combine(path, filename));
            // Если файл не существует, создать его.
            if (!newFile.Exists)
                newFile.Create();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Файл {filename} создан.");
            Console.ResetColor();
        }
        /// <summary>
        /// Перегруженый метод, позволяющий создать новый текстовый документ.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="destinationPath"></param>
        public void CreateNewFile(string filename, string destinationPath)
        {
            // Получаем информацию о файле для удаления
            FileInfo newFile = new FileInfo(Path.Combine(destinationPath, filename));
            // Если файл не существует, создать его.
            if (!newFile.Exists)
                newFile.Create();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Файл {filename} создан.");
            Console.ResetColor();
        }
        /// <summary>
        /// Метод, позволяющий создать новую директорию.
        /// </summary>
        /// <param name="filename"></param>
        public void CreateNewDirectory(string filename)
        {
            // Получаем информацию о директории
            DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine(path, filename));
            // Если директория не существует, создать ее.
            if (!dirInfo.Exists)
                dirInfo.Create();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Каталог {filename} создан.");
            Console.ResetColor();
        }
        /// <summary>
        ///  Перегруженый метод позволяющий создать новую директорию.
        /// </summary>
        /// /// <param name="filename"></param>
        /// /// <param name="destinationPath"></param>
        public void CreateNewDirectory(string filename, string destinationPath)
        {
            // Получаем информацию о директории
            DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine(destinationPath, filename));
            // Если директория не существует, создать ее.
            if (!dirInfo.Exists)
                dirInfo.Create();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Каталог {filename} создан.");
            Console.ResetColor();
        }
        /// <summary>
        /// Метод, позволяющий удалить директорию.
        /// </summary>
        /// <param name="sourcePath"></param>
        public void RemoveDir(string sourcePath)
        {
            // Получаем информацию об удаляемой директории
            DirectoryInfo dirInfo = new DirectoryInfo(sourcePath);
            // Удаляем директорию рекурсивно
            dirInfo.Delete(true);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Файл {dirInfo.Name} удален.");
            Console.ResetColor();
        }
        /// <summary>
        /// Метод, позволяющий удалить файл.
        /// </summary>
        /// <param name="sourcePath"></param>
        public void RemoveFile(string sourcePath)
        {
            // Получаем иформацию о файле
            FileInfo fileInfo = new FileInfo(sourcePath);
            try
            {
                // Пытаемся удалить файл
                fileInfo.Delete();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Файл {fileInfo.Name} удален.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка удаления. Файл занят другим процессом.");
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }            
        }
        /// <summary>
        /// Метод, отобрадающий информацию о текущей выбранной директории.
        /// </summary>
        public void ShowDirectoryInfo()
        {
            // Если текущий пункт меню не содержит символа ".."
            if (list[index] != "..")
            {
                // то выводим информацию о текущем пункте
                DirectoryInfo dirInfo = new DirectoryInfo(list[index]);
                FileInfo fileInfo = new FileInfo(list[index]);
                Console.WriteLine($"Название: {dirInfo.Name}");
                Console.WriteLine($"Время создания: {dirInfo.CreationTime}");
                Console.WriteLine($"Последнее время изменения: {dirInfo.LastWriteTime}");
                if(dirInfo.Extension.Length != 0)
                {
                    Console.WriteLine($"Расширение файла: {dirInfo.Extension}");
                    Console.WriteLine($"Только для чтения: {fileInfo.IsReadOnly}");
                }
            }
        }
        /// <summary>
        /// Метод, выводящий главное меню на консоль.
        /// </summary>
        public void WriteMenu()
        {
            Console.Clear();
            Console.WriteLine("==============================================================================");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("После создания файла нужно перезапустить программу, " +
                "так как файл занят другим процессом.");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Текущий путь: {path}");
            Console.ResetColor();
            Console.WriteLine($"Позиция: {index}");
            Console.WriteLine("==============================================================================");
            for (int i = 0; i < list.Count(); i++)
            {
                if (index == i)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("->");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(" ");
                }
                Console.WriteLine(list[i]);
            }
            Console.WriteLine("==============================================================================");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("|Копировать - с|Переместить - m|Новый каталог - d|Новый файл - f|Удалить - r |");
            Console.ResetColor();
            Console.WriteLine("==============================================================================");
            ShowDirectoryInfo();
            Console.WriteLine("------------------------------------------------------------------------------");
        }
    }
}
