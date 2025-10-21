using Dapper;
using FinTechBanking.Core.Entities;

namespace FinTechBanking.Data;

/// <summary>
/// Configuração do Dapper para mapeamento de colunas snake_case para PascalCase
/// </summary>
public static class DapperConfiguration
{
    public static void Configure()
    {
        // Mapear User
        var userMap = new CustomPropertyTypeMap(
            typeof(User),
            (type, columnName) => type.GetProperty(
                columnName,
                System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance));
        
        SqlMapper.SetTypeMap(typeof(User), userMap);

        // Mapear Account
        var accountMap = new CustomPropertyTypeMap(
            typeof(Account),
            (type, columnName) => type.GetProperty(
                columnName,
                System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance));
        
        SqlMapper.SetTypeMap(typeof(Account), accountMap);

        // Mapear Transaction
        var transactionMap = new CustomPropertyTypeMap(
            typeof(Transaction),
            (type, columnName) => type.GetProperty(
                columnName,
                System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance));
        
        SqlMapper.SetTypeMap(typeof(Transaction), transactionMap);
    }
}

