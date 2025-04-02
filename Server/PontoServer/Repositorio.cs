using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using PontoServer.Models;
using static PontoServer.Controllers.UsuarioController;

namespace PontoServer
{
    public class Repositorio
    {
        private DatabaseConnection _dbConnection;

        public Repositorio()
        {
            _dbConnection = new DatabaseConnection();
        }

        public bool VerificaAutenticacao(Login login)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM \"USUARIO\" WHERE username = @username AND password = @password", conn))
                    {
                        cmd.Parameters.AddWithValue("username", login.Username);
                        cmd.Parameters.AddWithValue("password", login.Password);
                        var result = (long)cmd.ExecuteScalar();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public DataTable GetDataTable(String tabela)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand($"SELECT * FROM \"{tabela}\" ORDER BY id", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
            }
        }

        public DataTable GetByID(String tabela, int id)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand($"SELECT * FROM \"{tabela}\" WHERE id = {id}", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
            }
        }

        public DataTable GetFuncByUsuario(int id_usu)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand($"SELECT * FROM \"FUNCIONARIO\" WHERE id_usuario = {id_usu}", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
            }
        }

        public DataTable GetByParametros(String tabela, int id, string nomeCampo, string valorCampo)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                string sql = $"SELECT * FROM \"{tabela}\" WHERE 1 = 1 ";
                if (id > 0)
                    sql += $" AND id = {id}";
                if (nomeCampo != null && nomeCampo != "" && valorCampo != null && valorCampo != "")
                    sql += $" AND trim({nomeCampo}) like '{valorCampo}%'";

                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
            }
        }

        public DataTable GetByParametrosInt(String tabela, int id, string nomeCampo, string valorCampo)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                string sql = $"SELECT * FROM \"{tabela}\" WHERE 1 = 1 ";
                if (id > 0)
                    sql += $" AND id = {id}";
                if (nomeCampo != null && nomeCampo != "" && valorCampo != null && valorCampo != "")
                    sql += $" AND {nomeCampo} = {valorCampo}";

                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
            }
        }


        public DataTable GetFolha(int? id, int? id_funcionario, string data_inicio, string data_fim)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                string sql = $" SELECT f.*, s.descricao as st_jornada " +
                    $"FROM \"FOLHA\" f, " +
                    $"\"SITUACAO_JORNADA\" s " +
                    $"WHERE s.id = f.id_st_jornada";

                if (id != null && id > 0)
                    sql += $" AND f.id = {id}";
                if (id_funcionario != null && id_funcionario > 0)
                    sql += $" AND f.id_funcionario = {id_funcionario}";
                if (data_inicio != null && data_fim != null && data_inicio != "null" && data_fim != "null")
                    sql += $" AND f.data_folha BETWEEN '{data_inicio}' AND '{data_fim}'"; //exemplo: 2025-02-23 a 2025-02-25

                sql += " ORDER BY f.data_folha";

                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
            }
        }

        public DataTable GetFolhaAjuste(int? id, int? id_funcionario, int? id_folha)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                string sql = $" SELECT fa.* " +
                    $"FROM \"SOLICITACAO_AJUSTE\" fa " +
                    $"WHERE 1 = 1 ";

                if (id != null && id > 0)
                    sql += $" AND fa.id = {id}";
                if (id_funcionario != null && id_funcionario > 0)
                    sql += $" AND fa.id_funcionario = {id_funcionario}";
                if (id_folha != null && id_folha > 0)
                    sql += $" AND fa.id_folha = {id_folha}";

                sql += " ORDER BY fa.id";

                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
            }
        }

        public Resultado InsertRegistro(String tabela, Dictionary<string, object> camposValores)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    var campos = (string.Join(", ", camposValores.Keys)).Replace("@", ""); 
                    var parametros = string.Join(", ", camposValores.Keys);

                    var cmd = new NpgsqlCommand($"INSERT INTO \"{tabela}\" ({campos}) VALUES ({parametros})", conn);

                    foreach (var campoValor in camposValores)
                    {
                        cmd.Parameters.AddWithValue(campoValor.Key, campoValor.Value);
                    }

                    cmd.ExecuteNonQuery();
                    return (new Resultado { Mensagem = "Registro inserido com sucesso: ", Sucesso = true });

                }
                catch (Exception ex)
                {
                    return (new Resultado { Mensagem = "Houve um erro: " + ex.Message, Sucesso = false });
                }
            }
        }


        public Resultado UpdateRegistro(String tabela, Dictionary<string, object> camposValores, int id)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    var setClause = string.Join(", ", camposValores.Keys.Select(campo => $"{campo.Replace("@", "")} = {campo}"));

                    var cmd = new NpgsqlCommand($"UPDATE \"{tabela}\" SET {setClause} WHERE id = {id}", conn);

                    foreach (var campoValor in camposValores)
                    {
                        cmd.Parameters.AddWithValue(campoValor.Key, campoValor.Value);
                    }

                    cmd.ExecuteNonQuery();
                    return (new Resultado { Mensagem = "Registro atualizado com sucesso: ", Sucesso = true });

                }
                catch (Exception ex)
                {
                    return (new Resultado { Mensagem = "Houve um erro: " + ex.Message, Sucesso = false });  
                }
            }
        }

        public Resultado DeleteRegistro(String tabela, int id)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    var cmd = new NpgsqlCommand($"DELETE FROM \"{tabela}\" WHERE id = {id}", conn);

                    cmd.ExecuteNonQuery();
                    return (new Resultado { Mensagem = "Registro deletado com sucesso: ", Sucesso = true });

                }
                catch (Exception ex)
                {
                    return (new Resultado { Mensagem = "Houve um erro: " + ex.Message, Sucesso = false });
                }
            }
        }

        public DataTable GetByUsername(string username)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand($"SELECT * FROM \"USUARIO\" WHERE username = '{username}'", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
            }
        }

        public DataTable GetByNickname(string nickname)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand($"SELECT * FROM \"USUARIO\" WHERE nickname = '{nickname}'", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
            }
        }

        public Resultado AtualizarFolha(int id_funcionario)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    var cmd = new NpgsqlCommand($"CALL verificar_folha({id_funcionario});", conn);

                    cmd.ExecuteNonQuery();
                    return (new Resultado { Mensagem = "Atualizado com sucesso: ", Sucesso = true });

                }
                catch (Exception ex)
                {
                    return (new Resultado { Mensagem = "Houve um erro: " + ex.Message, Sucesso = false });
                }
            }
        }

        public int ProximoIDSolicitacao()
        {
            using (var conn = _dbConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    //using (var cmd = new NpgsqlCommand($"SELECT last_value + 1 FROM sq_id_solicitacao_ajuste;", conn))
                    using (var cmd = new NpgsqlCommand($"SELECT nextval('sq_id_solicitacao_ajuste');", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                return id;
                            }
                            else
                            {
                                return 0;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }

        public SaldoBancoH BancoHorasSaldo(int id_funcionario)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand($"SELECT calcula_saldo_banco_horas({id_funcionario});", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int saldo = reader.GetInt32(0);
                                return new SaldoBancoH { Saldo = saldo, Mensagem = "Selecionado com sucesso: ", Sucesso = true };
                            }
                            else
                            {
                                return new SaldoBancoH { Saldo = 0, Mensagem = "Nenhum saldo retornado.", Sucesso = false };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return (new SaldoBancoH { Saldo = 0, Mensagem = "Houve um erro: " + ex.Message, Sucesso = false });
                }
            }
        }


        public class Resultado
        {
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

        public class SaldoBancoH
        {
            public int Saldo { get; set; }
            public string Mensagem { get; set; }
            public bool Sucesso { get; set; }
        }

    }
}