﻿using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace KitBuilderWebApi.Common
{
    public static class IQueryableHelper
    {
        private static readonly FieldInfo _queryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields
            .Single(x => x.Name == "_queryCompiler");

        private static readonly TypeInfo _queryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();

        private static readonly FieldInfo _queryModelGeneratorField =
            _queryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_queryModelGenerator");

        private static readonly FieldInfo _databaseField =
            _queryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");

        private static readonly PropertyInfo _dependenciesProperty =
            typeof(Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");

        public static void WriteDebugSql<TEntity>(this IQueryable<TEntity> queryable, string Label = "")
            where TEntity : class
        {
            if (!(queryable is EntityQueryable<TEntity>) && !(queryable is InternalDbSet<TEntity>))
                throw new ArgumentException();

            var queryCompiler = (IQueryCompiler) _queryCompilerField.GetValue(queryable.Provider);
            var queryModelGenerator = (IQueryModelGenerator) _queryModelGeneratorField.GetValue(queryCompiler);
            var queryModel = queryModelGenerator.ParseQuery(queryable.Expression);
            var database = _databaseField.GetValue(queryCompiler);
            var queryCompilationContextFactory = ((DatabaseDependencies) _dependenciesProperty.GetValue(database))
                .QueryCompilationContextFactory;
            var queryCompilationContext = queryCompilationContextFactory.Create(false);
            var modelVisitor = (RelationalQueryModelVisitor) queryCompilationContext.CreateQueryModelVisitor();
            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
            var sql = modelVisitor.Queries.Join(Environment.NewLine + Environment.NewLine);

            
            Debug.WriteLine($"##########################{Label}##########################");
            Debug.WriteLine("");
            Debug.WriteLine(sql);
            Debug.WriteLine("");

        }
    }
}