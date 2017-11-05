using System;
using Gtk;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Bestellungen_cs
{
	public class Artikel_speichern_fenster
	{
		/*die Klasse Artikel_speichern_fenster beinhaltet ein Fenster, in das man 
		 * Artikelname, Artikelnummer und den Preis eines Artikels eingegeben kann.
		 * Wenn der Knopf "Speichern" gedrueckt wird, dann werden diese Artikeldaten
		 * in einer Datenbank gespeichert */

		/*ein Fenster und ein Knopf, Beschriftungen und Textfelder fuer dieses Fenster
		werden angelegt */
		private Window fenster;

		private Label ueberschrift_label;
		private Label artikelname_label;
		private Entry artikelname_text;

		private Label artikelnummer_label;
		private Entry artikelnummer_text;

		private Label preis_label;
		private Entry preis_text;

		private Button bestaetigung;

		//Konstruktor
		public Artikel_speichern_fenster ()
		{

			//Knopf, Beschriftungen und Textfelder werden fuer das Fenster hinzugefuegt
			this.fenster = new Window("Artikel speichern");
			this.fenster.Resize (400, 400);

			this.ueberschrift_label = new Label ("Artikeldaten eingeben:");

			this.artikelname_label = new Label ("Artikelname:");
			this.artikelname_text = new Entry ();

			this.artikelnummer_label = new Label ("Artikelnummer:");
			this.artikelnummer_text = new Entry ();

			this.preis_label = new Label ("Preis:");
			this.preis_text = new Entry ();


			this.bestaetigung = new Button ("Speichern");
			/*Wenn der Knopf bestaetigung gedrueckt wird, dann wird die Funktion
			speichern ausgefuehrt */
			bestaetigung.Clicked += new EventHandler(speichern);


			VBox vbox= new VBox ();

			vbox.Add (ueberschrift_label);
			vbox.Add (artikelname_label);
			vbox.Add (artikelname_text);
			vbox.Add (artikelnummer_label);
			vbox.Add (artikelnummer_text);
			vbox.Add (preis_label);
			vbox.Add (preis_text);
			vbox.Add (bestaetigung);



			this.fenster.Add (vbox);
			this.fenster.ShowAll ();

		}

		private  void speichern(object sender, EventArgs e)
		{
			/*In dieser Funktion werden die Artikeldaten, die in das Fenster eingegeben wurden,
			 * in eine Datenbank geschrieben */
			string aname = this.artikelname_text.Text;
			string anr = this.artikelnummer_text.Text;
			string preis = this.preis_text.Text;

			//Verbindung zu mysql wird hergestellt
			string connetionString = "server=localhost;uid=root;pwd=;database=Bestellungen;";
			MySqlConnection conn = new MySqlConnection(connetionString);

			try
			{
				conn.Open();

				MySqlCommand sp_query = conn.CreateCommand();
				//Artikeldaten werden zur Tabelle artikel_db hinzugefuegt
				string sp = "insert into artikel_db (name,nummer,preis) values (?name,?nummer,?preis)";
				sp_query.CommandText = sp;
				sp_query.Parameters.AddWithValue("?name", aname);
				sp_query.Parameters.AddWithValue("?nummer", anr);
				sp_query.Parameters.AddWithValue("?preis", preis);
				sp_query.ExecuteNonQuery();

				conn.Close();
			}
			catch (Exception ex)
			{
				//mysql-Fehlermeldungen werden ausgegeben
				Console.WriteLine("MySQL-Fehlermeldung: " + ex.ToString());
			}
			//Textfelder werden geleert
			this.artikelname_text.Text = "";
			this.artikelnummer_text.Text = "";
			this.preis_text.Text = "";

		}


	}
}

