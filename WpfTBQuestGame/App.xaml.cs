using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfTBQuestGame.BusinessLayer;

namespace WpfTBQuestGame
{
    
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            GameBusiness gameBusiness = new GameBusiness();
        }
    }
}
