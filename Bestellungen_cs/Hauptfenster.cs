using System;
using Gtk;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace Bestellungen_cs
{

	public class Hauptfenster
	{
		/*Die Klasse Hauptfenster beinhaltet ein Fenster, das ein Menue enthaelt, ueber das verschiedene
		andere Fenster aufgerufen werden koennen */

		//ein Fenster wird angelegt
		private Window fenster;

		//Menu des Fensters wird angelegt
		private MenuBar mb;
		private Menu person_menu;
		private Menu artikel_menu;
		private Menu bestellung_menu;

		private MenuItem person_bearbeiten;
		private MenuItem person_speichern;
		private MenuItem person_suchen;

		private MenuItem artikel_bearbeiten;
		private MenuItem artikel_speichern;
		private MenuItem artikel_suchen;

		private MenuItem bestellung_bearbeiten;
		private MenuItem bestellung_speichern;
		private MenuItem bestellung_person;
		private MenuItem bestellung_artikel;


		//Konstruktor
		public Hauptfenster () 
		{
			//Anwendung wird initiiert
			Application.Init ();


			this.fenster = new Window ("Bestellungen");
			this.fenster.SetDefaultSize(400, 200);

			//Menue wird zum Fenster hinzugefuegt
			this.mb = new MenuBar();

			this.person_menu = new Menu();
			this.person_bearbeiten = new MenuItem("Personen bearbeiten");
			this.person_bearbeiten.Submenu = person_menu;

			this.person_speichern = new MenuItem("Person speichern");
			this.person_suchen = new MenuItem("Person suchen");

			this.person_menu.Append(person_speichern);
			this.person_menu.Append (person_suchen);

			//Wenn person_speichern angeklickt wird, dann wird die Funktion sp_oeffne_fenster ausgefuehrt
			this.person_speichern.Activated += sp_oeffne_fenster;
			//Wenn person_suchen angeklickt wird, dann wird die Funktion sup_oeffne_fenster ausgefuehrt
			this.person_suchen.Activated += sup_oeffne_fenster;


			this.artikel_menu = new Menu ();
			this.artikel_bearbeiten = new MenuItem ("Artikel bearbeiten");
			this.artikel_bearbeiten.Submenu = artikel_menu;

			this.artikel_speichern = new MenuItem ("Artikel speichern");
			this.artikel_suchen = new MenuItem ("Artikel suchen");

			this.artikel_menu.Append (artikel_speichern);
			this.artikel_menu.Append (artikel_suchen);

			//Wenn artikel_speichern angeklickt wird, dann wird die Funktion sa_oeffne_fenster ausgefuehrt
			this.artikel_speichern.Activated += sa_oeffne_fenster;
			//Wenn artikel_suchen angeklickt wird, dann wird die Funktion sua_oeffne_fenster ausgefuehrt
			this.artikel_suchen.Activated += sua_oeffne_fenster;

			this.bestellung_menu = new Menu ();
			this.bestellung_bearbeiten = new MenuItem ("Bestellungen bearbeiten");
			this.bestellung_bearbeiten.Submenu = bestellung_menu;
			this.bestellung_speichern = new MenuItem ("Bestellung speichern");
			this.bestellung_person = new MenuItem ("Bestellungen einer Person suchen");
			this.bestellung_artikel = new MenuItem ("Bestellungen eines Artikels suchen");

			this.bestellung_menu.Append (bestellung_speichern);
			this.bestellung_menu.Append (bestellung_person);
			this.bestellung_menu.Append (bestellung_artikel);

			//wenn bestellung_speichern angeklickt wird, dann wird die Funktion sb_oeffne_fenster ausgefuehrt
			this.bestellung_speichern.Activated += sb_oeffne_fenster;
			//Wenn bestellung_person angeklickt wird, dann wird die Funktion bp_oeffne_fenster ausgefuehrt
			this.bestellung_person.Activated += bp_oeffne_fenster;
			//Wenn bestellung_artikel angeklickt wird, dann wird die Funktion ba_oeffne_fenster ausgefuehrt
			this.bestellung_artikel.Activated += ba_oeffne_fenster;

			this.mb.Append(person_bearbeiten);	
			this.mb.Append (artikel_bearbeiten);
			this.mb.Append (bestellung_bearbeiten);


			VBox vbox = new VBox(false, 2);
			vbox.PackStart(mb, false, false, 0);

			this.fenster.Add(vbox);

			//Wenn das Schliessen-Symbol angeklickt wird, dann wird die Funktion fenster_schliessen ausgefuehrt
			this.fenster.DeleteEvent += new DeleteEventHandler (fenster_schliessen);
			this.fenster.ShowAll();

			db_anlegen ();

			//Anwendung wird ausgefuehrt
			Application.Run ();

		}

		private void db_anlegen()
		{
			//Verbindung zu mysql wird hergestellt
			string connetionString = "server=localhost;uid=root;pwd=;";
			MySqlConnection conn = new MySqlConnection(connetionString);

			try
			{
				conn.Open();
				MySqlCommand query = conn.CreateCommand();
				//die Datenbank Bestellungen wird erstellt, falls diese noch nicht existiert
				query.CommandText="create database if not exists Bestellungen";
				query.ExecuteNonQuery();
				//Datenbank Bestellungen auswaehlen
				query.CommandText = "use Bestellungen";
				query.ExecuteNonQuery();
				//die Tabelle personen_db wird angelegt, falls sie noch nicht existiert
				query.CommandText = "CREATE TABLE IF NOT EXISTS personen_db ( " +
									"nr INT AUTO_INCREMENT PRIMARY KEY, " +
									"vorname VARCHAR(30), " +
									"nachname VARCHAR(30)," +
									"strasse VARCHAR(30)," +
									"wohnort VARCHAR(30), " +
									"kundennummer VARCHAR(30))";
				query.ExecuteNonQuery();

				//Tabelle artikel_db wird angelegt, falls sie noch nicht existiert
				query.CommandText = "CREATE TABLE IF NOT EXISTS artikel_db (" +
									"nr INT AUTO_INCREMENT PRIMARY KEY," +
									"name VARCHAR(30)," +
									"nummer VARCHAR(30)," +
									"preis FLOAT) ";
				query.ExecuteNonQuery();

				//Tabelle bestellungen_db wird angelegt, falls sie noch nicht existiert
				query.CommandText = "CREATE TABLE IF NOT EXISTS bestellungen_db ( " +
									"nr INT AUTO_INCREMENT PRIMARY KEY," +
									"kundennummer VARCHAR(30)," +
									"artikelnummer VARCHAR(30)," +
									"anzahl FlOAT) ";
				query.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				//mysql-Fehlermeldungen werden ausgegeben
				Console.WriteLine("MySQL Fehlermeldung: " + ex.ToString());
			}

		}

		private void sp_oeffne_fenster(object sender, EventArgs args)
		{
			person_speichern_fenster sp_fenster = new person_speichern_fenster();
		}

		private void sup_oeffne_fenster(object sender, EventArgs args)
		{
			Person_suchen_fenster sup_fenster = new Person_suchen_fenster ();
		}

		private void sa_oeffne_fenster(object sender, EventArgs args)
		{
			Artikel_speichern_fenster sa_fenster = new Artikel_speichern_fenster ();
		}

		private void sua_oeffne_fenster(object sender, EventArgs args)
		{
			Artikel_suchen_fenster sua_fenster = new Artikel_suchen_fenster ();
		}

		private void sb_oeffne_fenster(object sender, EventArgs args)
		{
			Bestellung_speichern_fenster sb_fenster = new Bestellung_speichern_fenster ();
		}

		private void bp_oeffne_fenster(object sender, EventArgs args)
		{
			Bestellung_person_fenster bp_fenster = new Bestellung_person_fenster ();
		}

		private void ba_oeffne_fenster(object sender, EventArgs args)
		{
			Bestellung_artikel_fenster ba_fenster = new Bestellung_artikel_fenster ();
		}


		private static void fenster_schliessen (object o,
			DeleteEventArgs args)
		{
			//Anwendung wird beendet
			Application.Quit ();
			args.RetVal = true;
		}



	}
}

