// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 05-24-2016
// ***********************************************************************
// <copyright file="ISQLiteConnection.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace Core.Interfaces.SQLiteInterface
{
    /// <summary>
    /// Interface ISQLiteConnection
    /// </summary>
    public interface ISQLiteConnection: IDisposable
    {
        /// <summary>
        /// Begins the transaction.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Gets or sets the busy timeout.
        /// </summary>
        /// <value>The busy timeout.</value>
        TimeSpan BusyTimeout { get; set; }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        void Close();

        /// <summary>
        /// Commits this instance.
        /// </summary>
        void Commit();

        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <param name="cmdText">The command text.</param>
        /// <param name="ps">The ps.</param>
        /// <returns>SQLite.SQLiteCommand.</returns>
        SQLite.SQLiteCommand CreateCommand(string cmdText, params object[] ps);

        /// <summary>
        /// Creates the index.
        /// </summary>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="unique">if set to <c>true</c> [unique].</param>
        /// <returns>System.Int32.</returns>
        int CreateIndex(string indexName, string tableName, string columnName, bool unique = false);

        /// <summary>
        /// Creates the index.
        /// </summary>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="unique">if set to <c>true</c> [unique].</param>
        /// <returns>System.Int32.</returns>
        int CreateIndex(string indexName, string tableName, string[] columnNames, bool unique = false);

        /// <summary>
        /// Creates the index.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="unique">if set to <c>true</c> [unique].</param>
        /// <returns>System.Int32.</returns>
        int CreateIndex(string tableName, string columnName, bool unique = false);

        /// <summary>
        /// Creates the index.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="unique">if set to <c>true</c> [unique].</param>
        /// <returns>System.Int32.</returns>
        int CreateIndex(string tableName, string[] columnNames, bool unique = false);

        /// <summary>
        /// Creates the index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property">The property.</param>
        /// <param name="unique">if set to <c>true</c> [unique].</param>
        void CreateIndex<T>(System.Linq.Expressions.Expression<Func<T, object>> property, bool unique = false);

        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <param name="ty">The ty.</param>
        /// <param name="createFlags">The create flags.</param>
        /// <returns>System.Int32.</returns>
        int CreateTable(Type ty, SQLite.CreateFlags createFlags = SQLite.CreateFlags.None);

        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="createFlags">The create flags.</param>
        /// <returns>System.Int32.</returns>
        int CreateTable<T>(SQLite.CreateFlags createFlags = SQLite.CreateFlags.None);

        /// <summary>
        /// Gets the database path.
        /// </summary>
        /// <value>The database path.</value>
        string DatabasePath { get; }

        /// <summary>
        /// Deferreds the query.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="query">The query.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>System.Collections.Generic.IEnumerable&lt;System.Object&gt;.</returns>
        System.Collections.Generic.IEnumerable<object> DeferredQuery(SQLite.TableMapping map, string query, params object[] args);

        /// <summary>
        /// Deferreds the query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>System.Collections.Generic.IEnumerable&lt;T&gt;.</returns>
        System.Collections.Generic.IEnumerable<T> DeferredQuery<T>(string query, params object[] args) where T : new();

        /// <summary>
        /// Deletes the specified object to delete.
        /// </summary>
        /// <param name="objectToDelete">The object to delete.</param>
        /// <returns>System.Int32.</returns>
        int Delete(object objectToDelete);

        /// <summary>
        /// Deletes the specified primary key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns>System.Int32.</returns>
        int Delete<T>(object primaryKey);

        /// <summary>
        /// Deletes all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>System.Int32.</returns>
        int DeleteAll<T>();
        /// <summary>
        /// Drops the table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>System.Int32.</returns>
        int DropTable<T>();

        /// <summary>
        /// Enables the load extension.
        /// </summary>
        /// <param name="onoff">The onoff.</param>
        void EnableLoadExtension(int onoff);

        /// <summary>
        /// Executes the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>System.Int32.</returns>
        int Execute(string query, params object[] args);

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>T.</returns>
        T ExecuteScalar<T>(string query, params object[] args);

        /// <summary>
        /// Finds the specified pk.
        /// </summary>
        /// <param name="pk">The pk.</param>
        /// <param name="map">The map.</param>
        /// <returns>System.Object.</returns>
        object Find(object pk, SQLite.TableMapping map);

        /// <summary>
        /// Finds the specified predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns>T.</returns>
        T Find<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : new();

        /// <summary>
        /// Finds the specified pk.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pk">The pk.</param>
        /// <returns>T.</returns>
        T Find<T>(object pk) where T : new();

        /// <summary>
        /// Gets the specified predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns>T.</returns>
        T Get<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : new();

        /// <summary>
        /// Gets the specified pk.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pk">The pk.</param>
        /// <returns>T.</returns>
        T Get<T>(object pk) where T : new();

        /// <summary>
        /// Gets the mapping.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="createFlags">The create flags.</param>
        /// <returns>SQLite.TableMapping.</returns>
        SQLite.TableMapping GetMapping(Type type, SQLite.CreateFlags createFlags = SQLite.CreateFlags.None);

        /// <summary>
        /// Gets the mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>SQLite.TableMapping.</returns>
        SQLite.TableMapping GetMapping<T>();

        /// <summary>
        /// Gets the table information.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>System.Collections.Generic.List&lt;SQLite.SQLiteConnection.ColumnInfo&gt;.</returns>
        System.Collections.Generic.List<SQLite.SQLiteConnection.ColumnInfo> GetTableInfo(string tableName);

        /// <summary>
        /// Gets the handle.
        /// </summary>
        /// <value>The handle.</value>
        IntPtr Handle { get; }

        /// <summary>
        /// Inserts the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>System.Int32.</returns>
        int Insert(object obj);

        /// <summary>
        /// Inserts the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="extra">The extra.</param>
        /// <returns>System.Int32.</returns>
        int Insert(object obj, string extra);

        /// <summary>
        /// Inserts the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="extra">The extra.</param>
        /// <param name="objType">Type of the object.</param>
        /// <returns>System.Int32.</returns>
        int Insert(object obj, string extra, Type objType);

        /// <summary>
        /// Inserts the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="objType">Type of the object.</param>
        /// <returns>System.Int32.</returns>
        int Insert(object obj, Type objType);

        /// <summary>
        /// Inserts all.
        /// </summary>
        /// <param name="objects">The objects.</param>
        /// <returns>System.Int32.</returns>
        int InsertAll(System.Collections.IEnumerable objects);

        /// <summary>
        /// Inserts all.
        /// </summary>
        /// <param name="objects">The objects.</param>
        /// <param name="extra">The extra.</param>
        /// <returns>System.Int32.</returns>
        int InsertAll(System.Collections.IEnumerable objects, string extra);

        /// <summary>
        /// Inserts all.
        /// </summary>
        /// <param name="objects">The objects.</param>
        /// <param name="objType">Type of the object.</param>
        /// <returns>System.Int32.</returns>
        int InsertAll(System.Collections.IEnumerable objects, Type objType);

        /// <summary>
        /// Inserts the or replace.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>System.Int32.</returns>
        int InsertOrReplace(object obj);

        /// <summary>
        /// Inserts the or replace.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="objType">Type of the object.</param>
        /// <returns>System.Int32.</returns>
        int InsertOrReplace(object obj, Type objType);

        /// <summary>
        /// Gets a value indicating whether this instance is in transaction.
        /// </summary>
        /// <value><c>true</c> if this instance is in transaction; otherwise, <c>false</c>.</value>
        bool IsInTransaction { get; }

        /// <summary>
        /// Queries the specified map.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="query">The query.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>System.Collections.Generic.List&lt;System.Object&gt;.</returns>
        System.Collections.Generic.List<object> Query(SQLite.TableMapping map, string query, params object[] args);

        /// <summary>
        /// Queries the specified query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>System.Collections.Generic.List&lt;T&gt;.</returns>
        System.Collections.Generic.List<T> Query<T>(string query, params object[] args) where T : new();

        /// <summary>
        /// Releases the specified savepoint.
        /// </summary>
        /// <param name="savepoint">The savepoint.</param>
        void Release(string savepoint);

        /// <summary>
        /// Rollbacks this instance.
        /// </summary>
        void Rollback();

        /// <summary>
        /// Rollbacks to.
        /// </summary>
        /// <param name="savepoint">The savepoint.</param>
        void RollbackTo(string savepoint);

        /// <summary>
        /// Runs the in transaction.
        /// </summary>
        /// <param name="action">The action.</param>
        void RunInTransaction(Action action);

        /// <summary>
        /// Saves the transaction point.
        /// </summary>
        /// <returns>System.String.</returns>
        string SaveTransactionPoint();

        /// <summary>
        /// Gets a value indicating whether [store date time as ticks].
        /// </summary>
        /// <value><c>true</c> if [store date time as ticks]; otherwise, <c>false</c>.</value>
        bool StoreDateTimeAsTicks { get; }

        /// <summary>
        /// Tables this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>SQLite.TableQuery&lt;T&gt;.</returns>
        SQLite.TableQuery<T> Table<T>() where T : new();

        /// <summary>
        /// Gets the table mappings.
        /// </summary>
        /// <value>The table mappings.</value>
        System.Collections.Generic.IEnumerable<SQLite.TableMapping> TableMappings { get; }

        /// <summary>
        /// Gets or sets a value indicating whether [time execution].
        /// </summary>
        /// <value><c>true</c> if [time execution]; otherwise, <c>false</c>.</value>
        bool TimeExecution { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ISQLiteConnection"/> is trace.
        /// </summary>
        /// <value><c>true</c> if trace; otherwise, <c>false</c>.</value>
        bool Trace { get; set; }

        /// <summary>
        /// Updates the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>System.Int32.</returns>
        int Update(object obj);

        /// <summary>
        /// Updates the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="objType">Type of the object.</param>
        /// <returns>System.Int32.</returns>
        int Update(object obj, Type objType);

        /// <summary>
        /// Updates all.
        /// </summary>
        /// <param name="objects">The objects.</param>
        /// <returns>System.Int32.</returns>
        int UpdateAll(System.Collections.IEnumerable objects);
    }
}