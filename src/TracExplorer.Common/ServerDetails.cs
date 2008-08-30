﻿#region GPL Licence
/**********************************************************************
 VSTrac - Visual Studio Trac Integration
 Copyright (C) 2008 Mladen Mihajlovic
 http://vstrac.devjavu.com/
 
 This program is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.
 
 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.
 
 You should have received a copy of the GNU General Public License
 along with this program.  If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#endregion

using System.Collections.Generic;
using System;
using System.Security.Cryptography;
using System.Text;

namespace TracExplorer.Common
{
    public class ServerDetails
    {
        #region Private Variables
        private string server;
        private bool authenticated;
        private string username;
        private byte[] encryptPassword;
        private List<TicketQueryDefinition> ticketQueries = new List<TicketQueryDefinition>();
        #endregion

        static private byte[] entropy = { 17, 6, 19, 74 };

        #region Public Properties
        public string Server
        {
            get { return this.server; }
            set { this.server = value; }
        }

        public bool Authenticated
        {
            get { return this.authenticated; }
            set { this.authenticated = value; }
        }

        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }
 
        public string GetPassword()
        {
            if (this.encryptPassword != null)
            {
                byte[] cipherBytes = ProtectedData.Unprotect(this.encryptPassword, entropy, DataProtectionScope.CurrentUser);
                return Encoding.Unicode.GetString(cipherBytes);
            }
            else
            {
                return "";
            }
        }

        public void SetPassword(string value)
        {
            byte[] cipherBytes;
    
            // Convert the string value.
            cipherBytes = Encoding.Unicode.GetBytes(value);

            // Encrypt the string value (cipher text).
            this.encryptPassword = ProtectedData.Protect(cipherBytes, entropy, DataProtectionScope.CurrentUser);
        }
    
        public byte[] EncryptPassword
        {
            get { return this.encryptPassword; }
            set { this.encryptPassword = value; }
        }

        public List<TicketQueryDefinition> TicketQueries
        {
            get { return ticketQueries; }
            set { ticketQueries = value; }
        }
        #endregion

        #region Public Methods
        public string XmlRpcUrl()
        {
            return this.server + "login/xmlrpc";
        }
        
        public string WikiPageUrl(string WikiPageName)
        {
            return this.server + "wiki/" + WikiPageName;
        }

        public string TicketUrl(int ticketId)
        {
            return this.server + "ticket/" + ticketId.ToString();
        }

        public string NewTicketUrl()
        {
            return this.server + "newticket/";
        }
        #endregion

        #region ctors
        public ServerDetails()
        { }

        public ServerDetails(string server)
        {
            this.Server = server;
        }

        public ServerDetails(string server, string username, string password)
            : this(server)
        {
            this.Authenticated = true;
            this.Username = username;
            this.SetPassword(password);
        }
        #endregion



        #region ToString
        public override string ToString()
        {
            return server;
        }
        #endregion
    }
}