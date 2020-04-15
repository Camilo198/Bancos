using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Descripción breve de contratosBD
/// </summary>
public class contratosBD
{
    
    private SqlConnection conn;
    private SqlCommand cmd;
	public contratosBD()
	{

	}

    public DataSet contratos()
    {
        conn = new SqlConnection("Data Source=SBOGCHE005F\\Prueba;Initial Catalog=Bancos;Persist Security Info=True;User ID=EpicMigracion;Password=3P$Ch3*Migra");
        cmd = new SqlCommand();
        DataSet ds = new DataSet();
        SqlDataAdapter da  = new SqlDataAdapter();
        try
        {
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM temp_CONTRATOS WHERE REVISADO = '0' ORDER BY FECHA_TRANSACCION";
            da.SelectCommand = cmd;             
            da.Fill(ds, "Usuarios");
        }
        catch
        {

        }
        return ds;
    }

    public void actualizarContrato(String contrato, String firma)
    {
        conn = new SqlConnection("Data Source=SBOGCHE005F\\Prueba;Initial Catalog=Bancos;Persist Security Info=True;User ID=EpicMigracion;Password=3P$Ch3*Migra");
        cmd = new SqlCommand();
        
        try
        {
            cmd.Connection = conn;
            cmd.CommandText = "UPDATE temp_CONTRATOS SET REVISADO = '1' WHERE REF_VENTA = '" + contrato.ToString()  + "' AND FIRMA = '" + firma.ToString()  + "'";
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close(); 
        }
        catch
        {

        }
    }

}