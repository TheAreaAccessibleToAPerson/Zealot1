namespace Butterfly
{
    public struct __ 
    {
        public const string AND = "_AND_";
    }

    public class Hellper
    {
        public static string GetName(System.Reflection.MethodInfo pMethodInfo)
        {
            string result = pMethodInfo.Name;

            System.Reflection.ParameterInfo[] parametrs = pMethodInfo.GetParameters();

            if (parametrs.Length > 0)
            {
                result += "(";

                for (int i = 0; i < parametrs.Length; i++)
                {
                    result += parametrs[i].ParameterType;
                    result += " " + parametrs[i].Name;

                    if (i + 1 < parametrs.Length) result += ", ";
                }

                result += ")";
            }

            return result;
        }

        /// <summary>
        /// Обьеденяем массивы.
        /// </summary>
        /// <param name="pArray1"></param>
        /// <param name="pArray2"></param>
        /// <returns></returns>
        public static ulong[] ConcatArray(ulong[] pArray1, ulong[] pArray2)
        {
            ulong[] result = new ulong[pArray1.Length + pArray2.Length];

            pArray1.CopyTo(result, 0);
            pArray2.CopyTo(result, pArray1.Length);

            return result;
        }

        public static ValueType[] ConcatArray<ValueType>(ValueType[] pArray1, ValueType[] pArray2)
        {
            ValueType[] result = new ValueType[pArray1.Length + pArray2.Length];

            pArray1.CopyTo(result, 0);
            pArray2.CopyTo(result, pArray1.Length);

            return result;
        }

        public static ulong[] ExpendArray(ulong[] pArray1, ulong pValue)
        {
            ulong[] result = new ulong[pArray1.Length + 1];

            pArray1.CopyTo(result, 0);
            result[pArray1.Length] = pValue;

            return result;
        }

        public static ValueType[] ExpendArray<ValueType>(ValueType[] _arrays, ValueType _value)
        {
            ValueType[] result = new ValueType[_arrays.Length + 1];

            _arrays.CopyTo(result, 0);
            result[_arrays.Length] = _value;

            return result;
        }

        public static int ExpendArray<ValueType>(ref ValueType[] _arrays, ValueType _value)
        {
            ValueType[] result = new ValueType[_arrays.Length + 1];

            _arrays.CopyTo(result, 0);
            result[_arrays.Length] = _value;

            _arrays = result;

            return result.Length;
        }

        /// <summary>
        /// Получает обьект реализующий идеи доступа к нему
        /// и возращает ссылку.
        /// </summary>
        /// <param name="input">Ссылка на доступ к текущему обьекту.</param>
        /// <param name="obj">Обьект доступ к которму нужно получить.</param>
        /// <typeparam name="ObjectType">Тип обьекта к которму необходимо получить доступ.</typeparam>
        /// <typeparam name="InputType">Тип интерфейса который описывает способ получения доступа.</typeparam>
        /// <returns></returns>
        public static ObjectType GetInput<ObjectType, InputType>
            (ref InputType input, ObjectType obj)
                where ObjectType : InputType
        {
            input = obj;

            return obj;
        }

        /// <summary>
        /// Принимает на вход два обьекта и ссылку на входные данные для первого обьекта.
        /// Первый обьект реализует интерфейс IInputConnected который описывает способ подключения
        /// к обьекту реализующего интерфейс IInputConnect описывающий точку подключения.
        /// <param name="input">Способ передачи данных в первый обьект.</param>
        /// <param name="object1">Обьект реализующий подключение.</param>
        /// <param name="object2">Обьект предаставляющий точку для подключения.</param>
        /// <typeparam name="ObjectType1">Тип обьекта реализующий интефейс IInputConnected.</typeparam>
        /// <typeparam name="InputType">Тип передачи данных описаный в ObjectType1.</typeparam>
        /// <typeparam name="ObjectType2">Тип обьекта реализующий интерфейс IInputConnect.</typeparam>
        public static ObjectType1 SetConnected<ObjectType1, InputType, ObjectType2>
            (ref InputType input, ObjectType1 object1, ObjectType2 object2)
                where ObjectType1 : system.objects.main.IInputConnected, InputType
                where ObjectType2 : system.objects.main.IInputConnect
        {
            input = object1;

            object1.SetConnected(object2.GetConnect());

            return object1;
        }

        public static void Connected<ConnectedType>(object inputConnect, ref ConnectedType connected,
            System.Type connectedObjectType)
        {
            if (inputConnect is ConnectedType inputConnectReduse)
            {
                connected = inputConnectReduse;
            }
            else 
            {
                 System.Console.ForegroundColor = System.ConsoleColor.Red;
                 System.Console.WriteLine($"Вы не можете соединить обьект {connectedObjectType.FullName} c"+
                    $"c объектом {inputConnect.GetType().FullName}");

                System.Threading.Thread.Sleep(100000);
            }
        }

        public static ValueType[][] ExpendRange<ValueType>(ValueType[][] pArray, int pArrayLength)
        {
            ValueType[][] result = new ValueType[pArray.Length + 1][];

            for (int i = 0; i < pArray.Length; i++)
            {
                result[i] = pArray[i];
            }

            result[pArray.Length] = new ValueType[pArrayLength];

            return result;
        }

        public static bool GetSystemMethod(string methodName, global::System.Type type, 
            out global::System.Reflection.MethodInfo systemMethod)
        {
            systemMethod = type.GetMethod(methodName, System.Reflection.BindingFlags.Instance | 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);

            if (systemMethod == null)
                return false;
            else
                return true;
        }

        public static string FileRead(string path)
        {
            return "";
            /*
            using (FileStream fstream = File.OpenRead(Directory.GetCurrentDirectory() + "/" + path))
            {
                // выделяем массив для считывания данных из файла
                byte[] buffer = new byte[fstream.Length];
                // считываем данные
                fstream.Read(buffer, 0, buffer.Length);
                // декодируем байты в строку
                string textFromFile = System.Text.Encoding.Default.GetString(buffer);

                return textFromFile;
            }
            */
        }
    }
}
