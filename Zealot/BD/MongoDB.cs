using MongoDB.Bson;
using MongoDB.Driver;

namespace Zealot
{
    public static class MongoDB
    {
        public const int DEFAULT_PORT = 27017;

        public const string HEADER_DB = "header db";

        public struct DefineConnectionData
        {
            public const string SUCCSESS = @"[MongoDB]SuccsessConnectionDefine[Настройки подключения успешно заданы {0}]";
            public const string ERROR = @"[MongoDD]ErrorConnectionDefine[Вы попытались повторно задать настройки подключения \n]" +
                @"Текущие настройки:{0}, Новые настройки:{1}";
        }

        private static string _connection = "";

        public static bool DefineConnection(string value, out string info)
        {
            if (_connection == "")
            {
                info = string.Format(DefineConnectionData.SUCCSESS, value);

                _connection = value;

                return true;
            }

            info = string.Format(DefineConnectionData.ERROR, _connection, value);

            return false;
        }

        public struct ConnectionData
        {
            public const string SUCCSESS = @"[MongoDB]SuccsessDefine[Подключение создано {0}]";
            public const string NOT_DEFINE_CONNECTION = "[MongoDB]NotDefineConnection[Вы не определили поле хранящее аддрес базыданных." +
                "Использйте функцию DefineConnection(string, out string)]";
            public const string DUBLE_START_ERROR = "[MongoDB]DubleStartError[Попытка повторного создания обьекта пуллов]";

            public const string NULL = "[MongoDB]Вы не определили обьект для доступа к пуллу.";
        }

        private static MongoClient _client;

        public static bool StartConnection(out string info)
        {
            if (_connection != "")
            {
                if (_client == null)
                {
                    info = string.Format(ConnectionData.SUCCSESS, _connection);

                    _client = new MongoClient(_connection);

                    try
                    {
                        _client.ListDatabaseNames();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        info = $"[MongoDB|{_connection}]Сервер {_connection} выключен.";

                        return false;
                    }
                }
                else
                {
                    info = ConnectionData.DUBLE_START_ERROR;

                    return false;
                }
            }
            else
            {
                info = ConnectionData.NOT_DEFINE_CONNECTION;

                return false;
            }
        }

        public static bool TryGetDBListName(out List<string> names, out string error)
        {
            error = "";

            if (_client != null)
            {
                using (var c = _client.ListDatabaseNames())
                {
                    names = c.ToList();
                }

                return true;
            }
            else
            {
                names = null;
                error = ConnectionData.NULL;

                return false;
            }
        }

        public static bool ContainsDatabase(string name, out string error)
        {
            error = "";

            if (_client != null)
            {
                try
                {
                    using (var c = _client.ListDatabaseNames())
                    {
                        foreach (string s in c.ToList())
                            if (s == name) return true;
                    }
                }
                catch (TimeoutException ex)
                {
                    error = $"[MongoDB|{_connection}]Невозможно проверить наличие базы данных {name}, так как";
                }
                catch (Exception ex)
                {
                    error = ex.ToString();

                    return false;
                }

                return false;
            }
            else
            {
                error = ConnectionData.NULL;

                return false;
            }
        }

        public static bool TryCreatingDatabase(string name, out string info, string header = "")
        {
            if (name == "")
            {
                info = $"[MongoDB|{_connection}]Невозможно создать базу данных с пустым именем.";

                return false;
            }

            try
            {
                if (_client != null)
                {
                    using (var c = _client.ListDatabaseNames())
                    {
                        foreach (string n in c.ToList())
                        {
                            if (name == n)
                            {
                                info = $"[MongoDB|{_connection}]Вы пытаетесь повторно создать базу данных [{name}], " +
                                    $"но у вас отсутсвует соединение с сервером.";

                                return false;
                            }
                        }
                    }

                    IMongoDatabase db = _client.GetDatabase(name);
                    db.CreateCollection(HEADER_DB);
                    db.GetCollection<string>(HEADER_DB).InsertOne(header);

                    info = $"[MongoDB|{_connection}]Вы создали новую базу данных [{name}].";

                    return true;
                }
                else
                {
                    info = $"[MongoDB|{_connection}]Невозможно создать базу данных [{name}]," +
                        $" вы не определили пулл для работы с БД.";

                    return false;
                }
            }
            catch (TimeoutException e)
            {
                info = $"[MongoDB|{_connection}]Невозможно создать базу данных [{name}] так как, " +
                    $"отсутвует соединение с сервером.";

                return false;
            }
            catch (Exception ex)
            {
                info = ex.ToString();
                return false;
            }
        }
        public static bool TryInsertMany<T>(string dbName, string collectionName,
            out string info, T doc)
        {
            info = "";
            return true;
        }

