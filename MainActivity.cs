using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.FloatingActionButton;
using Newtonsoft.Json;
using System.Collections.Generic;
using AlertDialog = AndroidX.AppCompat.App.AlertDialog;

namespace blood_donor_app
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {   

        RecyclerView donorsRecyclerview;    
        AdapterDonors AdapterDonors;
        List<Donor> listOfDonor = new List<Donor>();
        public Fragments.NewDonorFragment newDonorFragment;
        TextView txtnoDonor;
        ISharedPreferences pref = Application.Context.GetSharedPreferences("donors",FileCreationMode.Private);
        ISharedPreferencesEditor editor;

        // Fragments.NewDonorFragment newDonorFragment;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            SupportActionBar.Title = "Blood Donors";
            txtnoDonor = FindViewById<TextView>(Resource.Id.txtPlaceholder);
            donorsRecyclerview = FindViewById<RecyclerView>(Resource.Id.donorsRecyclerview);
            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += Fab_Click;
            //CreateData();
            RetrieveData();
            if (listOfDonor.Count > 0)
            {
                SetupRecyclerView();
            }
            else
            {
                txtnoDonor.Visibility = Android.Views.ViewStates.Visible;
            }
            
            editor = pref.Edit();


            

        }

        private void Fab_Click(object sender, System.EventArgs e)
        {
            
            newDonorFragment = new Fragments.NewDonorFragment();
            var trans = FragmentManager.BeginTransaction();
            newDonorFragment.Show(trans,"Dialog");
            newDonorFragment.OnDonorRegistered += NewDonorFragment_OnDonorRegistered;

            
        }

        [System.Obsolete]
        private void NewDonorFragment_OnDonorRegistered(object sender, Fragments.NewDonorFragment.DonorDetailsEventArgs e)
        {

            if (newDonorFragment != null)
            {
                newDonorFragment.Dismiss();
                newDonorFragment = null;    
            }
            if (listOfDonor.Count > 0)
            {


                listOfDonor.Insert(0, e.Donor);
                AdapterDonors.NotifyItemInserted(0);

                string jsonString = JsonConvert.SerializeObject(listOfDonor);
                editor.PutString("donors", jsonString);
                editor.Apply();
            }
            else
            {
                listOfDonor.Add(e.Donor);
                string jsonString = JsonConvert.SerializeObject(listOfDonor);
                editor.PutString("donors", jsonString);
                editor.Apply();

                SetupRecyclerView();
            }
        }

        /*
        void CreateData()
        {
            listOfDonor = new List<Donor>();
            listOfDonor.Add(new Donor { BloodGroup = "AB+", City = "Delaware", Country = "USA", FullName = "John Carter", Phone = "08036472649", Email = "jcarte@gmail.com"});
           
        }
        */

        //to put back the shared pref string into the listofdonor
        void RetrieveData()
        {
            string json = pref.GetString("donors", "");
            if (!string.IsNullOrEmpty(json))
            {
                listOfDonor = JsonConvert.DeserializeObject<List<Donor>>(json);
            }
        }

        void SetupRecyclerView()
        {
            donorsRecyclerview.SetLayoutManager(new LinearLayoutManager(this,LinearLayoutManager.Horizontal,false));
            AdapterDonors = new AdapterDonors(listOfDonor);
            AdapterDonors.ItemClick += AdapterDonors_ItemClick;
            AdapterDonors.CallClick += AdapterDonors_CallClick;
            AdapterDonors.MailClick += AdapterDonors_MailClick;
            AdapterDonors.DeleteClick += AdapterDonors_DeleteClick;
            donorsRecyclerview.SetAdapter(AdapterDonors);
        }

        private void AdapterDonors_DeleteClick(object sender, AdapterDonorsClickEventArgs e)
        {
            var donor = listOfDonor[e.Position];
            AlertDialog.Builder deleteAlert = new AlertDialog.Builder(this);
            deleteAlert.SetMessage("Are you sure ?");
            deleteAlert.SetTitle("Delete Donor");
            deleteAlert.SetPositiveButton("Delete", (alert, args) =>
            {
                listOfDonor.RemoveAt(e.Position);
                AdapterDonors.NotifyItemRemoved(e.Position);

                string jsonString = JsonConvert.SerializeObject(listOfDonor);
                editor.PutString("donors", jsonString);
                editor.Apply();
            });

            deleteAlert.SetNegativeButton("Cancel", (alert,args) => { deleteAlert.Dispose(); });    

            deleteAlert.Show(); 

        }

        private void AdapterDonors_MailClick(object sender, AdapterDonorsClickEventArgs e)
        {
            var donor = listOfDonor[e.Position];
            AlertDialog.Builder mailAlert = new AlertDialog.Builder(this);
            mailAlert.SetMessage("Send mail to " + donor.FullName);
            mailAlert.SetPositiveButton("Send mail", (o, e) =>
            {
                //send email
                Intent intent = new Intent();
                intent.SetType("plain/text");
                intent.SetAction(Intent.ActionSend);
                intent.PutExtra(Intent.ExtraEmail, new string[] { donor.Email });
                intent.PutExtra(Intent.ExtraSubject, "Enquiry on your availability for blood donation");
                StartActivity(intent);
            });

            mailAlert.SetNegativeButton("Cancel", (o, e) => {
                mailAlert.Dispose();
            });
            mailAlert.Show();

        }

        private void AdapterDonors_CallClick(object sender, AdapterDonorsClickEventArgs e)
        {
            var donor = listOfDonor[e.Position];
            AlertDialog.Builder callAlert = new AlertDialog.Builder(this);
            callAlert.SetMessage("Call " + donor.FullName);
            callAlert.SetPositiveButton("Call", (o, e) => 
            {
                //call
                var uri = Android.Net.Uri.Parse("tel: " + donor.Phone);
                var intent = new Intent(Intent.ActionDial,uri);
                StartActivity(intent);
            });

            callAlert.SetNegativeButton("Cancel", (o, e) => {
                callAlert.Dispose();
                });
            callAlert.Show();   
        }

        private void AdapterDonors_ItemClick(object sender, AdapterDonorsClickEventArgs e)
        {
            
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}