using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpreadBet.Common.Interfaces;
using SpreadBet.Repository;

namespace SpreadBet.Application
{
    public class MigrateDatabase : IExecutableApplication
    {
        public void Run()
        {
            var initialise = new DatabaseInitializer(new Context());
        }
    }
}