        public static bool TryInsertOne<T>(string dbName, string collectionName,
            out string info, T doc)
        {
            string information = $"[MongoDB|{_connection}]Невозможно добавить BsonDocument так как, ";

            if (doc == null)
            {
                info = information + "вы передали в он null.";

                return false;
            }

            if (dbName == "")
            {
                info = information + "вы передали в качесве имени базы данных пустую строку.";

                return false;
            }

            if (collectionName == "")
            {
                info = information + "вы передали в качесве имени базы данных пустую строку.";

                return false;
            }

            information = $"[MongoDB|{_connection}]Невозможно добавить BsonDocument, " +
                $"в коллекцию {collectionName} расположеную в базе данных {dbName} так как, ";

            if (_client != null)
            {
                try
                {
                    IMongoDatabase db = _client.GetDatabase(dbName);

                    if (db != null)
                    {
                        IMongoCollection<T> c = db.GetCollection<T>(collectionName);

                        if (c != null)
                        {
                            info = $"[MongoDB{_connection}]Вы успешно добавили документ {typeof(T)} " + 
                                $"в коллекцию {collectionName} базы данных {dbName}";

                            c.InsertOne(doc);

                            return true;
                        }
                        else
                        {
                            info = information + $"так как отсутвует коллекция под таким именем " + 
                                $"хранящяя документ типа {typeof(T)}";

                            return false;
                        }

                    }
                    else
                    {
                        info = information + $"базы данных с именем {dbName} не сущесвует.";

                        return false;
                    }
                }
                catch
                {
                    info = information + "отсутвует подключение к серверу.";

                    return false;
                }
            }
            else
            {
                info = information + "обьект Client равен null.";

                return false;
            }
        }

        public static bool TryGetCollections()
        {
            return true;
        }

        public static bool TryGetDatabase(string name, out IMongoDatabase client, out string info)
        {
            if (name == "")
            {
                client = null;
                info = $"[MongoDB|{_connection}]Неудалось получить базу данных, имя базы данных неможет быть пустым.";

                return false;
            }

            try
            {
                if (_client != null)
                {
                    using (var c = _client.ListDatabaseNames())
                    {
                        foreach (string n in c.ToList())
                        {
                            if (name == n)
                            {
                                client = _client.GetDatabase(name);

                                info = $"[MongoDB|{_connection}]Вы получили обьект для доступа к базе данных [{name}].";

                                return true;
                            }
                        }
                    }

                    client = null;
                    info = $"[MongoDB|{_connection}]Вы пытаетесь получить несуществующую базу данных [{name}]";

                    return false;
                }
                else
                {
                    info = ConnectionData.NULL;
                    client = null;

                    return false;
                }
            }
            catch (Exception ex)
            {
                client = null;

                info = ex.ToString();

                return false;
            }
        }

        public static bool TryRemoveDatabase(string name, out string info)
        {
            if (name == "")
            {
                info = $"[MongoDB|{_connection}]Неудалось удалить базу данных так как было " +
                    $"передано пустое имя.";

                return false;
            }

            if (_client != null)
            {
                if (ContainsDatabase(name, out string error))
                {
                    _client.DropDatabase(name);

                    info = $"[MongoDB|{_connection}]Вы удалили базу данных {name}.";

                    return true;
                }
                else
                {
                    info = $"[MongoDB|{_connection}]Вы попытались удалить базу данных [{name}], "
                        + $"но базы нету.";

                    return false;
                }
            }
            else
            {
                info = ConnectionData.NULL;

                return false;
            }
        }

