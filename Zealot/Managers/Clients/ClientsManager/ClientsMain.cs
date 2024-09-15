using System.Text.RegularExpressions;
using Butterfly;
using MongoDB.Bson;

namespace Zealot.manager
{
    public abstract class ClientsMain : Controller
    {
        protected Devices DevicesManager;

        protected readonly Dictionary<string, ClientMain> Clients = new();

        protected void EAddNewClient(AddNewClient value, Clients.IClientConnect client, IReturn<AddNewClientResult> @return)
        {
            AddNewClientResult result = new();

            lock (StateInformation.Locker)
            {
                if (StateInformation.IsStart && !StateInformation.IsDestroy)
                {
                    if (client.IsAdmin())
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
                        if (result.IsSuccess == false)
                        {
                            @return.To(result);

                            return;
                        }
                        // Иначе продолжим запить в базу данных.


                        if (MongoDB.ContainsDatabase(manager.Clients.DB.Client.NAME, out string containsDBerror))
                        {
                            Logger.I.To(this, $"База данныx {manager.Clients.DB.Client.NAME} уже создана.");
                        }
                        else
                        {
                            if (containsDBerror != "")
                            {
                                Logger.I.To(this, $"База данныx {manager.Clients.DB.Client.NAME} уже создана.");
                            }
                            else
                            {
                                Logger.I.To(this, $"Создаем базу данных {manager.Clients.DB.Client.NAME}.");

                                if (MongoDB.TryCreatingDatabase(manager.Clients.DB.Client.NAME, out string infoR))
                                {
                                    Logger.I.To(this, infoR);
                                }
                                else
                                {
                                    result.OtherResult = $"Неудалось добавить нового клиeнта[Login:{value.Login}, Password:*********, Email:{value.Email}" +
                                        $"FullName:{value.FullName}, OrganizationName:{value.OrganizationName}].{MongoDB.Error.NOT_CREATING_DB}";

                                    @return.To(result);

                                    Logger.S_E.To(this, infoR);

                                    destroy();

                                    return;
                                }
                            }
                        }

                        // Проверяем наличие коллекции.
                        if (MongoDB.ContainsCollection<BsonDocument>(manager.Clients.DB.Client.NAME, manager.Clients.DB.Client.Collection.NAME,
                            out string error))
                        {
                            Logger.I.To(this, $"Коллекция [{manager.Clients.DB.Client.Collection.NAME}] в базе данных " +
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
                                    Logger.I.To(this, infoI);
                                }
                                else
                                {
                                    result.OtherResult = $"Неудалось добавить нового клиeнта[Login:{value.Login}, Password:*********, Email:{value.Email}" +
                                        $"FullName:{value.FullName}, OrganizationName:{value.OrganizationName}].{MongoDB.Error.NOT_CREATING_DB}";

                                    @return.To(result);

                                    Logger.S_E.To(this, infoI);

                                    destroy();

                                    return;
                                }
                            }
                            else
                            {
                                result.OtherResult = $"Неудалось добавить нового клиeнта[Login:{value.Login}, Password:*********, Email:{value.Email}" +
                                    $"FullName:{value.FullName}, OrganizationName:{value.OrganizationName}] {MongoDB.Error.ERROR_FROM_DB}.";

                                @return.To(result);

                                Logger.S_E.To(this, error);

                                destroy();

                                return;
                            }
                        }

                        // Проверяем наличие документа хранящего адреса и диопазоны адресов.
                        if (MongoDB.TryFind(manager.Clients.DB.Client.NAME, manager.Clients.DB.Client.NAME, out string findInfo,
                            out List<BsonDocument> clients))
                        {
                            Logger.I.To(this, findInfo);

                            if (clients != null)
                            {
                                try
                                {
                                    foreach (BsonDocument doc in clients)
                                    {
                                        if (doc[manager.Clients.DB.Client.Collection.Key.LOGIN] == login)
                                        {
                                            result.LoginResult = $"Клиент с логином {login} уже добавлен.";

                                            Logger.W.To(this, $"Клиент с логином {login} уже добавлен.");

                                            return;
                                        }

                                        if (doc[manager.Clients.DB.Client.Collection.Key.FULL_NAME] == fullName)
                                        {
                                            result.FullNameResult = $"Клиент с именем {fullName} уже добавлен.";

                                            Logger.W.To(this, $"Клиент с именем {fullName} уже добавлен.");

                                            return;
                                        }

                                        if (result.IsSuccess == false)
                                        {
                                            @return.To(result);

                                            return;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    result.OtherResult = $"Неудалось добавить нового клиeнта[Login:{value.Login}, Password:*********, Email:{value.Email}" +
                                        $"FullName:{value.FullName}, OrganizationName:{value.OrganizationName}].{MongoDB.Error.VALUE_EXCEPTION}";

                                    @return.To(result);

                                    Logger.S_E.To(this, ex.ToString());

                                    destroy();

                                    return;
                                }
                            }
                            else
                            {
                                result.OtherResult = $"Неудалось добавить нового клиeнта[Login:{value.Login}, Password:*********, Email:{value.Email}" +
                                    $"FullName:{value.FullName}, OrganizationName:{value.OrganizationName}].{MongoDB.Error.VALUE_IS_NULL}";

                                @return.To(result);

                                Logger.S_E.To(this, findInfo);

                                destroy();

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
                            { manager.Clients.DB.Client.Collection.Key.ADD_DATE, currentDate},
                            { manager.Clients.DB.Client.Collection.Key.WORK_UNTIL_WHAT_DATE, currentDate},
                        }))
                        {
                            result.IsSuccess = true;

                            result.LoginResult = "Success";
                            result.PasswordResult = "Success";
                            result.EmailResult = "Success";
                            result.FullNameResult = "Success";
                            result.OrganizationNameResult= "Success";
                            result.AccessRight = "Success";

                            Logger.I.To(this,
                                $"Add new client:\n" +
                            $"{manager.Clients.DB.Client.Collection.Key.LOGIN}:{login}\n" +
                            $"{manager.Clients.DB.Client.Collection.Key.PASSWORD}:{password}\n" +
                            $"{manager.Clients.DB.Client.Collection.Key.EMAIL}:{email}\n" +
                            $"{manager.Clients.DB.Client.Collection.Key.FULL_NAME}:{fullName}\n" +
                            $"{manager.Clients.DB.Client.Collection.Key.ORGANIZATION_NAME}:{value.OrganizationName}\n" +
                            $"{manager.Clients.DB.Client.Collection.Key.IS_RUNNING}:{true}\n" +
                            $"{manager.Clients.DB.Client.Collection.Key.ACCESS_RIGHTS}:{value.AccessRight}\n" +
                            $"{manager.Clients.DB.Client.Collection.Key.ASICS_COUNT}:{0}\n" +
                            $"{manager.Clients.DB.Client.Collection.Key.ADD_DATE}:{currentDate}\n" +
                            $"{manager.Clients.DB.Client.Collection.Key.WORK_UNTIL_WHAT_DATE}:{currentDate}\n"
                            );
                        }
                        else
                        {
                            result.OtherResult = $"Неудалось добавить нового клиeнта[Login:{value.Login}, Password:*********, Email:{value.Email}" +
                                $"FullName:{value.FullName}, OrganizationName:{value.OrganizationName}].{MongoDB.Error.VALUE_IS_NULL}";

                            @return.To(result);

                            Logger.S_E.To(this, findInfo);

                            destroy();

                            return;
                        }
                    }
                    else
                    {
                        result.OtherResult = "Добавить нового клиента может только администратор.";

                        @return.To(result);
                    }
                }
                else
                {
                    if (StateInformation.IsDestroy)
                    {
                        result.OtherResult = $"Неудалось добавить нового клиeнта[Login:{value.Login}, Password:{value.Password}, Email:{value.Email}" +
                            $"FullName:{value.FullName}, OrganizationName:{value.OrganizationName}] так как ClientsManager приступил к своему уничтожению.";
                    }
                    else if (StateInformation.IsStart == false)
                    {
                        result.OtherResult = $"Неудалось добавить нового клиeнта[Login:{value.Login}, Password:{value.Password}, Email:{value.Email}" +
                            $"FullName:{value.FullName}, OrganizationName:{value.OrganizationName}] так как ClientsManager еще не запустился." +
                            $"CurrentState:{StateInformation.CurrentState}";
                    }
                    else
                    {
                        result.OtherResult = $"Неудалось добавить нового клиeнта[Login:{value.Login}, Password:{value.Password}, Email:{value.Email}" +
                            $"FullName:{value.FullName}, OrganizationName:{value.OrganizationName}] так как текущее состояние состояние ClientManager " +
                            $"CurrentState:{StateInformation.CurrentState}";
                    }

                    @return.To(result);
                }
            }
        }
    }
}