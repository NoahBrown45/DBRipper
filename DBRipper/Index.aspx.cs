using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Text;
using System.Collections;
using System.IO;
using System.IO.Compression;

namespace DBRipper
{
    public partial class Index : System.Web.UI.Page
    {

        private static Dictionary<string, string> conversionTypes = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Populate our dictionary with our types for later useage.
            conversionTypes.Add("datetime", "datetime");
            conversionTypes.Add("date", "datetime");
            conversionTypes.Add("time", "datetime");
            conversionTypes.Add("timestamp", "datetime");
            conversionTypes.Add("double", "double");
            conversionTypes.Add("float", "float");
            conversionTypes.Add("longtext", "string");
            conversionTypes.Add("mediumtext", "string");
            conversionTypes.Add("text", "string");
            conversionTypes.Add("tinytext", "string");
        }

        public static string Down_Generate(string DBName, string DBServer, string UserName, string Password)
        {
            try {

                //this will be where we store our results
                StringBuilder results = new StringBuilder();

                //First we will open our database connection with our values from our form.
                using (OdbcConnection conn = new OdbcConnection("DRIVER={MySQL ODBC 3.51 Driver};Database=" + DBName + ";Server=" + DBServer + ";UID=" + UserName + ";PWD=" + Password + ";"))
                {
                    //Lets try to open our connection.  If we can't, no point in going any further.
                    try 
                    { 
                        conn.Open();
                    }
                    catch (NotSupportedException NSEX)
                    {
                        //Looks like we have an error.
                        results.Append("<span>Error Connection To The Database.  Please Check Connection Parameters And Try Again.</span><br />");
                        return results.ToString();
                    }

                    //We need a list of all the tables in our database.
                    StringBuilder sqlQuery = new StringBuilder("SHOW TABLES IN ");
                    sqlQuery.Append(DBName);
                    sqlQuery.Append(";");

                    OdbcCommand cmd = new OdbcCommand(sqlQuery.ToString(), conn);

                    OdbcDataReader dr = cmd.ExecuteReader();

                    //Reading our table names...
                    List<string> tables = new List<string>();
                    results.Append("<span>");
                    results.Append("Locating Tables...");
                    results.Append("</span>");
                    results.Append("<br />");

                    while (dr.Read())
                    {
                        //Add the table to our list of tables.
                        tables.Add(dr["Tables_in_" + DBName].ToString());
                    
                        //Show that we found this table in our results.
                        results.Append("<span>");
                        results.Append("Table Located:  ");
                        results.Append(dr["Tables_in_" + DBName].ToString());
                        results.Append("</span>");
                        results.Append("<br />");
                    }

                    dr.Close();

                    results.Append("<br />");

                    //Now that we have our tables, lets get our descriptions of each one
                    sqlQuery.Clear();

                    //Since we save the new class at the end of each of these cycles we should go ahead and clear our output folder
                    //just in case there is anything leftover from last time.
                    Array.ForEach(Directory.GetFiles("/OutputFiles/"), File.Delete);

                    foreach (string tableName in tables)
                    {
                        sqlQuery.Append("DESCRIBE ");
                        sqlQuery.Append(tableName);
                        sqlQuery.Append(";");

                        cmd.CommandText = sqlQuery.ToString();

                        dr = cmd.ExecuteReader();

                        //From here we can grab the details for each table, and generate a new class based on it

                        //Here is where we will start building the text for our new class
                        StringBuilder NewClass = new StringBuilder();

                        // Update Our Results
                        results.Append("<span>Generating Class ");
                        results.Append(tableName);
                        results.Append("...</span><br />");

                        //First we append our using statements
                        NewClass.Append(" using System;\n using System.Collections.Generic;\n using System.Data.Odbc;\n using System.Web;");
                        NewClass.Append("\n\n");

                        //Next we appeand our namespace
                        NewClass.Append("namespace ");
                        NewClass.Append(typeof(Index).Namespace);
                        NewClass.Append("\n{");

                        //Now we can open our class
                        NewClass.Append("public class ");
                        NewClass.Append(tableName);
                        NewClass.Append("\n{\n\n");

                        //Now that our class is open we can go ahead and put in our comment to let the user know that this is a generated file
                        NewClass.Append("//This File Was Automatically Generated On ");
                        NewClass.Append(DateTime.Now.ToString());
                        NewClass.Append(" By the HiveBuilder Database Ripper.\n\n");

                        //We will need a list of fields for later use
                        List<string> fields = new List<string>();

                        //We also need to note our Primary Key so we can access our database later.
                        //TODO: Currently this system only supports single primary keys, it needs to support multiple primary keys.
                        string pk = "";

                        //Now we can go ahead and start generating our class variables.
                        while (dr.Read())
                        {
                            NewClass.Append("public ");
                        
                            //check our mySql type to see which c# type we need.
                            if (dr["Type"].ToString().StartsWith("int"))
                            {
                                NewClass.Append("int ");
                                fields.Add(dr["Field"].ToString() + "/int");
                            }
                            else if (dr["Type"].ToString().StartsWith("varchar"))
                            {
                                NewClass.Append("string ");
                                fields.Add(dr["Field"].ToString() + "/string");
                            }
                            else if (dr["Type"].ToString().StartsWith("decimal"))
                            {
                                NewClass.Append("decimal ");
                                fields.Add(dr["Field"].ToString() + "/decimal");
                            }
                            else if (dr["Type"].ToString().StartsWith("bigint"))
                            {
                                NewClass.Append("int ");
                                fields.Add(dr["Field"].ToString() + "/int");
                            }
                            else if (dr["Type"].ToString().StartsWith("mediumint"))
                            {
                                NewClass.Append("int ");
                                fields.Add(dr["Field"].ToString() + "/int");
                            }
                            else if (dr["Type"].ToString().StartsWith("smallint"))
                            {
                                NewClass.Append("int ");
                                fields.Add(dr["Field"].ToString() + "/int");
                            }
                            else if (dr["Type"].ToString().StartsWith("tinyint"))
                            {
                                NewClass.Append("int ");
                                fields.Add(dr["Field"].ToString() + "/int");
                            }
                            else if (dr["Type"].ToString().StartsWith("char"))
                            {
                                NewClass.Append("string ");
                                fields.Add(dr["Field"].ToString() + "/string");
                            }
                            else if (dr["Type "].ToString().StartsWith("bit"))
                            {
                                NewClass.Append("bool ");
                                fields.Add(dr["Field"].ToString() + "/bool");
                            }
                            else
                            {
                                NewClass.Append(conversionTypes[dr["Type"].ToString()].ToString());
                                NewClass.Append(" ");

                                fields.Add(dr["Field"].ToString() + "/" + conversionTypes[dr["Type"].ToString()].ToString());
                            }

                            NewClass.Append(dr["Field"].ToString());
                            NewClass.Append(" { get; set; }\n");

                            //Here we check to see if we are looking at the primary key.  If we are, we note it for later use in the db accessors.
                            if (pk == "" && dr["Key"].ToString() == "PRI")
                            {
                                pk = dr["Field"].ToString();
                            }

                        }

                        NewClass.Append("\n");

                        //All of our variables are in the class at this point, so now we can build our constructors.
                        NewClass.Append("public ");
                        NewClass.Append(tableName);
                        NewClass.Append("(");
                        for (int i = 0; i > fields.Count; ++i )
                        {
                            NewClass.Append(fields[i].Split('/')[1]);
                            NewClass.Append(" ");
                            NewClass.Append("_");
                            NewClass.Append(fields[i].Split('/')[0]);
                            if (i < fields.Count - 1) { 
                                NewClass.Append(", ");
                            }
                        }
                        NewClass.Append(")\n{\n");
                        foreach (string field in fields)
                        {
                            NewClass.Append(field.Split('/')[0] + " = _" + field.Split('/')[0]);
                            NewClass.Append(";\n");
                        }

                        NewClass.Append("\n}");
                        NewClass.Append("\n\n");

                        //TODO: Finish Adding in The Database Accessors.
                        /*
                        //With our constructor in place we can go ahead and put in our database accessors.
                        //We will do our get first.
                        NewClass.Append("public bool get");
                        NewClass.Append(tableName);
                        //TODO: This is horrible bad practice.  I'm assuming that our PK is always an int, I need to carry down the type.
                        NewClass.Append("(int _");
                        NewClass.Append(pk);
                        NewClass.Append(")\n{\n");

                        NewClass.Append("}");

                        //Next we can do our write.
                        NewClass.Append("public bool write");
                        NewClass.Append(tableName);
                        //TODO: This is horrible bad practice.  I'm assuming that our PK is always an int, I need to carry down the type.
                        NewClass.Append("(int _");
                        NewClass.Append(pk);
                        NewClass.Append(")\n{\n");
                    
                        NewClass.Append("}");*/

                        //Lets go ahead and close our class
                        NewClass.Append("}\n");

                        //And finally we can close up our namespace
                        NewClass.Append("}");

                        //Now that our whole class is built we can write it to a file.
                        System.IO.File.WriteAllText("/OutputFiles/" + tableName + ".cs", NewClass.ToString());

                        //Now we need to notate that the table was converted to a class.
                        results.Append("<span>Class Generated Successfully:  ");
                        results.Append(tableName);
                        results.Append(".cs ...</span><br />");
                    }


                    //Now that all of our tables are converted we need to zip them so that the user can download the files, and give the user a link to do so.
                    results.Append("</br></br><span>Now zipping files for download...</span><br />");
                    ZipFile.CreateFromDirectory("/OutputFiles/", "/OutputFiles/DBClasses.zip");

                    //At this point all of our files are zipped and ready to go, we can offer them for the user to download.
                    results.Append("</br></br><span>Files Zipped.  Click Below to download.</span><br />");
                    results.Append("<span><a href='/OutputFiles/DBClasses.zip' download>Download Classes...</a>");
                }

                return results.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


    }
}