        public static bool TryRenameCollection<CollectionType>(string databaseName, string collectionName,
            string newCollectionName, out string info)
        {
            if (collectionName == "")
            {
                info = $"[MongoDB|{_connection}]Не удалось переименовать коллекцию, " +
                    $"так как текущее имя колекции не может быть пустым.";

                return false;
            }

            if (newCollectionName == "")
            {
                info = $"[MongoDB|{_connection}]Не удалось переименовать коллекцию, " +
                    $"так как новое имя колекции не может быть пустым.";

                return false;
            }

            if (databaseName == "")
            {
                info = $"[MongoDB|{_connection}]Неудалось переименовать коллекцию {collectionName}->{newCollectionName}, " +
                    $"так как было передано пустое имя для базы данных.";

                return false;
            }

            if (_client != null)
            {
                try
                {
                    IMongoDatabase db = _client.GetDatabase(databaseName);

                    if (db != null)
                    {
                        if (db.GetCollection<CollectionType>(collectionName) != null)
                        {
                            db.RenameCollection(collectionName, newCollectionName);

                            info = $"[MongoDB|{_connection}]В базе данных {databaseName} имя коллекция " +
                                $"{collectionName} было изменено на {newCollectionName}";

                            return true;
                        }
                        else
                        {
                            info = $"[MongoDB|{_connection}]Неудалось переименовать коллекцию, {collectionName}->{newCollectionName}, "
                                + $"так как коллекции с именем {collectionName} не сущесвует.";

                            return false;
                        }
                    }
                    else
                    {
                        info = $"[MongoDB|{_connection}]Неудолось переименовать коллекцию, {collectionName}->{newCollectionName}, " +
                            $"так как базы данных с именем {databaseName} не сущесвует.";

                        return false;
                    }
                }
                catch
                {
                    info = $"[MongoDB|{_connection}]Неудалось переименовать коллекцию {collectionName}->{newCollectionName}, " +
                        $"так как неудалось поключиться к серверу.";

                    return false;
                }
            }
            else
            {
                info = ConnectionData.NULL;

                return false;
            }
        }


        public static bool TryCreatingCollection(string databaseName, string collectionName,
            out string info)
        {
            if (collectionName == "")
            {
                info = $"[MongoDB|{_connection}]Не удалось создать новую коллекцию, " +
                    $" так как имя колекции не может быть пустым.";

                return false;
            }

            if (databaseName == "")
            {
                info = $"[MongoDB|{_connection}]Неудалось создать новую коллекцию {collectionName}, " +
                    $" так как было передано пустое имя для базы данных.";

                return false;
            }

            if (_client != null)
            {
                try
                {
                    IMongoDatabase db = _client.GetDatabase(databaseName);

                    if (db != null)
                    {
                        foreach (string n in db.ListCollectionNames().ToList())
                        {
                            if (collectionName == n)
                            {
                                info = $"[MongoDB|{_connection}]Невозможно создать новую коллекцию, " +
                                    $"коллекция с именем [{n}] уже сущесвует.";

                                return false;
                            }
                        }

                        db.CreateCollection(collectionName);

                        info = $"[MongoDB|{_connection}]Вы создали новую коллекцию [{collectionName}]";

                        return true;
                    }
                    else
                    {
                        info = $"[MongoDB|{_connection}]Неудалось создать коллекцию, " +
                            $"базы данных {databaseName} нету.";

                        return false;
                    }
                }
                catch
                {
                    info = $"[MongoDB|{_connection}]Неудалось создать новую коллекцию {collectionName}," +
                        $"неудалось подключиться к серверу .";

                    return false;
                }
            }
            else
            {
                info = ConnectionData.NULL;

                return false;
            }
        }
    }
}