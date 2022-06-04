using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

/*
Создать консольный фаловый менеджер, у которого будет меню
позволяющее:
- просматривать каталоги и их содержимое
- выводить информацию о файлах
- копировать каталоги
- перемещать каталоги
- удалять каталоги
- создавать файлы в каталогах
- удалять файлы в каталогах
- перемащать файлы
- копировать файлы
Так же реализовать все через аргументы командной строки.
*/

namespace ConsoleFileManager
{
    internal class Program
    {

        /*
         * В программе создан интерфейс IFileIO.
         * Класс DoWork является просто промежуточным классом.
         * Он принимает переменную интерфейса. Это я сделал на случай,
         * если мне вдруг захочется написать другую реализацию класса
         * FileManagerClass. Это позволит в дальнейшем быстрей переключаться
         * между разными реализациями. Получается я просто могу в методе
         * Main в конструктор класса DoWork подсунуть другой класс, который
         * как-то по другому производит операции над файлами.
         */
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                DoWork doWork = new DoWork(new FileManagerClass());
                doWork.Start();
            }
            else
            {
                DoWork doWork = new DoWork(new ProcessCommandLine(args));
            }
        }
    }
}
