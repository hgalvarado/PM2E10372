using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using PM2E10372.Models;
using System.Threading.Tasks;

namespace PM2E10372.Controllers
{
    public class DBProc
    {
        readonly SQLiteAsyncConnection _connection;

        public DBProc() { }
        public DBProc(string path) {
            _connection = new SQLiteAsyncConnection(path);
            //**Todos los objetos de bases de datos**

            //Crea una tabla tomando en cuenta los parametros de la clase Personas
            _connection.CreateTableAsync<sitios>().Wait();
        }
        /*CRUD de la BDPROC*/
        //CREATE, READ, UPDATE, DELETE

        public Task<int> addSitios(sitios sitios)
        {
            if (sitios.Id == 0)
            {
                return _connection.InsertAsync(sitios);
            }
            else
            {
                return _connection.UpdateAsync(sitios);
            }
        }
        public Task<List<sitios>> listSitios()
        {
            return _connection.Table<sitios>().ToListAsync();
        }

        public Task<sitios> GetSitiosID(int id)
        {
            return _connection.Table<sitios>()
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<int> DeleteSitios(sitios personas)
        {
            return _connection.DeleteAsync(personas);
        }


    }   
}
