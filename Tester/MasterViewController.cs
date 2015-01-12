using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using System.Threading.Tasks;
using EvolveQuest.Shared.ViewModels;

namespace Tester
{
  public class Beacon
  {
    public int Phase { get; set; }
    public string Clue { get; set; }
    public string UUID { get; set; }
    public int Major { get; set; }
    public int Minor { get; set; }
    public string Code { get; set; }

    public override string ToString()
    {
      return Major + "," + Minor;
    }
  }
  public partial class MasterViewController : UITableViewController
  {
    DataSource dataSource;

    public MasterViewController(IntPtr handle)
      : base(handle)
    {
      Title = NSBundle.MainBundle.LocalizedString("Master", "Master");

      // Custom initialization
    }

    public int Game { get; set; }


    public override void ViewDidLoad()
    {
      base.ViewDidLoad();
      viewModel = new WelcomeViewModel();
     

      var refresh = new UIBarButtonItem(UIBarButtonSystemItem.Refresh, (sender, args) =>
      {
        LoadData();
      });
      NavigationItem.RightBarButtonItem = refresh;

      TableView.Source = dataSource = new DataSource(this);
      LoadData();

			var location = new CoreLocation.CLLocationManager ();
			location.RequestWhenInUseAuthorization ();
    }


    private WelcomeViewModel viewModel;
    private async Task LoadData()
    {
      this.Title = "Game: " + (this.Game + 1).ToString(); ;
      BigTed.BTProgressHUD.Show("Loading game...");
      await viewModel.ExecuteLoadGameCommand(Game);

      dataSource.Beacons.Clear();

      dataSource.Beacons.Add(new Beacon
        {
          Phase = -1,
          Code = "secret",
          Clue = "secret",
          UUID = viewModel.Game.UUID,
          Major = 9999,
          Minor = 9999
        });

      foreach (var phase in viewModel.Game.Quests)
      {
        foreach(var banana in phase.Beacons)
        {
          dataSource.Beacons.Add(new Beacon
          {
            Phase = phase.Id,
            Code = phase.Code??string.Empty,
            Clue = phase.Clue.Message,
            UUID = viewModel.Game.UUID,
            Major = phase.Major,
            Minor = banana.Minor
          });
        }
      }
      TableView.ReloadData();
      BigTed.BTProgressHUD.Dismiss();
    }

    class DataSource : UITableViewSource
    {
      static readonly NSString CellIdentifier = new NSString("Cell");
      readonly List<Beacon> objects = new List<Beacon>();
      readonly MasterViewController controller;

      public DataSource(MasterViewController controller)
      {
        this.controller = controller;
      }

      public IList<Beacon> Beacons
      {
        get { return objects; }
      }
      // Customize the number of sections in the table view.
      public override nint NumberOfSections(UITableView tableView)
      {
        return 1;
      }

      public override nint RowsInSection(UITableView tableview, nint section)
      {
        return objects.Count;
      }
      // Customize the appearance of table view cells.
      public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
      {
        var cell = (UITableViewCell)tableView.DequeueReusableCell(CellIdentifier, indexPath);

        cell.TextLabel.Text = "Phase: " + objects[indexPath.Row].Phase + " - " + objects[indexPath.Row].ToString();
        cell.DetailTextLabel.Text = objects[indexPath.Row].Clue;
        return cell;
      }
    }

    public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
    {
      if (segue.Identifier == "showDetail")
      {
        var indexPath = TableView.IndexPathForSelectedRow;
        var item = dataSource.Beacons[indexPath.Row];

        ((DetailViewController)segue.DestinationViewController).SetDetailItem(item);
      }
    }
  }
}
