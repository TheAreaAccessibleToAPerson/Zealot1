using System.Reflection.Metadata.Ecma335;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Zealot
{
    public static class MongoDB
    {
        public struct Error 
        {
            private const string _ = "CODE_ERROR:";

            /// <summary>
            /// При работе с БД была возвращена ошибка.
            /// </summary> 
            public const string ERROR_FROM_DB = _ + "x1000";

            /// <summary>
            /// Неудалось получить доступ к базе данных.
            /// </summary>
            public const string NOT_CREATING_DB = _ + "x1001";

            /// <summary>
            /// Неудалось получить доступ к базе данных.
            /// </summary>
            public const string NOT_CREATING_COLLECTION = _ + "x1002";

            /// <summary>
            /// Из бызы данных был получен обьект равный null.
            /// </summary>
            public const string VALUE_IS_NULL = _ + "x1003";

            /// <summary>
            /// Исключение во время обработки полученых из базы данных обьекта.
            /// </summary>
            public const string VALUE_EXCEPTION  = _ + "x1004";
        }

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

        public static MongoClient Client { set; get; }

        public static bool StartConnection(out string info)
        {
            if (_connection != "")
            {
                if (Client == null)
                {
                    info = string.Format(ConnectionData.SUCCSESS, _connection);

                    Client = new MongoClient(_connection);

                    try
                    {
                        Client.ListDatabaseNames();

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

            if (Client != null)
            {
                using (var c = Client.ListDatabaseNames())
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

        public static bool ContainsCollection<T>(string dbName, string collectionName,
            out string error)
        {
            if (dbName == "")
            {
                error = $"[MongoDB|{_connection}]Вы не можете проверить наличие коллекции [{collectionName}] + " +
                    $", так как передали пустое имя базы данных.";

                return false;
            }

            if (collectionName == "")
            {
                error = $"[MongoDB|{_connection}]Вы не можете проверить наличие коллекции " +
                    $" в базе данных [{dbName}], так как передали пустое имя коллекции.";

                return false;
            }

            if (Client != null)
            {
                try
                {
                    IMongoDatabase db = Client.GetDatabase(dbName);

                    if (db != null)
                    {
                        IMongoCollection<T> c = db.GetCollection<T>(collectionName);

                        error = "";

                        if (c != null)
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        error = $"[MongoDB|{_connection}]Вы не можете проверить наличие коллекции[{collectionName}] " +
                            $" в базе данных [{dbName}], так как такой базы данных не существует.";

                        return false;
                    }
                }
                catch (TimeoutException)
                {
                    error = $"[MongoDB|{_connection}]Вы не можете проверить наличие коллекции[{collectionName}] " +
                        $" в базе данных [{dbName}], так как отсутствует соединение с сервером.";

                    return false;
                }
            }
            else
            {
                error = ConnectionData.NULL;
                return false;
            }
        }

        public static bool ContainsDatabase(string name, out string error)
        {
            error = "";

            if (Client != null)
            {
                try
                {
                    var c = Client.GetDatabase(name);

                    if (c != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                    /*
                    using (var c = _client.ListDatabaseNames())
                    {
                        foreach (string s in c.ToList())
                            if (s == name) return true;
                    }
                    */
                }
                catch (TimeoutException)
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
                if (Client != null)
                {
                    using (var c = Client.ListDatabaseNames())
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

                    IMongoDatabase db = Client.GetDatabase(name);
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

            if (Client != null)
            {
                try
                {
                    IMongoDatabase db = Client.GetDatabase(dbName);

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
                if (Client != null)
                {
                    using (var c = Client.ListDatabaseNames())
                    {
                        foreach (string n in c.ToList())
                        {
                            if (name == n)
                            {
                                client = Client.GetDatabase(name);

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

            if (Client != null)
            {
                if (ContainsDatabase(name, out string error))
                {
                    Client.DropDatabase(name);

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

        public static bool TryFindFirst(string databaseName, string collectionName,
            out string info, string key, out BsonDocument doc, out int count)
        {
            count = -1;

            string information = $"[MongoDB|{_connection}]Не удалось получить документ, так как";

            if (collectionName == "")
            {
                info = information + $"текущее имя колекции не может быть пустым.";

                doc = default;

                return false;
            }

            if (databaseName == "")
            {
                info = information + $"было передано пустое имя для базы данных.";

                doc = default;

                return false;
            }

            if (Client != null)
            {
                try
                {
                    IMongoDatabase db = Client.GetDatabase(databaseName);

                    if (db != null)
                    {
                        IMongoCollection<BsonDocument> c = db.GetCollection<BsonDocument>(collectionName);

                        if (c != null)
                        {
                            List<BsonDocument> buffer = c.Find(key).ToList();
                            count = buffer.Count;
                            doc = buffer[0];

                            info = $"[MongoDB|{_connection}]Вы получили BsonDocument";

                            return true;
                        }
                        else
                        {
                            info = information +
                                $" коллекции {collectionName} нету в базе данных {databaseName}";

                            doc = default;

                            return false;
                        }
                    }
                    else
                    {
                        info = information +
                            $"базы данных с именем {databaseName} не сущесвует.";

                        doc = default;

                        return false;
                    }
                }
                catch (TimeoutException)
                {
                    info = information + "отсутвует подключение к серверу.";
                    doc = default;
                    return false;
                }
                catch (Exception ex)
                {
                    info = information + ex.ToString();
                    doc = default;
                    return false;
                }
            }
            else
            {
                info = information + ConnectionData.NULL;
                doc = default;
                return false;
            }
        }

        public static bool TryDeleteOne(string databaseName, string collectionName,
            string key, string value, out string info)
        {
            string information = $"[MongoDB|{_connection}]Не удалось удалить документ, так как ";

            if (collectionName == "")
            {
                info = information + $"текущее имя колекции не может быть пустым.";

                return false;
            }

            if (databaseName == "")
            {
                info = information + $"было передано пустое имя для базы данных.";

                return false;
            }

            if (key == "")
            {
                info = information + $" ключь не может быть пустым.";

                return false;
            }

            if (Client != null)
            {
                try
                {
                    IMongoDatabase db = Client.GetDatabase(databaseName);

                    if (db != null)
                    {
                        IMongoCollection<BsonDocument> collection
                            = db.GetCollection<BsonDocument>(collectionName);

                        if (collection != null)
                        {
                            long count = collection.DeleteOne(new BsonDocument(key, value)).DeletedCount;

                            info = $"Вы удалили {count} документ c ключом {key} и значением {value}";

                            return true;
                        }
                        else
                        {
                            info = information + $" коллекции с именем [{collectionName}] " +
                                $" нету в базе данных [{databaseName}]";

                            return false;
                        }
                    }
                    else
                    {
                        info = information + $"базы данных с именем {databaseName} не сущесвует.";
                        return false;
                    }
                }
                catch (TimeoutException)
                {
                    info = information + "отсутвует подключение к серверу.";
                    return false;
                }
                catch (Exception ex)
                {
                    info = information + ex.ToString();
                    return false;
                }
            }
            else
            {
                info = information + ConnectionData.NULL;

                return false;
            }
        }

        public static bool TryFind(string databaseName, string collectionName,
            out string info, out List<BsonDocument> doc)
        {
            string information = $"[MongoDB|{_connection}]Не удалось получить список документов, так как";

            if (collectionName == "")
            {
                info = information + $"текущее имя колекции не может быть пустым.";

                doc = default;

                return false;
            }

            if (databaseName == "")
            {
                info = information + $"было передано пустое имя для базы данных.";

                doc = default;

                return false;
            }

            if (Client != null)
            {
                try
                {
                    IMongoDatabase db = Client.GetDatabase(databaseName);

                    if (db != null)
                    {
                        IMongoCollection<BsonDocument> c = db.GetCollection<BsonDocument>(collectionName);

                        if (c != null)
                        {
                            doc = c.Find(new BsonDocument()).ToList();

                            if (doc != null)
                            {
                                info = $"[MongoDB|{_connection}]Вы получили BsonDocument в количесве {doc.Count}";
                            }
                            else
                            {
                                info = $"[MongoDB|{_connection}]У вас нету неодного нокумента.";
                            }

                            return true;
                        }
                        else
                        {
                            info = information +
                                $" коллекции {collectionName} нету в базе данных {databaseName}";

                            doc = default;

                            return false;
                        }
                    }
                    else
                    {
                        info = information +
                            $"базы данных с именем {databaseName} не сущесвует.";

                        doc = default;

                        return false;
                    }
                }
                catch (TimeoutException)
                {
                    info = information + "отсутвует подключение к серверу.";
                    doc = default;
                    return false;
                }
                catch (Exception ex)
                {
                    info = information + ex.ToString();
                    doc = default;
                    return false;
                }
            }
            else
            {
                info = information + ConnectionData.NULL;
                doc = default;
                return false;
            }
        }

        /*
        public static bool TryFind(string databaseName, string collectionName,
            out string info, out List<BsonDocument> doc)
        {
            string information = $"[MongoDB|{_connection}]Не удалось получить список документ, так как";

            if (collectionName == "")
            {
                info = information + $"текущее имя колекции не может быть пустым.";

                doc = default;

                return false;
            }

            if (databaseName == "")
            {
                info = information + $"было передано пустое имя для базы данных.";

                doc = default;

                return false;
            }

            if (Client != null)
            {
                try
                {
                    IMongoDatabase db = Client.GetDatabase(databaseName);

                    if (db != null)
                    {
                        IMongoCollection<BsonDocument> c = db.GetCollection<BsonDocument>(collectionName);

                        if (c != null)
                        {
                            doc = c.Find(new BsonDocument()).ToList();

                            if (doc != null)
                            {
                                info = $"[MongoDB|{_connection}]Вы получили BsonDocument в количесве {doc.Count}";
                            }
                            else
                            {
                                info = $"[MongoDB|{_connection}]У вас нету неодного нокумента.";
                            }

                            return true;
                        }
                        else
                        {
                            info = information +
                                $" коллекции {collectionName} нету в базе данных {databaseName}";

                            doc = default;

                            return false;
                        }
                    }
                    else
                    {
                        info = information +
                            $"базы данных с именем {databaseName} не сущесвует.";

                        doc = default;

                        return false;
                    }
                }
                catch (TimeoutException)
                {
                    info = information + "отсутвует подключение к серверу.";
                    doc = default;
                    return false;
                }
                catch (Exception ex)
                {
                    info = information + ex.ToString();
                    doc = default;
                    return false;
                }
            }
            else
            {
                info = information + ConnectionData.NULL;
                doc = default;
                return false;
            }
        }
        */

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

            if (Client != null)
            {
                try
                {
                    IMongoDatabase db = Client.GetDatabase(databaseName);

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

            if (Client != null)
            {
                try
                {
                    IMongoDatabase db = Client.GetDatabase(databaseName);

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