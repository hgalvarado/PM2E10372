using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2E10372
{
    public partial class App : Application
    {
        static Controllers.DBProc dbProc;
        public static Controllers.DBProc Instancia
        {
            get
            {
                if (dbProc == null)
                {
                    string dbname = "SitiosDB.db3";
                    string dbpath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    string dbfull = Path.Combine(dbpath, dbname);
                    dbProc = new Controllers.DBProc(dbfull);
                }
                return dbProc;
            }
        }   
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new Views.PageListSitios());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
