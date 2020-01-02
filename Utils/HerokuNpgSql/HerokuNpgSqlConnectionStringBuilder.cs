using System;
using System.Data.Common;
// ReSharper disable MemberCanBePrivate.Global

namespace HerokuNpgSql
{
    public class HerokuNpgSqlConnectionStringBuilder : DbConnectionStringBuilder
    {
        private string _database;
        private string _host;
        private string _password;
        private bool _pooling;
        private int _port;
        private int _minPoolSize;
        private int _maxPoolSize;
        private SslMode _sslMode;
        private bool _trustServerCertificate;
        private string _username;

        public HerokuNpgSqlConnectionStringBuilder(string herokuPostgreSqlConnectionSting)
        {
            ParseUri(herokuPostgreSqlConnectionSting);
        }

        public string Database
        {
            get => _database;
            set
            {
                base["database"] = value;
                _database = value;
            }
        }

        public string Host
        {
            get => _host;
            set
            {
                base["host"] = value;
                _host = value;
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                base["password"] = value;
                _password = value;
            }
        }

        public bool Pooling
        {
            get => _pooling;
            set
            {
                base["pooling"] = value;
                _pooling = value;
            }
        }
        
        public int Port
        {
            get => _port;
            set
            {
                base["port"] = value;
                _port = value;
            }
        }

        public int MinPoolSize
        {
            get => _minPoolSize;
            set
            {
                base["minpoolsize"] = value;
                _minPoolSize = value;
            }
        }
        
        public int MaxPoolSize
        {
            get => _maxPoolSize;
            set
            {
                base["maxPoolSize"] = value;
                _maxPoolSize = value;
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                base["username"] = value;
                _username = value;
            }
        }

        public bool TrustServerCertificate
        {
            get => _trustServerCertificate;
            set
            {
                base["trust server certificate"] = value;
                _trustServerCertificate = value;
            }
        }

        public SslMode SslMode
        {
            get => _sslMode;
            set
            {
                base["ssl mode"] = value.ToString();
                _sslMode = value;
            }
        }

        public override object this[string keyword]
        {
            get
            {
                if (keyword == null) throw new ArgumentNullException(nameof(keyword));
                return base[keyword.ToLower()];
            }
            set
            {
                if (keyword == null) throw new ArgumentNullException(nameof(keyword));

                switch (keyword.ToLower())
                {
                    case "host":
                        Host = (string) value;
                        break;

                    case "port":
                        Port = Convert.ToInt32(value);
                        break;

                    case "database":
                        Database = (string) value;
                        break;

                    case "username":
                        Username = (string) value;
                        break;

                    case "password":
                        Password = (string) value;
                        break;

                    case "pooling":
                        Pooling = Convert.ToBoolean(value);
                        break; 
                    
                    case "minpoolsize":
                        MinPoolSize = Convert.ToInt32(value);
                        break;
                    
                    case "maxpoolsize":
                        MaxPoolSize = Convert.ToInt32(value);
                        break;

                    case "trust server certificate":
                        TrustServerCertificate = Convert.ToBoolean(value);
                        break;

                    case "sslmode":
                        SslMode = (SslMode) value;
                        break;

                    default:
                        throw new ArgumentException($"Invalid keyword '{keyword}'.");
                }
            }
        }

        public override bool ContainsKey(string keyword)
        {
            return base.ContainsKey(keyword.ToLower());
        }

        private void ParseUri(string uriString)
        {
            var isUri = Uri.TryCreate(uriString, UriKind.Absolute, out var uri);

            if (!isUri) throw new FormatException($"'{uriString}' is not a valid URI.");

            Host = uri.Host;
            Port = uri.Port;
            Database = uri.LocalPath.Substring(1);
            Username = uri.UserInfo.Split(':')[0];
            Password = uri.UserInfo.Split(':')[1];
        }
    }
}