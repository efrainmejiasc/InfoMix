using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;


public class Util
{
    private static string _encrKey = "3NcRípt4";

    private static string EncrKey
    {
        get { return _encrKey; }
        set { _encrKey = value; }
    }

    #region Encriptar

    public static string EncriptarVarURL(string strText)
    {
        byte[] bKey = new byte[8];
        byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        try
        {
            bKey = System.Text.Encoding.UTF8.GetBytes(EncrKey.Substring(0, 7));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            Byte[] inputByteArray = Encoding.UTF8.GetBytes(strText);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(bKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());

        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
    }


    /// <summary>
    /// Método para encriptar un texto plano usando el algoritmo (Rijndael).
    /// Este es el mas simple posible, muchos de los datos necesarios los
    /// definimos como constantes.
    /// </summary>
    /// <param name="textoQueEncriptaremos">texto a encriptar</param>
    /// <returns>Texto encriptado</returns>
    public static string Encriptar(string textoQueEncriptaremos)
    {
        string textoEncriptado = null;

        textoEncriptado = Encriptar(textoQueEncriptaremos, "P@55vv0rd3nCR1pt4ciÓn", "1ing3Nü5", "MD5", 1, "@51st3Ma4teNCiÓN", 128);

        return textoEncriptado;
    }

    /// <summary>
    /// Método para encriptar un texto plano usando el algoritmo (Rijndael)
    /// </summary>
    /// <returns>Texto encriptado</returns>
    public static string Encriptar(
        string textoQueEncriptaremos,
        string passBase,
        string saltValue,
        string hashAlgorithm,
        int passwordIterations,
        string initVector,
        int keySize)
    {
        byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
        byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(textoQueEncriptaremos);

        PasswordDeriveBytes password = new PasswordDeriveBytes(passBase, saltValueBytes, hashAlgorithm, passwordIterations);

        byte[] keyBytes = password.GetBytes(keySize / 8);

        RijndaelManaged symmetricKey = new RijndaelManaged()
        {
            Mode = CipherMode.CBC
        };

        ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
        cryptoStream.FlushFinalBlock();

        byte[] cipherTextBytes = memoryStream.ToArray();

        memoryStream.Close();
        cryptoStream.Close();

        string cipherText = Convert.ToBase64String(cipherTextBytes);

        return cipherText;
    }

    #endregion


    #region Desencriptar

    public static string DesencriptarVarURL(string strText)
    {
        byte[] bKey = new byte[8];
        byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        try
        {
            bKey = System.Text.Encoding.UTF8.GetBytes(EncrKey.Substring(0, 7));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            Byte[] inputByteArray = inputByteArray = Convert.FromBase64String(strText);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(bKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
    }



    /// <summary>
    /// Método para desencriptar un texto encriptado.
    /// </summary>
    /// <returns>Texto desencriptado</returns>
    public static string Desencriptar(string textoEncriptado)
    {
        string textoDesencriptado = null;

        textoDesencriptado = Desencriptar(textoEncriptado, "P@55vv0rd3nCR1pt4ciÓn", "1ing3Nü5", "MD5", 1, "@51st3Ma4teNCiÓN", 128);

        return textoDesencriptado;
    }

    /// <summary>
    /// Método para desencriptar un texto encriptado (Rijndael)
    /// </summary>
    /// <returns>Texto desencriptado</returns>
    public static string Desencriptar(
        string textoEncriptado,
        string passBase,
        string saltValue,
        string hashAlgorithm,
        int passwordIterations,
        string initVector,
        int keySize)
    {
        byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
        byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
        byte[] cipherTextBytes = Convert.FromBase64String(textoEncriptado);

        PasswordDeriveBytes password = new PasswordDeriveBytes(passBase, saltValueBytes, hashAlgorithm, passwordIterations);

        byte[] keyBytes = password.GetBytes(keySize / 8);

        RijndaelManaged symmetricKey = new RijndaelManaged()
        {
            Mode = CipherMode.CBC
        };

        ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

        MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
        CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

        byte[] plainTextBytes = new byte[cipherTextBytes.Length];

        int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

        memoryStream.Close();
        cryptoStream.Close();

        string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

        return plainText;
    }

    #endregion

    

    #region FUNCIONES RUT - DÍGITO VERIFICADOR

    /// <summary>
    /// Realiza el calculo del dígito verificador para el Rut de Chile para su posterior comparación
    /// con un dígito verificador ingresado
    /// </summary>
    /// <param name="rut">Número de Rut. Sin puntos para la separación de miles y millones.</param>
    /// <returns>Dígito verificador (0,1,2,3,4,5,6,7,8,9 ó K) del Rut entregado</returns>
    public static string DigitoVerificador(int rut)
    {
        int Digito;
        int Contador;
        int Multiplo;
        int Acumulador;
        string RutDigito;

        Contador = 2;
        Acumulador = 0;

        while (rut != 0)
        {
            Multiplo = (rut % 10) * Contador;
            Acumulador = Acumulador + Multiplo;
            rut = rut/10;
            Contador = Contador + 1;

            if (Contador == 8)
            {
                Contador = 2;
            }
        }

        Digito = 11 - (Acumulador % 11);
        RutDigito = Digito.ToString().Trim();

        if (Digito == 10 )
        {
            RutDigito = "K";
        }

        if (Digito == 11)
        {
            RutDigito = "0";
        }

        return (RutDigito);
    }

    /// <summary>
    /// Cambia el formato inglés (MM/DD/YYYY HH:MM:SS o MM-DD-YYYY HH:MM:SS) o español 
    /// (DD/MM/YYYY HH:MM:SS o DD-MM-YYYY HH:MM:SS) de la fecha a formato ANSI 
    /// (YYYYMMDD HH:MM:SS) para tener independencia del formato con la base de datos.
    /// </summary>
    /// <param name="fechaOriginal">Fecha en formato inglés o español</param>
    /// <returns>Fecha en formato ANSI (YYYYMMDD HH:MM:SS)</returns>
    public static string FechaAnsi(string fechaOriginal)
    {
        string formatoFecha = CultureInfo.CurrentCulture.Name;
        string fechaAnsi = null;
        string dia = null;
        string mes = null;
        string ano = null;
        string hrs = null;
        string[] fecha = null;
        string[] hora = null;

        if (fechaOriginal.Contains("-"))
        {
            fecha = fechaOriginal.Split('-');
            hora = fecha[2].Split(' ');
        }
        else
        {
            fecha = fechaOriginal.Split('/');
            hora = fecha[2].Split(' ');
        }

        if (formatoFecha.Contains("es"))
        {
            dia = fecha[0];
            mes = fecha[1];
            ano = hora[0];
            hrs = hora[1];
        }
        else if (formatoFecha.Contains("en"))
        {
            dia = fecha[1];
            mes = fecha[0];
            ano = hora[0];
            hrs = hora[1];
        }

        fechaAnsi = ano + mes + dia + " " + hrs;

        return fechaAnsi;
    }

    #endregion

    #region FUNCIONES EXPORTACIÓN A EXCEL
    
    public static void ExportaExcel(DataGrid DG, System.Web.HttpResponse RS, string nombreArchivo)
    {
        // Damos la salida como attachment
        RS.AddHeader("content-disposition", "attachment; filename=" + nombreArchivo + ".xls");
        // Especificamos el tipo de salida.
        RS.ContentType = "application/vnd.ms-excel";
        // Asociamos la salida con la codificación UTF8 (para poder visualizar los acentos correctamente)
        RS.ContentEncoding = Encoding.Default;
        DG.EnableViewState = false;
        RS.Charset = "UTF-8";
        StringWriter tw = new StringWriter();
        System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        DG.RenderControl(hw); // DG es el DATAGRID
        //Escribimos el HTML en el Explorador
        RS.Write(tw.ToString());
        // Terminamos el Response.
        RS.Flush();
        RS.End();
        
    }

    public static void ExportaExcel2(DataGrid DG, System.Web.HttpResponse RS, string nombreArchivo, string titulo)
    {
        // Damos la salida como attachment
        RS.AddHeader("content-disposition", "attachment; filename=" + nombreArchivo + ".xls");
        // Especificamos el tipo de salida.
        RS.ContentType = "application/vnd.ms-excel";
        // Asociamos la salida con la codificación UTF8 (para poder visualizar los acentos correctamente)
        RS.ContentEncoding = Encoding.Default;
        DG.EnableViewState = false;
        RS.Charset = "UTF-8";
        StringWriter tw = new StringWriter();
        System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        DG.RenderControl(hw); // DG es el DATAGRID        
        RS.Write(titulo);
        //Escribimos el HTML en el Explorador
        RS.Write(tw.ToString());       
        // Terminamos el Response.
        RS.Flush();
        RS.End();

    }

    public static void ExportaExcel(GridView GRV, System.Web.HttpResponse RS, string nombreArchivo)
    {
        // Damos la salida como attachment
        RS.AddHeader("content-disposition", "attachment; filename=" + nombreArchivo + ".xls");
        // Especificamos el tipo de salida.
        RS.ContentType = "application/vnd.ms-excel";
        // Asociamos la salida con la codificación UTF8 (para poder visualizar los acentos correctamente)
        RS.ContentEncoding = Encoding.Default;
        RS.Charset = "UTF-8";
        StringWriter tw = new StringWriter();
        System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        GRV.RenderControl(hw); // GRV es el GRIDVIEW
        //Escribimos el HTML en el Explorador
        RS.Write(tw.ToString());
        // Terminamos el Response.
        RS.End();
    }
    
    #endregion

    public static string Archivo_FormatearNombre(string file)
    {
        file.ToLower();
        //Elimina caracteres invalidos
        file = file.Replace("/", "");
        file = file.Replace("*", "");
        file = file.Replace("<", "");
        file = file.Replace(">", "");
        file = file.Replace(":", "");
        file = file.Replace("?", "");
        file = file.Replace("|", "");

        //combinaciones con backslash
        file = file.Replace("\'", "'");
        file = file.Replace("\"", "'");
        file = file.Replace("\0", "0");
        file = file.Replace("\a", "a");
        file = file.Replace("\b", "b");
        file = file.Replace("\f", "f");
        file = file.Replace("\n", "n");
        file = file.Replace("\r", "r");
        file = file.Replace("\t", "t");
        file = file.Replace("\v", "v");

        //Elimina espacios
        file = file.Replace(" ", "_");

        file.ToUpper();

        return file;
    }

    public static void Archivo_Subir(HttpServerUtility server, string file, string folder, FileUpload FU)
    {
        string savePath = null;

        if (server != null && folder != null && folder.Length > 0)
        {
            folder = server.MapPath(@"~\" + folder);

            if (Directory.Exists(folder))
            {
                if (FU != null && file != null && file.Length > 0)
                {
                    savePath = folder + "\\" + file;

                    if (File.Exists(savePath))
                    {
                        File.Delete(savePath);
                    }

                    FU.SaveAs(savePath);
                }
            }
        }
    }

    //public static void Archivo_SubirAjax(HttpServerUtility server, string file, string folder, AsyncFileUpload FU)
    //{
    //    string savePath = null;

    //    if (server != null && folder != null && folder.Length > 0)
    //    {
    //        folder = server.MapPath(@"~\" + folder);

    //        if (Directory.Exists(folder))
    //        {
    //            if (FU != null && file != null && file.Length > 0)
    //            {
    //                savePath = folder + "\\" + file;

    //                if (File.Exists(savePath))
    //                {
    //                    File.Delete(savePath);
    //                }

    //                FU.SaveAs(savePath);
    //            }
    //        }
    //    }
    //}

    public static string LinkDescarga_Generar(HttpServerUtility server, string file, string folder)
    {
        string link = "";
        string path = null;

        if (server != null && folder != null && folder.Length > 0)
        {
            path = server.MapPath(@"~\" + folder);

            if (Directory.Exists(path))
            {
                if (file != null && file.Length > 0)
                {
                    path = path + "\\" + file;

                    if (File.Exists(path))
                    {
                        folder = Util.EncriptarVarURL(folder).Replace('+', ' ');
                        file = Util.EncriptarVarURL(file).Replace('+', ' ');

                       link = "AbrirDocumentoV2.aspx?fldr=" + folder + "&fl=" + file;
                        
                        //link = "<a href='AbrirDocumentoV2.aspx?fldr='" + folder + "&fl=" + file +">link</a>";
                    }
                }
            }
        }

        return link;
    }

}