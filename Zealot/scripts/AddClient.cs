using System.Text.RegularExpressions;
using MongoDB.Bson;
using static Zealot.manager.ClientMain;

namespace Zealot.script
{
    /// <summary>
    /// Скрипт для добовления клинтов.
    /// </summary> <summary>
    public static class AddClient
    {
        public static void DeleteCollection()
        {
            if (MongoDB.TryRemoveDatabase(manager.Clients.DB.Client.NAME, out string info))
            {
                System.Console.WriteLine(info);
            }
            else 
            {
                System.Console.WriteLine(info);
            }
        }

        public static void Add(AddNewClient value)
        {
            AddNewClientResult result = new();

            {
                {
                    {
                        string login = value.Login;
                        {
                            if (login.Length < 4 || login.Length > 16)
                            {
                                result.LoginResult = "Логин должен иметь длину от 4 до 16 символов.";
                            }
                        }

                        string password = value.Password;
                        {
                            if (password.Length < 8 || password.Length > 32)
                            {
                                result.PasswordResult = "Пароль должен иметь длину от 8 до 32 символов.";
                            }
                        }

                        string email = value.Email;
                        {

                            if (Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$") == false)
                            {
                                result.LoginResult = "Пароль должен иметь длину от 8 до 32 символов.";
                            }
                        }

                        string fullName = value.FullName;
                        {
                            if (fullName.Length < 4)
                            {
                                result.FullNameResult = "Полное имя клиента должен иметь длину от 8 до 32 символов.";
                            }
                        }

                        // Если введеные данные не верны вернем ответ.
                        if (result.LoginResult != "" || result.PasswordResult != "" ||
                            result.EmailResult != "" || result.FullNameResult != "")
                        {

                            return;
                        }
                        // Иначе продолжим запить в базу данных.


                        if (MongoDB.ContainsDatabase(manager.Clients.DB.Client.NAME, out string containsDBerror))
                        {
                            System.Console.WriteLine($"База данныx {manager.Clients.DB.Client.NAME} уже создана.");
                        }
                        else
                        {
                            if (containsDBerror != "")
                            {
                                System.Console.WriteLine($"База данныx {manager.Clients.DB.Client.NAME} уже создана.");
                            }
                            else
                            {
                                System.Console.WriteLine($"Создаем базу данных {manager.Clients.DB.Client.NAME}.");

                                if (MongoDB.TryCreatingDatabase(manager.Clients.DB.Client.NAME, out string infoR))
                                {
                                    System.Console.WriteLine(infoR);
                                }
                                else
                                {
                                    result.OtherResult = $"Неудалось добавить нового клиeнта[Login:{value.Login}, Password:*********, Email:{value.Email}" +
                                        $"FullName:{value.FullName}, OrganizationName:{value.OrganizationName}].{MongoDB.Error.NOT_CREATING_DB}";

                                    System.Console.WriteLine(infoR);

                                    return;
                                }
                            }
                        }

                        // Проверяем наличие коллекции.
                        if (MongoDB.ContainsCollection<BsonDocument>(manager.Clients.DB.Client.NAME, manager.Clients.DB.Client.Collection.NAME,
                            out string error))
                        {
                            System.Console.WriteLine($"Коллекция [{manager.Clients.DB.Client.Collection.NAME}] в базе данных " +
                                $" [{manager.Clients.DB.Client.NAME}] уже создана.");
                        }
                        else
                        {
                            // Коллекции нету, создадим ее.
                            if (error == "")
                            {
                                if (MongoDB.TryCreatingCollection(manager.Clients.DB.Client.NAME, manager.Clients.DB.Client.Collection.NAME,
                                    out string infoI))
                                {
                                    System.Console.WriteLine(infoI);
                                }
                                else
                                {
                                    result.OtherResult = $"Неудалось добавить нового клиeнта[Login:{value.Login}, Password:*********, Email:{value.Email}" +
                                        $"FullName:{value.FullName}, OrganizationName:{value.OrganizationName}].{MongoDB.Error.NOT_CREATING_DB}";

                                    System.Console.WriteLine(infoI);

                                    return;
                                }
                            }
                            else
                            {
                                result.OtherResult = $"Неудалось добавить нового клиeнта[Login:{value.Login}, Password:*********, Email:{value.Email}" +
                                    $"FullName:{value.FullName}, OrganizationName:{value.OrganizationName}] {MongoDB.Error.ERROR_FROM_DB}.";

                                System.Console.WriteLine(error);

                                return;
                            }
                        }

                        // Проверяем наличие документа хранящего адреса и диопазоны адресов.
                        if (MongoDB.TryFind(manager.Clients.DB.Client.NAME, manager.Clients.DB.Client.NAME, out string findInfo,
                            out List<BsonDocument> clients))
                        {
                            System.Console.WriteLine(findInfo);

                            if (clients != null)
                            {
                                try
                                {
                                    foreach (BsonDocument doc in clients)
                                    {
                                        if (doc[manager.Clients.DB.Client.Collection.Key.LOGIN] == login)
                                        {
                                            result.LoginResult = $"Клиент с логином {login} уже добавлен.";

                                            System.Console.WriteLine($"Клиент с логином {login} уже добавлен.");

                                            return;
                                        }

                                        if (doc[manager.Clients.DB.Client.Collection.Key.EMAIL] == email)
                                        {
                                            result.EmailResult = $"Клиент с логином {email} уже добавлен.";

                                            System.Console.WriteLine($"Клиент с логином {email} уже добавлен.");

                                            return;
                                        }

                                        if (doc[manager.Clients.DB.Client.Collection.Key.FULL_NAME] == fullName)
                                        {
                                            result.FullNameResult = $"Клиент с именем {fullName} уже добавлен.";

                                            System.Console.WriteLine($"Клиент с именем {fullName} уже добавлен.");

                                            return;
                                        }

                                        // Если введеные данные не верны вернем ответ.
                                        if (result.LoginResult != "" || result.PasswordResult != "" ||
                                            result.EmailResult != "" || result.FullNameResult != "")
                                        {
                                            return;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    result.OtherResult = $"Неудалось добавить нового клиeнта[Login:{value.Login}, Password:*********, Email:{value.Email}" +
                                        $"FullName:{value.FullName}, OrganizationName:{value.OrganizationName}].{MongoDB.Error.VALUE_EXCEPTION}";

                                    System.Console.WriteLine(ex.ToString());

                                    return;
                                }
                            }
                            else
                            {
                                result.OtherResult = $"Неудалось добавить нового клиeнта[Login:{value.Login}, Password:*********, Email:{value.Email}" +
                                    $"FullName:{value.FullName}, OrganizationName:{value.OrganizationName}].{MongoDB.Error.VALUE_IS_NULL}";

                                System.Console.WriteLine(findInfo);

                                return;
                            }
                        }

                        string currentDate = DateTime.Now.ToString();

                        // Затем данное значение передадим в базу данных.
                        if (MongoDB.TryInsertOne(manager.Clients.DB.Client.NAME, manager.Clients.DB.Client.Collection.NAME,
                        out string info, new BsonDocument()
                        {
                            { manager.Clients.DB.Client.Collection.Key.LOGIN, login},
                            { manager.Clients.DB.Client.Collection.Key.PASSWORD, password},
                            { manager.Clients.DB.Client.Collection.Key.EMAIL, email},
                            { manager.Clients.DB.Client.Collection.Key.FULL_NAME, fullName},
                            { manager.Clients.DB.Client.Collection.Key.ORGANIZATION_NAME, value.OrganizationName},
                            { manager.Clients.DB.Client.Collection.Key.IS_RUNNING, true},
                            { manager.Clients.DB.Client.Collection.Key.ACCESS_RIGHTS, value.AccessRight},
                            { manager.Clients.DB.Client.Collection.Key.ASICS_COUNT, 0},
                            { manager.Clients.DB.Client.Collection.Key.CREATING_DATE, currentDate},
                            { manager.Clients.DB.Client.Collection.Key.WORK_UNTIL_WHAT_DATE, currentDate},
                        }))
                        {
                            result.IsSuccess = true;

                            result.LoginResult = "Success";
                            result.PasswordResult = "Success";
                            result.EmailResult = "Success";
                            result.FullNameResult = "Success";
                            result.OrganizationNameResult = "Success";
                            result.AccessRightResult = "Success";

                            ClientData data = new(value);
                            {
                                data.AddClientDate = currentDate;
                                data.WorkUntilWantDate = currentDate;
                            }

                            System.Console.WriteLine(
                                $"Add new client:\n" +
                            $"{manager.Clients.DB.Client.Collection.Key.LOGIN}:{login}\n" +
                            $"{manager.Clients.DB.Client.Collection.Key.PASSWORD}:{password}\n" +
                            $"{manager.Clients.DB.Client.Collection.Key.EMAIL}:{email}\n" +
                            $"{manager.Clients.DB.Client.Collection.Key.FULL_NAME}:{fullName}\n" +
                            $"{manager.Clients.DB.Client.Collection.Key.ORGANIZATION_NAME}:{value.OrganizationName}\n" +
                            $"{manager.Clients.DB.Client.Collection.Key.IS_RUNNING}:{true}\n" +
                            $"{manager.Clients.DB.Client.Collection.Key.ACCESS_RIGHTS}:{value.AccessRight}\n" +
                            $"{manager.Clients.DB.Client.Collection.Key.ASICS_COUNT}:{0}\n" +
                            $"{manager.Clients.DB.Client.Collection.Key.CREATING_DATE}:{currentDate}\n" +
                            $"{manager.Clients.DB.Client.Collection.Key.WORK_UNTIL_WHAT_DATE}:{currentDate}\n"
                            );
                        }
                        else
                        {
                            result.OtherResult = $"Неудалось добавить нового клиeнта[Login:{value.Login}, Password:*********, Email:{value.Email}" +
                                $"FullName:{value.FullName}, OrganizationName:{value.OrganizationName}].{MongoDB.Error.VALUE_IS_NULL}";

                            System.Console.WriteLine(findInfo);

                            return;
                        }
                    }
                }
            }
        }
    }
}