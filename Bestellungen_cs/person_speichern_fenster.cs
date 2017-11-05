using System;
using Gtk;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Bestellungen_cs
{
	public class person_speichern_fenster
	{
		/* Die Klasse person_speichern_fenster beinhaltet ein Fenster, in das die Daten eines Kunden eingegeben werden 
		 * koennen. Wenn der Knopf "Speichern" gedrueckt wurde, dann werden diese Daten in einer Datenbank gespeichert */

		//ein Fenster wird angelegt
		private Window fenster;

		//Knopf, Beschriftungen und Textfelder fuer das Fenster werden angelegt
		private Label ueberschrift_label;
		private Label vorname_label;
		private Entry vorname_text;

		private Label nachname_label;
		private Entry nachname_text;

		private Label strasse_label;
		private Entry strasse_text;

		private Label wohnort_label;
		private Entry wohnort_text;

		private Label kundennummer_label;
		private Entry kundennummer_text;

		private Button bestaetigung;

		//Konstruktor
		public person_speichern_fenster ()
		{
			
			//Knopf, Beschriftungen und Textfelder werden zum Fenster hinzugefuegt
			this.fenster = new Window("Person speichern");
			this.fenster.Resize (400, 400);

			this.ueberschrift_label = new Label ("Personendaten eingeben:");

			this.vorname_label = new Label ("Vorname:");
			this.vorname_text = new Entry ();

			this.nachname_label = new Label ("Nachname:");
			this.nachname_text = new Entry ();

			this.strasse_label = new Label ("Strasse:");
			this.strasse_text = new Entry ();

			this.wohnort_label = new Label ("Wohnort:");
			this.wohnort_text = new Entry ();

			this.kundennummer_label = new Label ("Kundennummer:");
			this.kundennummer_text = new Entry ();

			this.bestaetigung = new Button ("Speichern");
			//Wenn der Knopf bestaetigung gedrueckt wird, dann wird die Funktion speichern ausgefuehrt
			bestaetigung.Clicked += new EventHandler(speichern);


			VBox vbox= new VBox ();

			vbox.Add (ueberschrift_label);
			vbox.Add (vorname_label);
			vbox.Add (vorname_text);
			vbox.Add (nachname_label);
			vbox.Add (nachname_text);
			vbox.Add (strasse_label);
			vbox.Add (strasse_text);
			vbox.Add (wohnort_label);
			vbox.Add (wohnort_text);
			vbox.Add (kundennummer_label);
			vbox.Add (kundennummer_text);
			vbox.Add (bestaetigung);



			this.fenster.Add (vbox);
			this.fenster.ShowAll ();

		}

		private  void speichern(object sender, EventArgs e)
		{
			/* In dieser Funktion werden die Kundendaten, die ins Fenster eingegeben wurden, in eine
			 * Datenbank geschrieben */
			string vorname = this.vorname_text.Text;
			string nachname = this.nachname_text.Text;
			string strasse = this.strasse_text.Text;
			string wohnort = this.wohnort_text.Text;
			string kundennummer = this.kundennummer_text.Text;

			//Verbindung zu mysql wird hergestellt
			string connetionString = "server=localhost;uid=root;pwd=;database=Bestellungen;";
			MySqlConnection conn = new MySqlConnection(connetionString);

			try
			{
				conn.Open();

				MySqlCommand sp_query = conn.CreateCommand();
				//Personendaten, die ins Fenster eingegeben wurden, werden in die Tabelle personen_db geschrieben
				string sp = "insert into personen_db (vorname, nachname, strasse, wohnort, kundennummer) values (?vorname,?nachname,?strasse,?wohnort,?kundennummer)";
				sp_query.CommandText = sp;
				sp_query.Parameters.AddWithValue("?vorname", vorname);
				sp_query.Parameters.AddWithValue("?nachname", nachname);
				sp_query.Parameters.AddWithValue("?strasse", strasse);
				sp_query.Parameters.AddWithValue("?wohnort", wohnort);
				sp_query.Parameters.AddWithValue("?kundennummer", kundennummer);
				sp_query.ExecuteNonQuery();

				conn.Close();
			}
			catch (Exception ex)
			{
				//mysql-Fehlermeldungen werden ausgegeben
				Console.WriteLine("MySQL-Fehlermeldung: " + ex.ToString());
			}

			//Textfelder werden geleert
			this.vorname_text.Text = "";
			this.nachname_text.Text = "";
			this.strasse_text.Text = "";
			this.wohnort_text.Text = "";
			this.kundennummer_text.Text = "";


		}


	}
}

